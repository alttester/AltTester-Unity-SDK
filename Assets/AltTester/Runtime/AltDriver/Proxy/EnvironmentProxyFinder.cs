using System;
using System.Linq;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class EnvironmentProxyFinder : IProxyFinder
    {
        public string GetProxy(string uri, string host)
        {
            // TODO: Check HTTPS_PROXY if we use wss
            string proxyUrl = GetEnv("HTTP_PROXY");

            if (proxyUrl == null)
            {
                proxyUrl = GetEnv("ALL_PROXY");
            }

            if (proxyUrl != null)
            {
                string exceptions = GetEnv("NO_PROXY");

                if (!string.IsNullOrEmpty(exceptions))
                {
                    var exceptionsList = exceptions.Split(';').ToList<string>();

                    if (exceptionsList.Contains(proxyUrl))
                    {
                        return null;
                    }
                }
            }

            return proxyUrl;
        }

        private string GetEnv(string key)
        {
            return System.Environment.GetEnvironmentVariable(key) ?? System.Environment.GetEnvironmentVariable(key.ToLowerInvariant());
        }
    }
}
