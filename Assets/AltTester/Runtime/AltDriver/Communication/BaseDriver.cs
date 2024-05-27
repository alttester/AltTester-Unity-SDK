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
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltWebSocketSharp;

namespace AltTester.AltTesterUnitySDK.Driver.Communication
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

        public void Connect(string host, int port, int connectTimeout = 60, string appName = "__default__", string platform = "unknown", string platformVersion = "unknown", string deviceInstanceId = "unknown", string appId = "unknown", string driverType = "unknown")
        {
            this.isRunning = false;
            this.WsClient = new DriverWebSocketClient(host, port, path, appName, connectTimeout, platform, platformVersion, deviceInstanceId, appId, driverType);
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
