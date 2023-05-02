using System;
using System.Threading;
using System.Diagnostics;
using AltWebSocketSharp;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;

namespace AltTester.AltTesterUnitySDK.Driver.Communication {
    public class DriverWebSocketClient
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        private readonly string host;
        private readonly int port;
        private readonly string uri;
        private readonly string appName;
        private readonly int connectTimeout;

        private String error = null;

        private int closeCode = 0;
        private String closeReason = null;

        private WebSocket wsClient = null;
        public event EventHandler<MessageEventArgs> OnMessage;

        public bool IsAlive { get { return this.wsClient != null && this.wsClient.IsAlive; } }
        public string URI { get { return this.uri; } }

        public DriverWebSocketClient(string host, int port, string path, string appName, int connectTimeout)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;
            this.connectTimeout = connectTimeout;

            this.uri = Utils.CreateURI(host, port, path, appName).ToString();
        }

        private void CheckCloseMessage()
        {
            if (this.closeCode != 0 && this.closeReason != null)
            {
                if (this.closeCode == 4001)
                {
                    throw new NoAppConnectedException(this.closeReason);
                }

                if (this.closeCode == 4002)
                {
                    throw new AppDisconnectedException(this.closeReason);
                }
            }
        }

        private void CheckError()
        {
            if (this.error != null)
            {
                throw new ConnectionException(this.error);
            }
        }

        protected void OnError(object sender, AltWebSocketSharp.ErrorEventArgs e)
        {
            logger.Error(e.Message);
            if (e.Exception != null)
            {
                logger.Error(e.Exception);
            }

            this.error = e.Message;
        }

        protected void OnClose(object sender, CloseEventArgs e)
        {
            logger.Debug("Connection to AltTester closed: [Code:{0}, Reason:{1}].", e.Code, e.Reason);

            this.closeCode = e.Code;
            this.closeReason = e.Reason;
        }

        public void Connect()
        {
            logger.Info("Connecting to: '{0}'.", this.uri);

            int delay = 100;

            this.wsClient = new WebSocket(this.uri);
            this.wsClient.OnError += OnError;
            this.wsClient.OnClose += OnClose;
            this.wsClient.OnMessage += (sender, message) => this.OnMessage.Invoke(this, message);

            Stopwatch watch = Stopwatch.StartNew();
            int retries = 0;

            while (this.connectTimeout > watch.Elapsed.TotalSeconds)
            {
                if (retries > 0)
                {
                    logger.Debug(string.Format("Retrying #{0} to connect to: '{1}'.", retries, this.uri));
                }
                wsClient.Connect();

                if (wsClient.IsAlive)
                {
                    break;
                }

                retries++;
                Thread.Sleep(delay); // Delay between retries.
            }

            this.CheckCloseMessage();
            this.CheckError();

            if (watch.Elapsed.TotalSeconds > this.connectTimeout && !wsClient.IsAlive)
            {
                throw new ConnectionTimeoutException(string.Format("Failed to connect to AltServer on host: {0} port: {1}.", this.host, this.port));
            }

            logger.Debug(string.Format("Connected to: '{0}'.", this.uri));
        }

        public void Close()
        {
            logger.Info(string.Format("Closing connection to AltServer on: '{0}'.", this.uri));
            this.wsClient.Close();
        }

        public void Send(string message) {
            if (!this.IsAlive) {
                logger.Warn("The connection is already closed.");
                return;
            }

            this.wsClient.Send(message);
        }
    }
}
