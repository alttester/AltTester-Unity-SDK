/*
    Copyright(C) 2023 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Communication;
using AltTester.AltTesterUnitySDK.Driver.WebSocketClient;
using AltTester.AltTesterUnitySDK.Driver.Proxy;
using AltTester.AltTesterUnitySDK.Logging;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class RuntimeWebSocketClient : IRuntimeWebSocketClient
    {
        private IWebSocketClient wsClient;

        private readonly string host;
        private readonly int port;
        private readonly string appName;

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationDisconnectHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }
        public CommunicationMessageHandler OnMessage { get; set; }

        public bool IsConnected { get { return this.wsClient != null && this.wsClient.IsAlive; } }

        public RuntimeWebSocketClient(string host, int port, string path, string appName)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;

            Uri uri = Utils.CreateURI(host, port, path, appName);
            string proxyUrl = new ProxyFinder().GetProxy(string.Format("http://{0}:{1}", host, port), host);
            wsClient = WebSocketClientFactory.CreateWebSocketClient(uri, null, proxyUrl);

            wsClient.OnOpen += () =>
            {
                if (this.OnConnect != null) this.OnConnect();
            };

            wsClient.OnClose += (code, reason) =>
            {
                if (this.OnDisconnect != null) this.OnDisconnect(code, reason);
            };

            wsClient.OnError += (message) =>
            {
                if (this.OnError != null) this.OnError.Invoke(message, null);
            };

            wsClient.OnMessage += (data) =>
            {
                if (this.OnMessage != null) this.OnMessage.Invoke(System.Text.Encoding.UTF8.GetString(data));
            };
        }

        public void Connect()
        {
            try
            {
                this.wsClient.ConnectAsync();
            }
            catch (Exception ex)
            {
                throw new RuntimeWebSocketClientException("An error occurred while starting the AltTester client.", ex);
            }
        }

        public void Close()
        {
            wsClient.CloseAsync();
        }

        public void Send(string message)
        {
            this.wsClient.Send(message);
        }

        public void Send(byte[] message)
        {
            this.wsClient.Send(message);
        }
    }

#if UNITY_WEBGL
    public class WebGLRuntimeWebSocketClient : IRuntimeWebSocketClient
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        private WebGLWebSocket wsClient;

        private readonly string host;
        private readonly int port;
        private readonly string appName;

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationDisconnectHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }
        public CommunicationMessageHandler OnMessage { get; set; }

        public bool IsConnected { get { return this.wsClient != null && this.wsClient.State == WebSocketState.Open; } }

        public WebGLRuntimeWebSocketClient(string host, int port, string path, string appName)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;

            Uri uri = Utils.CreateURI(host, port, path, appName);
            wsClient = new WebGLWebSocket(uri.ToString());

            wsClient.OnOpen += () =>
            {
                if (this.OnConnect != null) this.OnConnect.Invoke();

                wsClient.OnError += (string errorMsg) =>
                {
                    if (this.OnError != null) this.OnError.Invoke(errorMsg, null);
                };
            };

            wsClient.OnClose += (int closeCode, string reason) =>
            {
                if (this.OnDisconnect != null) this.OnDisconnect.Invoke(closeCode, reason);
            };

            wsClient.OnMessage += (byte[] message) =>
            {
                if (this.OnMessage != null) this.OnMessage.Invoke(Encoding.UTF8.GetString(message));
            };
        }

        public void Connect()
        {
            try
            {
                this.wsClient.Connect().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new RuntimeWebSocketClientException("An error occurred while starting the AltTester client.", ex);
            }
        }

        public void Close()
        {
            try
            {
                this.wsClient.Close().GetAwaiter().GetResult();
            }
            catch (WebSocketInvalidStateException ex)
            {
                logger.Debug(ex.Message);
            }
        }

        public void Send(string message)
        {
            this.wsClient.SendText(message).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void Send(byte[] message)
        {
            this.wsClient.Send(message).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
#endif
}
