using System;

namespace UmbHost.FallbackTextProperty.Services
{
    public interface IFallbackTextLoggerService
    {
        void LogWarning(Exception exception, string message, params object[] args);
        void LogWarning(string message, params object[] args);
    }
}
