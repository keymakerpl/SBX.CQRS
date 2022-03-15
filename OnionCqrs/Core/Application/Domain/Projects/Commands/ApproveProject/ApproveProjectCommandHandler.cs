using Application.Common;
using Application.Domain.SharedKernel;
using CSharpFunctionalExtensions;
using Domain.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Commands.ApproveProject
{
    public class ApproveProjectCommandHandler : ICommandHandler<ApproveProjectCommand, Result>
    {
        private readonly IProjectRepository projectRepository;

        public ApproveProjectCommandHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<Result> Handle(ApproveProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
                .ToResult($"Project not found, id: {request.ProjectId}");

            var nonWorkingDays = project.Bind(project =>
            project.SetNonWorkingDays(NonWorkingDays.PolandNonWorkingDays(DateTime.Now.Year)));
            var approve = project.Bind(project => project.Approve(DateOnly.FromDateTime(DateTime.Now)));

            return await Result.Combine(approve, nonWorkingDays)
                               .Bind(async () => await projectRepository.SaveAsync(project.Value, cancellationToken));
        }
    }
}
