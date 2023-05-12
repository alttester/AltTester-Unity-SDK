using System;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class RuntimeCommunicationHandler
    {
        private IRuntimeWebSocketClient wsClient = null;
        private ICommandHandler cmdHandler;

        private readonly string host;
        private readonly int port;
        private readonly string appName;
        private readonly string path = "/altws/app";

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationDisconnectHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }

        public bool IsConnected { get { return this.wsClient != null && this.wsClient.IsConnected; } }
        public ICommandHandler CmdHandler { get { return this.cmdHandler; } }

        public RuntimeCommunicationHandler(string host, int port, string appName)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;

            this.cmdHandler = new CommandHandler();
        }

        public void Connect()
        {
            #if UNITY_WEBGL
                this.wsClient = new WebGLRuntimeWebSocketClient(this.host, this.port, this.path, this.appName);
            #else
                this.wsClient = new RuntimeWebSocketClient(this.host, this.port, this.path, this.appName);
            #endif

            this.wsClient.OnMessage += (message) =>
            {
                this.OnMessage(message);
            };

            this.wsClient.OnConnect += () =>
            {
                if (this.OnConnect != null) this.OnConnect();
            };

            this.wsClient.OnDisconnect += (code, reason) =>
            {
                if (this.OnDisconnect != null) this.OnDisconnect(code, reason);
            };

            this.wsClient.OnError += (message, exception) =>
            {
                if (this.OnError != null) this.OnError.Invoke(message, exception);
            };

            this.wsClient.Connect();
            this.cmdHandler.OnSendMessage += this.wsClient.Send;
        }

        public void Close()
        {
            this.wsClient.Close();
        }

        private void OnMessage(string message)
        {
            this.cmdHandler.OnMessage(message);
        }
    }
}
