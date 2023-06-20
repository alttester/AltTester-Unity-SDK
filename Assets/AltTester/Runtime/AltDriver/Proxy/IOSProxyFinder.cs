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

            if (result == "")
            {
                return null;
            }

            CultureInfo ci = new CultureInfo("en-US");
            string[] subs = result.Split(';');
            foreach (var sub in subs)
            {
                var rule = sub.Trim();
                if (rule.StartsWith("PROXY", false, ci) || rule.StartsWith("SOCKS", false, ci) || rule.StartsWith("HTTPS", false, ci))
                {
                    return rule.Substring(5).Trim();
                }

                if (sub.StartsWith("HTTP", false, ci))
                {
                    return rule.Substring(4).Trim();
                }
            }

            return null;
        }
    }
}
#endif
