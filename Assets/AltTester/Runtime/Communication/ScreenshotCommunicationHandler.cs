using AltWebSocketSharp;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class ScreenshotCommunicationHandler
    {
        private IRuntimeWebSocketClient wsClient = null;

        private readonly string host;
        private readonly int port;
        private readonly string appName;
        private readonly string path = "/altws/screenshot/app";

        private bool isRunning = false;
        private int quality = 75;
        private int frameRate = 10;

        public bool IsConnected { get { return this.wsClient != null && this.wsClient.IsConnected; } }
        public bool IsRunning { get { return this.isRunning; } }
        public int Quality { get { return this.quality; } }
        public int FrameRate { get { return this.frameRate; } }

        public ScreenshotCommunicationHandler(string host, int port, string appName)
        {
            this.host = host;
            this.port = port;
            this.appName = appName;
        }

        public void Connect()
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
            this.wsClient.Connect();
        }

        public void Close()
        {
            this.wsClient.Close();
        }

        private void OnMessage(string message)
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

        public void SendScreenshot()
        {
            if (this.isRunning)
            {
                this.wsClient.Send(GetScreenshot());
            }
        }

        private byte[] GetScreenshot() {
            var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
            var screenshotSerialized = UnityEngine.ImageConversion.EncodeToJPG(screenshot, quality: this.quality);

            UnityEngine.Debug.LogWarning("ScreenCompressed: " + screenshotSerialized.Length);

            UnityEngine.Object.Destroy(screenshot);
            return screenshotSerialized;
        }
    }
}
