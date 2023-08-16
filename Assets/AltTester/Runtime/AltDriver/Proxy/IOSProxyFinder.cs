/*
    Copyright(C) 2023 Altom Consulting

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

#if !UNITY_EDITOR && UNITY_IOS
using System;
using System.Runtime.InteropServices;
using System.Globalization;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class IOSProxyFinder : IProxyFinder
    {
        [DllImport("__Internal")]
        private static extern string _getProxy(string uri, string host);

        public string GetProxy(string uri, string host)
        {
            var result = _getProxy(uri, host);
            return result;

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            CultureInfo ci = new CultureInfo("en-US");
            string[] subs = result.Split(';');
            foreach (var sub in subs)
            {
                var rule = sub.Trim();

                if (rule.ToUpper() == "DIRECT") {
                    return null;
                }

                if (rule.StartsWith("PROXY ", false, ci) || rule.StartsWith("SOCKS ", false, ci))
                {
                    return "http://" + rule.Substring(5).Trim();
                }

                if (rule.StartsWith("SOCKS4 ", false, ci) || rule.StartsWith("SOCKS5 ", false, ci))
                {
                    return "http://" + rule.Substring(6).Trim();
                }

                if (sub.StartsWith("HTTP ", false, ci))
                {
                    return "http://" + rule.Substring(4).Trim();
                }

                if (rule.StartsWith("HTTPS ", false, ci))
                {
                    return "https://" + rule.Substring(5).Trim();
                }
            }

            return null;
        }
    }
}
#endif
