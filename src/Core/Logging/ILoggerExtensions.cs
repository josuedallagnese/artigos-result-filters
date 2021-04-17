using ResultFilters.Core;

namespace Microsoft.Extensions.Logging
{
    public static class ILoggerExtensions
    {
        public static ILogger LogResponse(this ILogger logger, ApiResponse response)
        {
            if (!logger.IsEnabled(LogLevel.Error))
                return logger;

            if (!response.HasErrors)
                return logger;

            foreach (var error in response.Errors)
            {
                logger.LogError(LoggingEvents.Validations, error);
            }

            return logger;
        }
    }
}
