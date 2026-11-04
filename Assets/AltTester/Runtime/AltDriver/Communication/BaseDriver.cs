/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Diagnostics;
using AltTester.AltTesterSDK.Driver.Logging;
using AltWebSocketSharp;

namespace AltTester.AltTesterSDK.Driver.Communication
{
    public class BaseDriver
    {
        protected static readonly NLog.Logger Logger = DriverLogManager.Instance.GetCurrentClassLogger();

        protected DriverWebSocketClient WsClient = null;

        protected bool isRunning = false;

        public bool IsRunning { get { return this.isRunning; } }
        public bool IsAlive { get { return this.WsClient != null && this.WsClient.IsAlive; } }

        public event EventHandler<byte[]> OnMessage;
        public event EventHandler<String> OnMessageData;
        public event EventHandler<CloseEventArgs> OnCloseEvent;
        protected string path;

        public BaseDriver(string path)
        {
            this.path = path;
        }

        public void Close()
        {
            Logger.Info(string.Format("Closing connection to AltTester(R) on: '{0}'.", this.WsClient.URI));

            this.isRunning = false;
            this.WsClient.Close();
        }

        public void Connect(string host, int port, int connectTimeout = 60, string appName = "__default__", string platform = "unknown", string platformVersion = "unknown", string deviceInstanceId = "unknown", string appId = "unknown", string driverType = "unknown", bool secureMode = false)
        {
            this.isRunning = false;
            this.WsClient = new DriverWebSocketClient(host, port, path, appName, connectTimeout, platform, platformVersion, deviceInstanceId, appId, driverType, secureMode);
            this.WsClient.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    if (e.Data.Contains("driverRegistered"))
                    {
                        WsClient.DriverRegisteredCalled = true;
                        return;
                    }
                    OnMessageData.Invoke(this, e.Data);
                    return;
                }
                this.OnMessage.Invoke(this, e.RawData);
            };
            this.WsClient.OnCloseEvent += (sender, data) =>
            {
                OnCloseEvent.Invoke(this, data);
            };
            this.WsClient.Connect();
        }



    }
}
