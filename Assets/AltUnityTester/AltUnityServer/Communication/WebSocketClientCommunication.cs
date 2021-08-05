using System;
using WebSocketSharp;
namespace Assets.AltUnityTester.AltUnityServer.Communication
{
    public class WebSocketClientCommunication : ICommunication
    {
        private readonly AltClientWebSocketHandler websocketHandler;
        WebSocket wsClient;
        private readonly int port;
        private readonly string host;
        public WebSocketClientCommunication(string host, int port)
        {
            this.port = port;
            this.host = host;
            Uri uri;
            if (!Uri.TryCreate(string.Format("ws://{0}:{1}/altws/game", host, port), UriKind.Absolute, out uri))
            {
                throw new Exception(String.Format("Invalid host or port {0}:{1}", host, port));
            }
            System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse("0.0.0.0");

            wsClient = new WebSocket(uri.ToString());
            websocketHandler = new AltClientWebSocketHandler(wsClient);


        }
        public bool IsConnected { get { return wsClient.IsAlive; } }
        public bool IsListening { get { return false; } }
        public void Start()
        {
            try
            {
                wsClient.Connect();
            }
            catch (Exception ex)
            {
                throw new UnhandledStartCommError("An error occured while starting AltUnity Server", ex);
            }
        }

        protected void OnMessage()
        {
        }


        public void Stop()
        {
            wsClient.Close();
        }
    }
}