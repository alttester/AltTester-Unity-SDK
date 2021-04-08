using NLog;

namespace Altom.AltUnityDriver.Logging
{
    public static class AltUnityLogLevelExtensions
    {
        public static LogLevel ToNLogLevel(this AltUnityLogLevel logLevel)
        {
            return LogLevel.FromOrdinal((int)logLevel);
        }
    }
}