using System;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class DotnetProxyFinder : IProxyFinder
    {
        public string GetProxy(string uri)
        {
            var Proxy = System.Net.WebRequest.GetSystemWebProxy() as System.Net.WebProxy;
            if (Proxy != null && Proxy.Address != null)
            {
                return Proxy.GetProxy(new Uri(uri)).ToString();
            }

            return null;
        }
    }
}
