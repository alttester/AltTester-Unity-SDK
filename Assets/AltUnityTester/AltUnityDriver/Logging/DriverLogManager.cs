using System;
using System.Collections.Generic;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace Altom.AltUnityDriver.Logging
{
    public class DriverLogManager
    {
        const string LOGSFILEPATH = "./AltUnityTesterLog.txt";

        public static LogFactory Instance { get { return instance.Value; } }

        private static readonly Lazy<LogFactory> instance = new Lazy<LogFactory>(buildLogFactory);

        internal static void SetupAltUnityDriverLogging(Dictionary<AltUnityLogger, AltUnityLogLevel> minLogLevels)
        {
            foreach (var key in minLogLevels.Keys)
            {
                SetMinLogLevel(key, minLogLevels[key]);
            }

            Instance.GetCurrentClassLogger().Info(AltUnityLogLevel.Info.ToNLogLevel());
            AltUnityLogLevel level;
            if (minLogLevels.TryGetValue(AltUnityLogger.File, out level) && level != AltUnityLogLevel.Off)
                Instance.GetCurrentClassLogger().Info("AltUnity Tester logs are saved at: " + LOGSFILEPATH);
        }

        /// <summary>
        /// Reconfigures the NLog logging level.
        /// </summary>
        /// <param name="minLogLevel">The <see cref="AltUnityLogLevel" /> to be set.</param>
        public static void SetMinLogLevel(AltUnityLogger loggerType, AltUnityLogLevel minLogLevel)
        {

            foreach (var rule in Instance.Configuration.LoggingRules)
            {
                if (rule.Targets[0].Name == string.Format("AltUnityDriver{0}Target", loggerType))
                {
                    if (minLogLevel == AltUnityLogLevel.Off)
                    {
                        rule.SetLoggingLevels(LogLevel.Off, LogLevel.Off);
                    }
                    else
                    {
                        rule.SetLoggingLevels(minLogLevel.ToNLogLevel(), LogLevel.Fatal);
                    }
                }
            }

            Instance.ReconfigExistingLoggers();
        }

        public static void ResumeLogging()
        {
            Instance.ResumeLogging();
        }

        public static void SuspendLogging()
        {
            Instance.SuspendLogging();
        }

        public static bool IsLoggingEnabled()
        {
            return Instance.IsLoggingEnabled();
        }

        public static void StopLogging()
        {
            while (IsLoggingEnabled())
                SuspendLogging();
        }

        private static LogFactory buildLogFactory()
        {
            var config = new LoggingConfiguration();

#if UNITY_EDITOR || ALTUNITYTESTER
            var unityTarget = new UnityTarget("AltUnityDriverUnityTarget")
            {
                Layout = Layout.FromString("${longdate}|Driver|${level:uppercase=true}|${message}"),
            };
            config.AddRuleForOneLevel(LogLevel.Off, unityTarget);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltUnityServerUnityRule";
#else
            var consoleTarget = new ConsoleTarget("AltUnityDriverConsoleTarget")
            {
                Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${message}")
            };
            config.AddRuleForOneLevel(LogLevel.Off, consoleTarget);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltUnityServerConsoleRule";
#endif

            var logfile = new FileTarget("AltUnityDriverFileTarget")
            {
                FileName = LOGSFILEPATH,
                Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${message}"),
                DeleteOldFileOnStartup = true, //overwrite existing log file.
                KeepFileOpen = true,
                ConcurrentWrites = false
            };
            config.AddRuleForOneLevel(LogLevel.Debug, logfile);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltUnityServerFileRule";

            LogFactory logFactory = new LogFactory
            {
                Configuration = config,
                AutoShutdown = true
            };
            return logFactory;
        }
    }
}