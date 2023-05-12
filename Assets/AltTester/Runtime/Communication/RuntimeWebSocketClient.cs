using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Communication;
using AltTester.AltTesterUnitySDK.Logging;
using AltWebSocketSharp;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class RuntimeWebSocketClient : IRuntimeWebSocketClient
    {
        private WebSocket wsClient;

        private readonly string host;
        private readonly int port;
        private readonly string appName;

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }
        public CommunicationMessageHandler OnMessage { get; set; }

        public bool IsConnected { get { return this.wsClient != null && this.wsClient.IsAlive; } }


        public RuntimeWebSocketClient(string host, int port, string path, string appName)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;

            Uri uri = Utils.CreateURI(host, port, path, appName);
            wsClient = new WebSocket(uri.ToString());
            wsClient.WaitTime = TimeSpan.FromSeconds(0.5);
            wsClient.Log.Level = LogLevel.Debug;
            wsClient.OnOpen += (sender, message) =>
            {
                if (this.OnConnect != null) this.OnConnect();
            };

            wsClient.OnClose += (sender, args) =>
            {
                if (this.OnDisconnect != null) this.OnDisconnect();
            };

            wsClient.OnError += (sender, args) =>
            {
                if (this.OnError != null) this.OnError.Invoke(args.Message, args.Exception);
            };

            wsClient.OnMessage += (sender, args) =>
            {
                if (this.OnMessage != null) this.OnMessage.Invoke(args.Data);
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
        public CommunicationHandler OnDisconnect { get; set; }
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

            wsClient.OnClose += (WebSocketCloseCode closeCode) =>
            {
                if (this.OnDisconnect != null) this.OnDisconnect.Invoke();
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
