using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityScreenshotReadyCommand :AltUnityCommand
    {
        UnityEngine.Texture2D screenshot;
        UnityEngine.Vector2 size;

        public AltUnityScreenshotReadyCommand (Texture2D screenshot, Vector2 size)
        {
            this.screenshot = screenshot;
            this.size = size;
        }

        public override string Execute()
        {
                int width = (int)size.x;
                int height = (int)size.y;

                var heightDifference = screenshot.height - height;
                var widthDifference = screenshot.width - width;
                if (heightDifference >= 0 || widthDifference >= 0)
                {
                    if (heightDifference > widthDifference)
                    {
                        width = height * screenshot.width / screenshot.height;
                    }
                    else
                    {
                        height = width * screenshot.height / screenshot.width;
                    }
                }
                string[] fullResponse = new string[5];

                fullResponse[0] = Newtonsoft.Json.JsonConvert.SerializeObject(new UnityEngine.Vector2(screenshot.width, screenshot.height), new Newtonsoft.Json.JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                AltUnityTextureScale.Bilinear(screenshot, width, height);
                screenshot.Apply(true);
                screenshot.Compress(false);
                screenshot.Apply(false);


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
