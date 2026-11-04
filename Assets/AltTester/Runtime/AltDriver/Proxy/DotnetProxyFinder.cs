/*
    Copyright(C) 2026 Altom Consulting
*/

using System;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class DotnetProxyFinder : IProxyFinder
    {
        public string GetProxy(string uri, string host)
        {
            var Proxy = System.Net.WebRequest.GetSystemWebProxy() as System.Net.WebProxy;
            if (Proxy != null && Proxy.Address != null)
            {
                string proxyUri = Proxy.GetProxy(new Uri(uri)).ToString();

                if (proxyUri != uri)
                {
                    return proxyUri;
                }
            }

            return null;
        }
    }
}
