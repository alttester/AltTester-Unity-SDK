using System;

using NLog;

using NLog.Layouts;
using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityTesterEditor.Logging
{
    public class EditorLogManager
    {
        public static LogFactory Instance { get { return instance.Value; } }
        private static readonly Lazy<LogFactory> instance = new Lazy<LogFactory>(buildLogFactory);

        private static LogFactory buildLogFactory()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var unitylog = new UnityTarget("AltUnityEditorUnityTarget")
            {
                Layout = Layout.FromString("${longdate}|Editor|${level:uppercase=true}|${message}"),
            };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, unitylog);

            LogFactory logFactory = new LogFactory
            {
                Configuration = config,
                AutoShutdown = true
            };

            return logFactory;
        }
    }
}