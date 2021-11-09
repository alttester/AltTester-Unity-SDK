using System;
using System.Collections.Generic;
using Altom.AltUnityDriver.Logging;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace Altom.AltUnityTester.Logging
{
    public class ServerLogManager
    {
        public static LogFactory Instance { get { return instance.Value; } }

        private static readonly Lazy<LogFactory> instance = new Lazy<LogFactory>(buildLogFactory);
        private static string logsFilePath = null;

        public static void SetupAltUnityServerLogging(Dictionary<AltUnityLogger, AltUnityLogLevel> minLogLevels)
        {
            foreach (var key in minLogLevels.Keys)
            {
                SetMinLogLevel(key, minLogLevels[key]);
            }

            Instance.GetCurrentClassLogger().Info(AltUnityLogLevel.Info.ToNLogLevel());
            AltUnityLogLevel level;
            if (!string.IsNullOrEmpty(logsFilePath) && minLogLevels.TryGetValue(AltUnityLogger.File, out level) && level != AltUnityLogLevel.Off)
                Instance.GetCurrentClassLogger().Info("AltUnity Tester logs are saved at: " + logsFilePath);
        }


        /// <summary>
        /// Reconfigures the NLog logging level.
        /// </summary>
        /// <param name="minLogLevel">The <see cref="AltUnityLogLevel" /> to be set.</param>
        public static void SetMinLogLevel(AltUnityLogger loggerType, AltUnityLogLevel minLogLevel)
        {
            LogLevel minLevel, maxLevel;
            if (minLogLevel == AltUnityLogLevel.Off)
            {
                minLevel = LogLevel.Off;
                maxLevel = LogLevel.Off;
            }
            else
            {
                minLevel = minLogLevel.ToNLogLevel();
                maxLevel = LogLevel.Fatal;
            }
            bool found = false;
            foreach (var rule in Instance.Configuration.LoggingRules)
            {
                if (rule.Targets[0].Name == string.Format("AltUnityServer{0}Target", loggerType))
                {
                    found = true;
                    rule.SetLoggingLevels(minLevel, maxLevel);
                    rule.RuleName = string.Format("AltUnityServer{0}Rule", loggerType);
                }
            }
            if (!found && loggerType == AltUnityLogger.File)
            {
                addFileLogger(minLevel, maxLevel);
            }

            Instance.ReconfigExistingLoggers();
        }

        private static void addFileLogger(LogLevel minLevel, LogLevel maxLevel)
        {
            logsFilePath = UnityEngine.Application.persistentDataPath + "/AltUnityServerLog.txt";
            var logfile = new FileTarget("AltUnityServerFileTarget")
            {
                FileName = logsFilePath,
                Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${message}"),
                DeleteOldFileOnStartup = true, //overwrite existing log file.
                KeepFileOpen = true,
                ConcurrentWrites = false
            };
            Instance.Configuration.AddRule(minLevel, maxLevel, logfile);
            Instance.Configuration.LoggingRules[Instance.Configuration.LoggingRules.Count - 1].RuleName = "AltUnityServerFileRule";
        }

        private static LogFactory buildLogFactory()
        {
            var config = new LoggingConfiguration();

#if UNITY_EDITOR || ALTUNITYTESTER
            var unitylog = new UnityTarget("AltUnityServerUnityTarget")
            {
                Layout = Layout.FromString("${longdate}|Tester|${level:uppercase=true}|${message}"),
            };
            config.AddRuleForOneLevel(LogLevel.Off, unitylog);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltUnityServerUnityRule";
#else
            var consoleTarget = new ConsoleTarget("AltUnityDriverConsoleTarget")
            {
                Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${message}"),
            };
            config.AddRuleForOneLevel(LogLevel.Off, consoleTarget);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltUnityServerConsoleRule";
#endif

            LogFactory logFactory = new LogFactory
            {
                Configuration = config,
                AutoShutdown = true
            };
            return logFactory;
        }
    }
}