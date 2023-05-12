using AltWebSocketSharp;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public class LiveUpdateCommunicationHandler
    {
        private IRuntimeWebSocketClient wsClient = null;

        private readonly string host;
        private readonly int port;
        private readonly string appName;
        private readonly string path = "/altws/live-update/app";

        public CommunicationHandler OnConnect { get; set; }
        public CommunicationDisconnectHandler OnDisconnect { get; set; }
        public CommunicationErrorHandler OnError { get; set; }

        private bool isRunning = false;
        private int quality = 75;
        private int frameRate = 10;

        public bool IsConnected { get { return this.wsClient != null && this.wsClient.IsConnected; } }
        public bool IsRunning { get { return this.isRunning; } }
        public int Quality { get { return this.quality; } }
        public int FrameRate { get { return this.frameRate; } }

        public LiveUpdateCommunicationHandler(string host, int port, string appName)
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

            UnityEngine.Object.Destroy(screenshot);
            return screenshotSerialized;
        }
    }
}
