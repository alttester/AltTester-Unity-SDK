/*
    Copyright(C) 2024 Altom Consulting

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
using AltTester.AltTesterUnitySDK.Driver.Logging;
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
