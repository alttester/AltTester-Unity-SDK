/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.Proxy;
using AltWebSocketSharp;

namespace AltTester.AltTesterUnitySDK.Driver.Communication
{
    public class DriverWebSocketClient
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        private readonly string host;
        private readonly int port;
        private readonly string uri;
        private readonly string appName;
        private readonly int connectTimeout;
        private readonly string platform;
        private readonly string platformVersion;
        private readonly string deviceInstanceId;
        private string appId;
        private string driverType;

        private String error = null;

        private int closeCode = 0;
        private string closeReason = null;

        private ClientWebSocket wsClient = null;
        public event EventHandler<MessageEventArgs> OnMessage;
        public event EventHandler<CloseEventArgs> OnCloseEvent;

        public bool IsAlive { get { return this.wsClient != null && this.wsClient.IsAlive; } }
        public string URI { get { return this.uri; } }
        public bool DriverRegisteredCalled = false;

        public DriverWebSocketClient(string host, int port, string path, string appName, int connectTimeout, string platform, string platformVersion, string deviceInstanceId, string appId, string driverType)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;
            this.connectTimeout = connectTimeout;
            this.platform = platform;
            this.platformVersion = platformVersion;
            this.deviceInstanceId = deviceInstanceId;
            this.appId = appId;
            this.driverType = driverType;

            this.error = null;
            this.closeCode = 0;
            this.closeReason = null;

            this.uri = Utils.CreateURI(host, port, path, appName, platform, platformVersion, deviceInstanceId, appId, driverType).ToString();
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

                if (this.closeCode == 4005)
                {
                    throw new MultipleDriversException(this.closeReason);
                }
                if (closeCode == 4007)
                {
                    throw new MultipleDriversTryingToConnectException(closeReason);
                }

                throw new ConnectionException(string.Format("Connection closed by AltTester(R) Server with reason: {0}.", this.closeReason));
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
            logger.Debug("Connection to AltTester(R) Server closed: [Code:{0}, Reason:{1}].", e.Code, e.Reason);
            OnCloseEvent.Invoke(this, e);
            DriverRegisteredCalled = false;
            this.closeCode = e.Code;
            this.closeReason = e.Reason;
        }

        public void Connect()
        {
            logger.Info("Connecting to: '{0}'.", this.uri);

            int delay = 100;

            this.wsClient = new ClientWebSocket(this.uri);

            string proxyUri = new ProxyFinder().GetProxy(string.Format("http://{0}:{1}", this.host, this.port), this.host);
            if (proxyUri != null)
            {
                wsClient.SetProxy(proxyUri, null, null);
            }

            this.wsClient.OnError += OnError;
            this.wsClient.OnClose += OnClose;
            this.wsClient.OnMessage += (sender, message) => this.OnMessage.Invoke(this, message);

            Stopwatch watch = Stopwatch.StartNew();
            int retries = 0;

            while (this.connectTimeout > watch.Elapsed.TotalSeconds)
            {
                this.error = null;
                this.closeCode = 0;
                this.closeReason = null;

                if (retries > 0)
                {
                    logger.Debug(string.Format("Retrying #{0} to connect to: '{1}'.", retries, this.uri));
                }

                try
                {
                    wsClient.Connect();
                }
                catch (Exception e)
                {
                    logger.Debug(string.Format("Connection error: {0}", e.Message));
                }
                float waitForNotification = 0;
                try
                {
                    while (waitForNotification < 5000)
                    {
                        if (DriverRegisteredCalled)
                        {
                            logger.Debug(string.Format("Connected to: '{0}'.", this.uri));
                            return;
                        }
                        Thread.Sleep(delay);
                        waitForNotification += delay;
                        this.CheckCloseMessage();
                    }
                }
                catch (Exception e)
                {
                    logger.Debug($"Closed connection because {e}");
                }
                retries++;
            }

            this.CheckCloseMessage();
            this.CheckError();

            if (watch.Elapsed.TotalSeconds > this.connectTimeout && !wsClient.IsAlive)
            {
                throw new ConnectionTimeoutException(string.Format("Failed to connect to AltTester(R) Server on host: {0} port: {1}.", this.host, this.port));
            }
            logger.Debug(string.Format("Connected to: '{0}'.", this.uri));

        }

        public void Close()
        {
            logger.Info(string.Format("Closing connection to AltTester(R) Server on: '{0}'.", this.uri));
            DriverRegisteredCalled = false;
            this.wsClient.Close();
        }

        public void Send(string message)
        {
            if (!this.IsAlive)
            {
                logger.Warn("The connection is already closed.");
                return;
            }

            this.wsClient.Send(message);
        }
    }
}
