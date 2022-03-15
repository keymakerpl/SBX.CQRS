using Application.Common;
using CSharpFunctionalExtensions;
using Domain.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Commands.UnassignDeveloper
{
    public class UnassignDeveloperCommandHandler : ICommandHandler<UnassignDeveloperCommand, Result>
    {
        private readonly IProjectRepository projectRepository;
        private readonly IDeveloperRepository developerRepository;

        public UnassignDeveloperCommandHandler(IProjectRepository projectRepository, IDeveloperRepository developerRepository)
        {
            this.projectRepository = projectRepository;
            this.developerRepository = developerRepository;
        }

        public async Task<Result> Handle(UnassignDeveloperCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
                .ToResult($"Project not found, id: {request.ProjectId}");

            var developer = await developerRepository.GetByIdAsync(request.DeveloperId, cancellationToken)
                .ToResult($"Developer not found, id: {request.DeveloperId}")
                .Ensure(developer => developer.Project != null, "Developer is not assigned!");

            return await Result.Combine(project, developer)
                               .Ensure(() => developer.Value.Project == project.Value,
                                            "Developer is assigned to diffrent project!")
                               .Bind(() => project.Value.UnassignDeveloper(developer.Value))
                               .Bind(async () => await projectRepository.SaveAsync(project.Value, cancellationToken));
        }
    }
}
