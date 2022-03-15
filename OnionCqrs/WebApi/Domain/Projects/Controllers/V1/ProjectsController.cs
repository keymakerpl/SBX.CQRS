using Application.Domain.Projects;
using Application.Domain.Projects.Commands.AssignDeveloper;
using Application.Domain.Projects.Commands.ChangeStatus;
using Application.Domain.Projects.Commands.RegisterProject;
using Application.Domain.Projects.Queries.GetProjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi.Domain.Projects.Controllers.V1
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProjectsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjectDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProjects() => Ok(await mediator.Send(new GetProjectsQuery()));

        [HttpPost]
        [ProducesResponseType(typeof(ProjectDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterProject([FromBody] RegisterProjectRequest request)
        {
            var createdProject = await mediator.Send(new RegisterProjectCommand(request.Name,
                                                                                request.DevelopersLimit,
                                                                                request.CodeLinesToWrite,
                                                                                DateOnly.FromDateTime(request.StartDate),
                                                                                DateOnly.FromDateTime(request.DeadLine)));

            if (createdProject.IsFailure)
                return Problem(statusCode: StatusCodes.Status422UnprocessableEntity, detail: createdProject.Error);

            return Created(string.Empty, createdProject.Value);
        }

        [HttpPost("{projectId}/developers", Name = "assign-developer")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> AssignDeveloper(long projectId, long developerId)
        {
            var assignedDeveloper = await mediator.Send(new AssignDeveloperCommand(projectId, developerId));

            if (assignedDeveloper.IsFailure)
                return Problem(statusCode: StatusCodes.Status422UnprocessableEntity, detail: assignedDeveloper.Error);

            return Created("", "Assigned");
        }

        [HttpPost("{projectId}/status", Name = "project-status")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> ChangeStatus(long projectId, [FromBody]ChangeStatusRequest changeStatusRequest)
        {
            var changeResult = await mediator.Send(new ChangeStatusCommand(projectId, changeStatusRequest.Status));

            if (changeResult.IsFailure)
                return Problem(statusCode: StatusCodes.Status422UnprocessableEntity, detail: changeResult.Error);

            return Created("", "Status changed");
        }
    }
}
