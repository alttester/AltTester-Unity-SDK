/*
    Copyright(C) 2023 Altom Consulting

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
using AltWebSocketSharp;

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


        public void Init()
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

            this.cmdHandler.OnSendMessage += this.wsClient.Send;
        }
        public void Connect()
        {
            this.wsClient.Connect();
        }

        public void Close()
        {
            UnityEngine.Debug.Log("Closing Websocket");
            this.wsClient.Close();
            UnityEngine.Debug.Log("Closed Websocket");
        }

        private void OnMessage(string message)
        {
            this.cmdHandler.OnMessage(message);
        }
    }
}
