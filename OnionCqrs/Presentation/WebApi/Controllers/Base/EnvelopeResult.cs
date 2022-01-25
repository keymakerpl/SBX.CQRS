using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers.Base
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract partial class BaseController
    {
        public sealed class EnvelopeResult : IActionResult
        {
            private readonly Envelope _envelope;
            private readonly int _statusCode;

            public EnvelopeResult(Envelope envelope, HttpStatusCode statusCode)
            {
                _statusCode = (int)statusCode;
                _envelope = envelope;
            }

            public Task ExecuteResultAsync(ActionContext context)
            {
                var objectResult = new ObjectResult(_envelope)
                {
                    StatusCode = _statusCode
                };

                return objectResult.ExecuteResultAsync(context);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}