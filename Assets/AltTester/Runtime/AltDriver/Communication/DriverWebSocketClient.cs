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

        private readonly string _host;
        private readonly int _port;
        private readonly string _uri;
        private readonly string _appName;
        private readonly int _connectTimeout;

        private String error = null;

        private int closeCode = 0;
        private String closeReason = null;

        private WebSocket wsClient = null;
        // public event EventHandler<ErrorEventArgs> OnError;
        public event EventHandler<MessageEventArgs> OnMessage;
        // public event EventHandler<CloseEventArgs> OnClose;

        public bool IsAlive { get { return this.wsClient != null && this.wsClient.IsAlive; } }
        public string URI { get { return this._uri; } }

        public DriverWebSocketClient(string host, int port, string path, string appName, int connectTimeout)
        {
            _host = host;
            _port = port;
            _appName = appName;

            _uri = Utils.CreateURI(host, port, path, appName).ToString();
            _connectTimeout = connectTimeout;
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

        protected void OnError(object sender, ErrorEventArgs e)
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
            logger.Debug("Connection to AltTester closed: [Code:{0}, Reason:{1}]", e.Code, e.Reason);

            this.closeCode = e.Code;
            this.closeReason = e.Reason;
        }

        public void Connect()
        {
            logger.Info("Connecting to: '{0}'.", _uri);

            int delay = 100;

            this.wsClient = new WebSocket(_uri);
            // this.wsClient.OnError += (sender, error) => this.OnError.Invoke(this, error);
            this.wsClient.OnError += OnError;
            // this.wsClient.OnClose += (sender, closeEventArgs) => this.OnClose.Invoke(this, closeEventArgs);
            this.wsClient.OnClose += OnClose;
            this.wsClient.OnMessage += (sender, message) => this.OnMessage.Invoke(this, message);

            Stopwatch watch = Stopwatch.StartNew();
            int retries = 0;

            while (_connectTimeout > watch.Elapsed.TotalSeconds)
            {
                if (retries > 0)
                {
                    logger.Debug(string.Format("Retrying #{0} to connect to: '{1}'.", retries, _uri));
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

            if (watch.Elapsed.TotalSeconds > _connectTimeout && !wsClient.IsAlive)
            {
                throw new ConnectionTimeoutException(string.Format("Failed to connect to AltServer on host: {0} port: {1}.", _host, _port));
            }

            logger.Debug("Connected to: " + _uri);
        }

        public void Close()
        {
            logger.Info(string.Format("Closing connection to AltServer on: {0}", _uri));
            this.wsClient.Close();
        }

        public void Send(string message) {
            if (this.IsAlive) {
                this.wsClient.Send(message);
            }
        }
    }
}
