/*
    Copyright(C) 2026 Altom Consulting
*/


using AltTester.AltTesterUnitySDK.Commands;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class RuntimeCommunicationHandler : BaseCommunicationHandler
    {
        private ICommandHandler cmdHandler;

        private readonly string path = "/altws/app";


        public ICommandHandler CmdHandler { get { return this.cmdHandler; } }

        public RuntimeCommunicationHandler(string host, int port, string appName, string platform, string platformVersion, string deviceInstanceId, string appID = "unknown")
        {
            this.Host = host;
            this.Port = port;
            this.AppName = appName;
            this.Platform = platform;
            this.PlatformVersion = platformVersion;
            this.DeviceInstanceId = deviceInstanceId;
            this.AppId = appID;
            this.cmdHandler = new CommandHandler();
        }


        public void Init()
        {
            base.Init(path, (code, reason) =>
            {
                if (this.OnDisconnect != null) this.OnDisconnect(code, reason);
            });
            this.cmdHandler.OnSendMessage += this.WsClient.Send;
        }

        protected override void OnMessage(string message)
        {
            this.cmdHandler.OnMessage(message);
        }
    }
}
