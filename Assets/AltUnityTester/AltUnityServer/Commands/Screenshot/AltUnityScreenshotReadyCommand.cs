using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityScreenshotReadyCommand : AltUnityCommand
    {
        UnityEngine.Texture2D screenshot;
        UnityEngine.Vector2 size;
        int quality;

        public AltUnityScreenshotReadyCommand(Texture2D screenshot, int quality, Vector2 size)
        {
            this.screenshot = screenshot;
            this.size = size;
            this.quality = quality;
        }

        public override string Execute()
        {
            int width = (int)size.x;
            int height = (int)size.y;

            quality = UnityEngine.Mathf.Clamp(quality, 1, 100);
            if (width == 0 && height == 0)
            {
                width = screenshot.width;
                height = screenshot.height;
            }

            width = width * quality / 100;
            height = height * quality / 100;
            AltUnityTextureScale.Bilinear(screenshot, width, height);
            screenshot.Apply(true);
                
            screenshot.Compress(false);
            screenshot.Apply(false);

            string[] fullResponse = new string[5];

            fullResponse[0] = Newtonsoft.Json.JsonConvert.SerializeObject(new UnityEngine.Vector2(screenshot.width, screenshot.height), new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            var screenshotSerialized = screenshot.GetRawTextureData();
            AltUnityRunner._altUnityRunner.LogMessage(screenshotSerialized.LongLength + " size after Unity Compression");
            AltUnityRunner._altUnityRunner.LogMessage(System.DateTime.Now + " Start Compression");
            var screenshotCompressed = AltUnityRunner.CompressScreenshot(screenshotSerialized);
            UnityEngine.Debug.Log(System.DateTime.Now + " Finished Compression");
            var length = screenshotCompressed.LongLength;
            fullResponse[1] = length.ToString();

            var format = screenshot.format;
            fullResponse[2] = format.ToString();

            var newSize = new UnityEngine.Vector3(screenshot.width, screenshot.height);
            fullResponse[3] = Newtonsoft.Json.JsonConvert.SerializeObject(newSize, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            UnityEngine.Debug.Log(System.DateTime.Now + " Serialize screenshot");
            fullResponse[4] = Newtonsoft.Json.JsonConvert.SerializeObject(screenshotCompressed, new Newtonsoft.Json.JsonSerializerSettings
            {
                StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii
            });

            AltUnityRunner._altUnityRunner.LogMessage(System.DateTime.Now + " Finished Serialize Screenshot Start serialize response");
            AltUnityRunner._altUnityRunner.LogMessage(System.DateTime.Now + " Finished send Response");
            UnityEngine.GameObject.Destroy(screenshot);
            AltUnityRunner._altUnityRunner.destroyHightlight = true;
            return Newtonsoft.Json.JsonConvert.SerializeObject(fullResponse);
        }
    }
}
