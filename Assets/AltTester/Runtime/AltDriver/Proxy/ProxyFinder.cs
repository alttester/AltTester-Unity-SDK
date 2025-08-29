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
using AltTester.AltTesterSDK.Driver.Logging;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class ProxyFinder : IProxyFinder
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        public string GetProxy(string uri, string host)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            // On WebGL Proxies are detected and managed by the browser.
            return null;
#endif

            IProxyFinder Finder = null;
            string ProxyUri = null;

#if !UNITY_EDITOR && UNITY_ANDROID
            try
            {
                Finder = new AndroidProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
            }
            catch (Exception)
            {
                logger.Info("AndroidProxyFinder in ProxyFinder failed to get proxy for uri: {0}, host: {1}", uri, host);
            }
#endif

#if !UNITY_EDITOR && UNITY_IOS
            try
            {
                Finder = new IOSProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
            }
            catch (Exception)
            {
                logger.Info("IOSProxyFinder in ProxyFinder failed to get proxy for uri: {0}, host: {1}", uri, host);
            }
#endif
            if (ProxyUri == null)
            {
                try
                {
                    Finder = new EnvironmentProxyFinder();
                    ProxyUri = Finder.GetProxy(uri, host);
                }
                catch (Exception)
                {
                    logger.Info("EnvironmentProxyFinder in ProxyFinder failed to get proxy for uri: {0}, host: {1}", uri, host);
                }
            }

            if (ProxyUri == null)
            {
                try
                {
                    Finder = new DotnetProxyFinder();
                    ProxyUri = Finder.GetProxy(uri, host);
                }
                catch (Exception)
                {
                    logger.Info("DotnetProxyFinder in ProxyFinder failed to get proxy for uri: {0}, host: {1}", uri, host);
                }
            }
            logger.Info("Using proxy in ProxyFinder: {0} for uri: {1} and host: {2}", ProxyUri, uri, host);
            return ProxyUri;
        }
    }
}
