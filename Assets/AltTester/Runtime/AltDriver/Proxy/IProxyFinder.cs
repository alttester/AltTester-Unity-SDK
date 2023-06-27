using System;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public interface IProxyFinder
    {
        /// <summary>
        /// Finds the appropriate proxy server for the specified URL and host.
        /// </summary>
        /// <param name="uri">The URL for which the proxy server needs to be determined.</param>
        /// <param name="host">The host associated with the URL. This is only for convenience; it is the same string as between :// and the first : or / after that.</param>
        /// <returns>The proxy server uri. If the string is null, no proxies should be used.</returns>
        string GetProxy(string uri, string host);
    }
}
