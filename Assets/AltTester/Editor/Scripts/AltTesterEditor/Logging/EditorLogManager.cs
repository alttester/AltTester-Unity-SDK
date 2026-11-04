/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using AltTester.AltTesterSDK.Driver.Logging;
using NLog;
using NLog.Layouts;

namespace AltTester.AltTesterUnitySDK.Editor.Logging
{
    public class EditorLogManager
    {
        public static LogFactory Instance { get { return instance.Value; } }
        private static readonly Lazy<LogFactory> instance = new Lazy<LogFactory>(buildLogFactory);

        private static LogFactory buildLogFactory()
        {
            DriverLogManager.EnsureConfigurationItemFactory();
            var config = new NLog.Config.LoggingConfiguration();
            var unitylog = new UnityTarget("AltEditorUnityTarget")
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
