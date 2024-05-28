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

using AltWebSocketSharp;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class LiveUpdateCommunicationHandler : BaseCommunicationHandler
    {
        private string path = "/altws/live-update/app";

        private int quality = 75;
        private int frameRate = 10;
        private bool isRunning = false;

        public LiveUpdateCommunicationHandler(string host, int port, string appName, string platform, string platformVersion, string deviceInstanceId, string appId)
        {
            this.Host = host;
            this.Port = port;
            this.AppName = appName;
            this.Platform = platform;
            this.PlatformVersion = platformVersion;
            this.DeviceInstanceId = deviceInstanceId;
            this.AppId = appId;
        }

        public bool IsRunning { get { return this.isRunning; } }


        public int Quality { get { return this.quality; } }
        public int FrameRate { get { return this.frameRate; } }

        public void SendScreenshot()
        {
            if (this.IsRunning)
            {
                this.WsClient.Send(GetScreenshot());
            }
        }

        private byte[] GetScreenshot()
        {
            var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
            var screenshotSerialized = UnityEngine.ImageConversion.EncodeToJPG(screenshot, quality: this.quality);

            UnityEngine.Object.Destroy(screenshot);
            return screenshotSerialized;
        }
        public void Init()
        {
            base.Init(path, (code, reason) =>
            {
                this.isRunning = false;
                if (this.OnDisconnect != null) this.OnDisconnect(code, reason);
            });
        }
        protected override void OnMessage(string message)
        {
            if (message == "Start")
            {
                this.isRunning = true;
                return;
            }
            if (message == "Stop")
            {
                this.isRunning = false;
                return;
            }
            if (message.StartsWith("FrameRate:"))
            {
                this.frameRate = int.Parse(message.Substring(message.IndexOf(":") + 1));
                return;
            }
            if (message.StartsWith("Quality:"))
            {
                this.quality = int.Parse(message.Substring(message.IndexOf(":") + 1));
                return;
            }
        }
        public new void Close()
        {
            this.isRunning = false;
            this.WsClient.Close();
        }

        public new void Connect()
        {
            this.isRunning = false;
            this.WsClient.Connect();
        }

    }
}
