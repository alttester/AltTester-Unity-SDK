using System;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class ProxyFinder
    {
        public string GetProxy(string uri) {
            // On WebGL proxies are detected and managed by the browser.
            #if !UNITY_EDITOR && UNITY_WEBGL
                return null;
            #endif

            IProxyFinder Finder = null;
            string ProxyUri = null;

            #if !UNITY_EDITOR && UNITY_ANDROID
                Finder = new AndroidProxyFinder();
                ProxyUri = Finder.GetProxy(uri);
            #endif

            if (ProxyUri == null) {
                Finder = new EnvironmentProxyFinder();
                ProxyUri = Finder.GetProxy(uri);
            }

            if (ProxyUri == null) {
                Finder = new DotnetProxyFinder();
                ProxyUri = Finder.GetProxy(uri);
            }

            return ProxyUri;
        }
    }
}
