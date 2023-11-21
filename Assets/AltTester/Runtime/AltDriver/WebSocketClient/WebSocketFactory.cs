using System;
using System.Collections.Generic;

namespace AltTester.AltTesterUnitySDK.Driver.WebSocketClient
{
    public class WebSocketClientFactory
    {
        public static IWebSocketClient CreateWebSocketClient(Uri url, Dictionary<string, string> headers = null, string proxyUrl = null, bool useWebSocketSharp = false)
        {
            if (useWebSocketSharp)
            {
                return new WebSocketSharpClient(url, headers, proxyUrl);
            }

            return new WebSocketClient(url, headers, proxyUrl);
        }
    }
}
