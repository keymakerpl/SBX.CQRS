using Domain.Errors.Base;
using System;

namespace WebApi.Controllers.Base
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public abstract partial class BaseController
    {
        public sealed class Envelope
        {
            public object Result { get; }
            public string ErrorCode { get; }
            public string ErrorMessage { get; }
            public string InvalidField { get; }
            public DateTime TimeGenerated { get; }

            private Envelope(object result, Error error, string invalidField)
            {
                Result = result;
                ErrorCode = error?.Code;
                ErrorMessage = error?.Message;
                InvalidField = invalidField;
                TimeGenerated = DateTime.UtcNow;
            }

            public static Envelope Ok(object result = null) =>
                new(result, null, null);

            public static Envelope Error(Error error, string invalidField) =>
                new(null, error, invalidField);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}