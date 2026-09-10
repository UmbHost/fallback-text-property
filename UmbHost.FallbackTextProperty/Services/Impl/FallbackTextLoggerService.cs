using System;
using Microsoft.Extensions.Logging;

namespace UmbHost.FallbackTextProperty.Services.Impl
{
    public class FallbackTextLoggerService : IFallbackTextLoggerService
    {
        private readonly ILogger<FallbackTextLoggerService> _logger;

        public FallbackTextLoggerService(ILogger<FallbackTextLoggerService> logger)
        {
            _logger = logger;
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }
    }
}
