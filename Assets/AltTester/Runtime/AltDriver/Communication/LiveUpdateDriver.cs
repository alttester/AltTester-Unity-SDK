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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Driver.Communication {
    public class LiveUpdateDriver
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        private DriverWebSocketClient wsClient = null;

        public event EventHandler<byte[]> OnMessage;

        private bool isRunning = false;

        public bool IsRunning { get { return this.isRunning; } }
        public bool IsAlive { get { return this.wsClient != null && this.wsClient.IsAlive; } }

        public void Connect(string host, int port, string appName, int connectTimeout)
        {
            this.isRunning = false;
            this.wsClient = new DriverWebSocketClient(host, port, "/altws/live-update", appName, connectTimeout);
            this.wsClient.OnMessage += (sender, e) =>
            {
                this.OnMessage.Invoke(this, e.RawData);
            };
            this.wsClient.Connect();
        }

        public void Close()
        {
            logger.Info(string.Format("Closing connection to AltTester on: '{0}'.", this.wsClient.URI));

            this.isRunning = false;
            this.wsClient.Close();
        }

        public void Start()
        {
            this.wsClient.Send("Start");
            this.isRunning = true;
        }

        public void Stop()
        {
            this.wsClient.Send("Stop");
            this.isRunning = false;
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
