#if !UNITY_EDITOR && UNITY_IOS
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class IOSProxyFinder : IProxyFinder
    {
        [DllImport("__Internal")]
        private static extern string _getProxy(string uri, string host);

        public string GetProxy(string uri, string host)
        {
            var proxyUrl = _getProxy(uri, host);
            UnityEngine.Debug.Log(">>> " + proxyUrl);

            if (proxyUrl == "")
            {
                return null;
            }

            return proxyUrl;
        }
    }
}
#endif
