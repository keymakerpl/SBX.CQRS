using Application.Common;
using CSharpFunctionalExtensions;
using Domain.Projects;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Commands.AssignDeveloper
{
    public class AssignDeveloperCommandHandler : ICommandHandler<AssignDeveloperCommand, Result>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IDeveloperRepository developerRepository;

        public AssignDeveloperCommandHandler(IProjectRepository projectRepository, IDeveloperRepository developerRepository)
        {
            this.projectRepository = projectRepository;
            this.developerRepository = developerRepository;
        }

        public async Task<Result> Handle(AssignDeveloperCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
                .ToResult($"Project not found, id: {request.ProjectId}");

            var developer = await developerRepository.GetByIdAsync(request.DeveloperId, cancellationToken)
                .ToResult($"Developer not found, id: {request.DeveloperId}")
                .Ensure(developer => developer.Project == null,
                                x => $"Developer is already assigned to project {x.Project.Name}!");

            return await Result.Combine(project, developer)
                               .Bind(() => project.Value.AssignDeveloper(developer.Value))
                               .Bind(async () => await projectRepository.SaveAsync(project.Value, cancellationToken));
        }
    }
}
