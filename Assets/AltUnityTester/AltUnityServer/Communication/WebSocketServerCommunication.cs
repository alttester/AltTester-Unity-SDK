using System;
using WebSocketSharp.Server;
namespace Assets.AltUnityTester.AltUnityServer.Communication
{
    public class WebSocketServerCommunication : ICommunication
    {
        WebSocketServer wsServer;
        private readonly int port;
        private readonly string host;
        public WebSocketServerCommunication(string host, int port)
        {
            this.port = port;
            this.host = host;
            Uri uri;
            if (!Uri.TryCreate(string.Format("ws://{0}:{1}/", host, port), UriKind.Absolute, out uri))
            {
                throw new Exception(String.Format("Invalid host or port {0}:{1}", host, port));
            }
            System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse("0.0.0.0");

            wsServer = new WebSocketServer(uri.ToString());
            wsServer.AddWebSocketService<AltWebSocketHandler>("/altws");
        }
        public bool IsConnected { get { return wsServer.WebSocketServices.SessionCount > 0; } }
        public bool IsListening { get { return wsServer.IsListening; } }
        public void Start()
        {
            try
            {
                wsServer.Start();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Only one usage of each socket address"))
                {
                    throw new AddressInUseCommError("Cannot start AltUnity Server. Another process is listening on port " + port);
                }

                throw new UnhandledStartCommError("An error occured while starting AltUnity Server", ex);
            }
        }

        public void Stop()
        {
            wsServer.Stop();
        }
    }
}