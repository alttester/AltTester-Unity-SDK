using System;
using AltTester.AltTesterUnitySDK.Logging;
using AltTester.AltTesterUnitySDK.Driver.Communication;
using AltWebSocketSharp;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class WebSocketClientCommunication : ICommunication
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private readonly AltClientWebSocketHandler websocketHandler;

        WebSocket wsClient;

        private readonly string host;
        private readonly int port;
        private readonly string appName;

        public bool IsConnected { get { return wsClient.IsAlive; } }
        public bool IsListening { get { return false; } }

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }

        public WebSocketClientCommunication(ICommandHandler cmdHandler, string host, int port, string appName)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;

            Uri uri = Utils.CreateURI(host, port, "/altws/app", appName);
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
                throw new UnhandledStartCommError("An error occurred while starting the AltTester client.", ex);
            }
        }
    }
}
