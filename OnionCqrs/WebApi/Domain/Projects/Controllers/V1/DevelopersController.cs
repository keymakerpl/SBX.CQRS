using Application.Domain.Projects;
using Application.Domain.Projects.Commands.RegisterDeveloper;
using Application.Domain.Projects.Queries.GetDevelopers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi.Domain.Projects.Controllers.V1
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class DevelopersController : ControllerBase
    {
        private readonly IMediator mediator;

        public DevelopersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeveloperDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDevelopers() => Ok(await mediator.Send(new GetDevelopersQuery()));

        [HttpPost]
        [ProducesResponseType(typeof(DeveloperDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegisterDeveloper([FromBody] RegisterDeveloperRequest request)
        {
            var createdDeveloper = await mediator.Send(new RegisterDeveloperCommand(request.FirstName,
                                                                                    request.LastName,
                                                                                    request.CodeLinesPerHour,
                                                                                    request.WorkTime));
            if (createdDeveloper.IsFailure)
                return Problem(statusCode: StatusCodes.Status422UnprocessableEntity, detail: createdDeveloper.Error);

            return Created(string.Empty, createdDeveloper.Value);
        }
    }
}