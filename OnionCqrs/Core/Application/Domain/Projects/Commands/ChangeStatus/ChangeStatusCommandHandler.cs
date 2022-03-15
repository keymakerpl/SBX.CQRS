using Application.Common;
using Application.Domain.SharedKernel;
using CSharpFunctionalExtensions;
using Domain.Projects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Commands.ChangeStatus
{
    public class ChangeStatusCommandHandler : ICommandHandler<ChangeStatusCommand, Result>
    {
        private readonly IProjectRepository projectRepository;

        public ChangeStatusCommandHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<Result> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
                .ToResult($"Project not found, id: {request.ProjectId}");

            var setNonWorkingDaysResult = project.Bind(project =>
            project.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(DateTime.Now.Year)));

            Func<Result> changeStatus = (request.Status) switch
            {
                Status.Approve => () => project.Value.Approve(DateOnly.FromDateTime(DateTime.Now)),
                Status.Hold => () => project.Value.Hold(),
                Status.Proceed => () => project.Value.Proceed(),
                Status.Cancell => () => project.Value.Cancell(),
                Status.Done => () => project.Value.Complete(DateOnly.FromDateTime(DateTime.Now)),
                _ => throw new NotImplementedException(),
            };

            return await project.Bind(_ => setNonWorkingDaysResult)
                                .Bind(() => changeStatus())
                                .Bind(async () => await projectRepository.SaveAsync(project.Value, cancellationToken));
        }
    }
}
