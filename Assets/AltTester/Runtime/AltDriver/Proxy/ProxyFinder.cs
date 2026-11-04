/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
#if UNITY_IOS || UNITY_ANDROID
using UnityEngine;
#endif

namespace AltTester.AltTesterSDK.Driver.Proxy
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
                Debug.Log("[AltTester][ProxyFinder] Trying AndroidProxyFinder");
                Finder = new AndroidProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
                Debug.Log("[AltTester][ProxyFinder] AndroidProxyFinder result: " + (ProxyUri != null ? "proxy found" : "no proxy"));
            }
            catch (Exception e)
            {
                Debug.LogError("[AltTester][ProxyFinder] AndroidProxyFinder threw: " + e);
            }
#endif

#if !UNITY_EDITOR && UNITY_IOS
            try
            {
                Debug.Log("[AltTester][ProxyFinder] Trying IOSProxyFinder");
                Finder = new IOSProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
                Debug.Log("[AltTester][ProxyFinder] IOSProxyFinder result: " + (ProxyUri != null ? "proxy found" : "no proxy"));
            }
            catch (Exception e)
            {
                Debug.LogError("[AltTester][ProxyFinder] IOSProxyFinder threw: " + e);
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
