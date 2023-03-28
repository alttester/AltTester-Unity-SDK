using System;

namespace AltTester.AltDriver.Proxy
{
    public interface IProxyFinder
    {
        string GetProxy(string uri);
    }
}
