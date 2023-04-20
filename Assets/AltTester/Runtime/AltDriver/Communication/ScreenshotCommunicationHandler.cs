using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltWebSocketSharp;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Driver.Communication {
    public class DriverScreenshotCommunicationHandler
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        private DriverWebSocketClient wsClient = null;

        private event EventHandler<byte[]> OnMessage;

        private string _host;
        private int _port;
        private string _appName;
        private int _connectTimeout;

        private string _uri;

        public string Host { get { return this._host; } }
        public int Port { get { return this._port; } }
        public string AppName { get { return this._appName; } }
        public int ConnectTimeout { get { return this._connectTimeout; } }

        public string URI { get { return this._uri; } }

        public DriverScreenshotCommunicationHandler()
        {
        }

        public void Connect(string host, int port, string appName, int connectTimeout)
        {
            _host = host;
            _port = port;
            _appName = appName;
            _connectTimeout = connectTimeout;

            this.wsClient = new DriverWebSocketClient(_host, _port, "/altws/screenshot", _appName, _connectTimeout);
            this.wsClient.OnMessage += (sender, e) =>
            {
                this.OnMessage.Invoke(this, e.RawData);
            };
            this.wsClient.Connect();
        }

        public void Close()
        {
            logger.Info(string.Format("Closing connection to AltTester on: {0}", this.wsClient.URI));
            this.wsClient.Close();
        }

        public void Start()
        {
            this.wsClient.Send("Start");
        }

        public void Stop()
        {
            this.wsClient.Send("Stop");
        }

        public void UpdateFrameRate(int frameRate)
        {
            this.wsClient.Send(string.Format("FrameRate:{0}", frameRate));
        }

        public void UpdateQuality(int quality)
        {
            this.wsClient.Send(string.Format("Quality:{0}", quality));
        }
    }
}
