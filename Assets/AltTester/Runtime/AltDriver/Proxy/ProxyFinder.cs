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

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class ProxyFinder : IProxyFinder
    {
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
                }
            }

            return ProxyUri;
        }
    }
}
