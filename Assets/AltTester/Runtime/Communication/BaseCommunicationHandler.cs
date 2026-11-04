/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterUnitySDK.Commands;
using AltWebSocketSharp;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public abstract class BaseCommunicationHandler
    {
        private string host;
        private int port;
        private string appName;
        private string platform;
        private string platformVersion;
        private string deviceInstanceId;
        private string appId;
        private IRuntimeWebSocketClient wsClient = null;



        public CommunicationHandler OnConnect { get; set; }
        public CommunicationDisconnectHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }

        public bool IsConnected { get { return this.WsClient != null && this.WsClient.IsConnected; } }
        public WebSocketState WsClientReadyState { get { return WsClient.ReadyState; } }
        public bool waitingToConnect = true;


        protected string Host { get => host; set => host = value; }
        protected int Port { get => port; set => port = value; }
        protected string AppName { get => appName; set => appName = value; }
        protected string Platform { get => platform; set => platform = value; }
        protected string PlatformVersion { get => platformVersion; set => platformVersion = value; }
        protected string DeviceInstanceId { get => deviceInstanceId; set => deviceInstanceId = value; }
        protected string AppId { get => appId; set => appId = value; }
        protected IRuntimeWebSocketClient WsClient { get => wsClient; set => wsClient = value; }

        public void Close()
        {
            this.WsClient.Close();
        }

        public void Connect()
        {
            this.WsClient.Connect();
        }
        public void Init(string path, CommunicationDisconnectHandler OnDisconnect)
        {
            bool secureMode = false;
            if (AltRunner._altRunner.InstrumentationSettings != null)
            {
                if (AltRunner._altRunner.InstrumentationSettings.SecureMode)
                {
                    secureMode = true;
                }
            }

#if UNITY_WEBGL && !UNITY_EDITOR
                this.wsClient = new WebGLRuntimeWebSocketClient(this.host, this.port, path, this.appName, this.platform, this.platformVersion, this.deviceInstanceId, this.appId, secureMode: secureMode);
#else
            this.WsClient = new RuntimeWebSocketClient(this.Host, this.Port, path, this.AppName, this.Platform, this.PlatformVersion, this.DeviceInstanceId, this.AppId, secureMode: secureMode);
#endif

            this.WsClient.OnConnect += () =>
            {
                if (this.OnConnect != null) this.OnConnect();
            };

            this.WsClient.OnDisconnect += OnDisconnect;

            this.WsClient.OnError += (message, exception) =>
            {
                if (this.OnError != null) this.OnError.Invoke(message, exception);
            };

            this.WsClient.OnMessage += (message) =>
            {
                this.OnMessage(message);
            };
        }

        protected abstract void OnMessage(string message);


    }
}