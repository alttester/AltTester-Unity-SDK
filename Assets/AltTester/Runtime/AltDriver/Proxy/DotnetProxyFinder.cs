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
using AltWebSocketSharp;
using AltTester.AltTesterSDK.Driver.Logging;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class DotnetProxyFinder : IProxyFinder
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        public string GetProxy(string uri, string host)
        {
            logger.Info("DotnetProxyFinder.GetProxy called with uri: {0}, host: {1}", uri, host);
            var Proxy = System.Net.WebRequest.GetSystemWebProxy() as System.Net.WebProxy;
            if (Proxy != null && Proxy.Address != null)
            {
                string proxyUri = Proxy.GetProxy(new Uri(uri)).ToString();

                if (proxyUri != uri)
                {
                    return proxyUri;
                }
            }
            logger.Info("No proxy found for in DotnetProxyFinder uri: {0} and host: {1}", uri, host);
            return null;
        }
    }
}
