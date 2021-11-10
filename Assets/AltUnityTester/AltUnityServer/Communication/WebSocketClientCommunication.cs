using System;
using Altom.AltUnityTester.Logging;
using WebSocketSharp;

namespace Altom.AltUnityTester.Communication
{
    public class WebSocketClientCommunication : ICommunication
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private readonly AltClientWebSocketHandler websocketHandler;
        WebSocket wsClient;
        private readonly int port;
        private readonly string host;

        public WebSocketClientCommunication(ICommandHandler cmdHandler, string host, int port)
        {
            this.port = port;
            this.host = host;
            Uri uri;
            if (!Uri.TryCreate(string.Format("ws://{0}:{1}/altws/game", host, port), UriKind.Absolute, out uri))
            {
                throw new Exception(String.Format("Invalid host or port {0}:{1}", host, port));
            }

            wsClient = new WebSocket(uri.ToString());
            wsClient.Log.Level = LogLevel.Fatal;
            websocketHandler = new AltClientWebSocketHandler(wsClient, cmdHandler);
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
        }

        public bool IsConnected { get { return wsClient.IsAlive; } }
        public bool IsListening { get { return false; } }

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }

        public void Start()
        {
            connect();
        }

        public void Stop()
        {
            wsClient.Close();
        }

        private void connect()
        {
            try
            {
                wsClient.ConnectAsync();
            }
            catch (Exception ex)
            {
                throw new UnhandledStartCommError("An error occurred while starting the CommunicationProtocol Proxy mode.", ex);
            }
        }
    }
}
