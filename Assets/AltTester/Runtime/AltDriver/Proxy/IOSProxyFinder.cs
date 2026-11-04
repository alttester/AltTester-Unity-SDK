/*
    Copyright(C) 2026 Altom Consulting
*/

#if !UNITY_EDITOR && UNITY_IOS
using System;
using System.Runtime.InteropServices;
using System.Globalization;
using UnityEngine;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class IOSProxyFinder : IProxyFinder
    {
        [DllImport("__Internal")]
        private static extern string _getProxy(string uri, string host);

        public string GetProxy(string uri, string host)
        {
            Debug.Log("[AltTester][IOSProxyFinder] GetProxy called");
            var result = _getProxy(uri, host);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            return result;
        }
    }
}
#endif
