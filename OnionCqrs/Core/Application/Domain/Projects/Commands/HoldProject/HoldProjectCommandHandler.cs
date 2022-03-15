using Application.Common;
using CSharpFunctionalExtensions;
using Domain.Projects;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Commands.HoldProject
{
    public class HoldProjectCommandHandler : ICommandHandler<HoldProjectCommand, Result>
    {
        private readonly IProjectRepository projectRepository;

        public HoldProjectCommandHandler(IProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<Result> Handle(HoldProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
                .ToResult($"Project not found, id: {request.ProjectId}");

            return await project.Bind(project => project.Hold())
                                .Bind(async () => await projectRepository.SaveAsync(project.Value, cancellationToken));
        }
    }
}
