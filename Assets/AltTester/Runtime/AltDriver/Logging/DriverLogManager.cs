/*
    Copyright(C) 2026 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace AltTester.AltTesterSDK.Driver.Logging
{
    public class DriverLogManager
    {
        const string LOGSFILEPATH = "AltTester.log";

        public static LogFactory Instance { get { return instance.Value; } }
#if UNITY_6000_5_OR_NEWER
        private static readonly Lazy<bool> nlogBuiltinItemsRegistered = new Lazy<bool>(() =>
        {
            ConfigurationItemFactory.Default = new ConfigurationItemFactory(typeof(LogManager).Assembly);
            return true;
        });
#endif

        private static readonly Lazy<LogFactory> instance = new Lazy<LogFactory>(buildLogFactory);

        /// <summary>
        /// Registers NLog's built-in configuration items before the first layout or target is created.
        ///
        /// Unity 6000.5 resolves NLog's assembly location to the host executable path, so NLog's scan
        /// for optional NLog.*.dll extension assemblies calls Directory.GetFiles on a file and throws
        /// IOException while building ConfigurationItemFactory.Default. Creating any layout or target
        /// resolves that factory implicitly, so the exception escapes the static constructors that
        /// reach the log managers (AltRunner, AltBuilder) and permanently poisons those types.
        ///
        /// The SDK ships no NLog extension assemblies, so nothing is lost by registering the built-in
        /// items directly. No-op on every other Unity version and outside Unity.
        ///
        /// Must go through the setter only: reading ConfigurationItemFactory.Default before it's set
        /// triggers NLog's own lazy default-factory construction, which is the exact bug this avoids.
        /// </summary>
        public static void EnsureConfigurationItemFactory()
        {
#if UNITY_6000_5_OR_NEWER
            _ = nlogBuiltinItemsRegistered.Value;
#endif
        }

        internal static void SetupAltDriverLogging(Dictionary<AltLogger, AltLogLevel> minLogLevels)
        {
            foreach (var key in minLogLevels.Keys)
            {
                SetMinLogLevel(key, minLogLevels[key]);
            }

            Instance.GetCurrentClassLogger().Info(AltLogLevel.Info.ToNLogLevel());
            AltLogLevel level;
            if (minLogLevels.TryGetValue(AltLogger.File, out level) && level != AltLogLevel.Off)
                Instance.GetCurrentClassLogger().Info("AltTester(R) logs are saved at: " + Path.Combine(System.Environment.CurrentDirectory, LOGSFILEPATH));
        }

        /// <summary>
        /// Reconfigures the NLog logging level.
        /// </summary>
        /// <param name="minLogLevel">The <see cref="AltLogLevel" /> to be set.</param>
        public static void SetMinLogLevel(AltLogger loggerType, AltLogLevel minLogLevel)
        {

            foreach (var rule in Instance.Configuration.LoggingRules)
            {
                if (rule.Targets[0].Name == string.Format("AltDriver{0}Target", loggerType))
                {
                    if (minLogLevel == AltLogLevel.Off)
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
            EnsureConfigurationItemFactory();
            var config = new LoggingConfiguration();

#if UNITY_EDITOR || ALTTESTER
            var unityTarget = new UnityTarget("AltDriverUnityTarget")
            {
                Layout = Layout.FromString("${longdate}|Driver|${level:uppercase=true}|${message}"),
            };
            config.AddRuleForOneLevel(LogLevel.Off, unityTarget);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltServerUnityRule";
#else
            var consoleTarget = new ConsoleTarget("AltDriverConsoleTarget")
            {
                Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${message}")
            };
            config.AddRuleForOneLevel(LogLevel.Off, consoleTarget);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltServerConsoleRule";
#endif
            var path = Path.Combine(System.Environment.CurrentDirectory, LOGSFILEPATH);
            var logfile = new FileTarget("AltDriverFileTarget")
            {
                FileName = path,
                Layout = Layout.FromString("${longdate}|${level:uppercase=true}|${message}"),
                DeleteOldFileOnStartup = true, //overwrite existing log file.
                KeepFileOpen = true,
                ConcurrentWrites = false
            };
            config.AddRuleForOneLevel(LogLevel.Debug, logfile);
            config.LoggingRules[config.LoggingRules.Count - 1].RuleName = "AltServerFileRule";

            LogFactory logFactory = new LogFactory
            {
                Configuration = config,
                AutoShutdown = true
            };
            return logFactory;
        }
    }
}
