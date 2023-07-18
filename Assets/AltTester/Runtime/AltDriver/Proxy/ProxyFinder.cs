using System;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class ProxyFinder : IProxyFinder
    {
        public string GetProxy(string uri, string host)
        {
            // On WebGL proxies are detected and managed by the browser.
#if !UNITY_EDITOR && UNITY_WEBGL
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
                return null;
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
                return null;
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
                    return null;
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
                    return null;
                }
            }

            return ProxyUri;
        }
    }
}
