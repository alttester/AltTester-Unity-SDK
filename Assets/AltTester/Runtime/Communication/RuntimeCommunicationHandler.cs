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
