using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltWebSocketSharp;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Driver.Communication {
    public class ScreenshotCommunicationHandler
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        private DriverWebSocketClient wsClient = null;

        public event EventHandler<byte[]> OnMessage;

        private readonly string _host;
        private readonly int _port;
        private readonly string _appName;
        private readonly int _connectTimeout;

        private readonly string _uri;

        public ScreenshotCommunicationHandler(string host, int port, int connectTimeout, string appName)
        {
            _host = host;
            _port = port;
            _appName = appName;
            _connectTimeout = connectTimeout;
        }

        public void Connect()
        {
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
