/*
    Copyright(C) 2025 Altom Consulting

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
using System.Linq;
using AltTester.AltTesterSDK.Driver.Logging;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class EnvironmentProxyFinder : IProxyFinder
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        public string GetProxy(string uri, string host)
        {
            // TODO: Check HTTPS_PROXY if we use wss
            string proxyUrl = GetEnv("HTTP_PROXY");

            if (proxyUrl == null)
            {
                proxyUrl = GetEnv("ALL_PROXY");
            }

            if (proxyUrl != null)
            {
                string exceptions = GetEnv("NO_PROXY");

                if (!string.IsNullOrEmpty(exceptions))
                {
                    logger.Info("NO_PROXY environment variable found in EnvironmentProxyFinder: {0}", exceptions);
                    var exceptionsList = exceptions.Split(';').ToList<string>();

                    if (exceptionsList.Contains(proxyUrl))
                    {
                        logger.Info("Proxy {0} is in NO_PROXY list, skipping in EnvironmentProxyFinder.", proxyUrl);
                        return null;
                    }
                }
            }
            logger.Info("Using proxy in EnvironmentProxyFinder: {0} for uri: {1} and host: {2}", proxyUrl, uri, host);
            return proxyUrl;
        }

        private string GetEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key) ?? System.Environment.GetEnvironmentVariable(key.ToLowerInvariant());
        }
    }
}
