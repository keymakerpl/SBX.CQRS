using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Persistence.Utils
{
    internal static class ExceptionHandler
    {
        public static Func<Exception, ILogger, string> LogError = (ex, logger) =>
        {
            logger.LogError(ex, message: ex.Message);
            return ex.Message;
        };
    }
}
