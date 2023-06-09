using System;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public interface IProxyFinder
    {
        string GetProxy(string uri);
    }
}
