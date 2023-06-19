using System;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class ProxyFinder: IProxyFinder
    {
        public string GetProxy(string uri, string host) {
            // On WebGL proxies are detected and managed by the browser.
            #if !UNITY_EDITOR && UNITY_WEBGL
                return null;
            #endif

            IProxyFinder Finder = null;
            string ProxyUri = null;

            #if !UNITY_EDITOR && UNITY_ANDROID
                Finder = new AndroidProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
            #endif

            #if !UNITY_EDITOR && UNITY_IOS
                Finder = new IOSProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
            #endif

            if (ProxyUri == null) {
                Finder = new EnvironmentProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
            }

            if (ProxyUri == null) {
                Finder = new DotnetProxyFinder();
                ProxyUri = Finder.GetProxy(uri, host);
            }

            return ProxyUri;
        }
    }
}
