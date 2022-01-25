using CSharpFunctionalExtensions;
using Domain.Errors.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace WebApi.Controllers.Base
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract partial class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected new IActionResult Ok(object result = null) =>
            new EnvelopeResult(Envelope.Ok(result), HttpStatusCode.OK);

        protected IActionResult NotFound(Error error, string invalidField = null) =>
            new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.NotFound);

        protected IActionResult Error(Error error, string invalidField = null) =>
            new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.BadRequest);

        protected IActionResult FromResult<T>(Result<T, Error> result) =>
            result.IsSuccess ? Ok(result.Value) : Error(result.Error);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}