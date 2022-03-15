using Application.Common;
using Application.Domain.SharedKernel;
using CSharpFunctionalExtensions;
using Domain.Projects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Commands.RegisterProject
{
    public class RegisterProjectCommandHandler : ICommandHandler<RegisterProjectCommand, Result<ProjectDto>>
    {
        private readonly IProjectRepository projectRepository;
        private readonly ILogger<RegisterProjectCommandHandler> logger;

        public RegisterProjectCommandHandler(IProjectRepository projectRepository, ILogger<RegisterProjectCommandHandler> logger)
        {
            this.projectRepository = projectRepository;
            this.logger = logger;
        }

        public async Task<Result<ProjectDto>> Handle(RegisterProjectCommand request, CancellationToken cancellationToken) =>
            await Project.Create(request.Name,
                                 request.StartDate,
                                 request.DeadLine,
                                 request.DevelopersLimit,
                                 request.CodeLinesToWrite)
                         .Bind(async entity => await projectRepository.SaveAsync(entity, cancellationToken)
                         .OnFailure(error => logger.LogError(error))
                         .Map(() => new ProjectDto
                         {
                             Name = entity.Name,
                             StartDate = entity.StartDate,
                             DeadLine = entity.DeadLine,
                             CodeLinesToWrite = entity.CodeLinesToWrite,
                             DevelopersLimit = entity.DevelopersLimit,
                             Id = entity.Id,
                         }));
    }
}
