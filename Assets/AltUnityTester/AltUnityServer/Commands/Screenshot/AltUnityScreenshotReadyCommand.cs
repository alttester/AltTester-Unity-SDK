using System;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityScreenshotReadyCommand : AltUnityCommand
    {
        UnityEngine.Vector2 size;
        int quality;

        public AltUnityScreenshotReadyCommand(params string[] parameters) : base(parameters, 4)
        {
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.quality = Int32.Parse(parameters[3]);
        }

        public override string Execute()
        {
            var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
            int width = (int)size.x;
            int height = (int)size.y;

            quality = UnityEngine.Mathf.Clamp(quality, 1, 100);
            if (width == 0 && height == 0)
            {
                width = screenshot.width;
                height = screenshot.height;
            }
            else
            {
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

            }
            string[] fullResponse = new string[5];

            fullResponse[0] = Newtonsoft.Json.JsonConvert.SerializeObject(new UnityEngine.Vector2(screenshot.width, screenshot.height), new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            width = width * quality / 100;
            height = height * quality / 100;
            AltUnityTextureScale.Bilinear(screenshot, width, height);
            screenshot.Compress(false);
            screenshot.Apply(false);


            var screenshotSerialized = screenshot.GetRawTextureData();
            LogMessage(screenshotSerialized.LongLength + " size after Unity Compression");
            LogMessage(System.DateTime.Now + " Start Compression");
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

            LogMessage(System.DateTime.Now + " Finished Serialize Screenshot Start serialize response");
            LogMessage(System.DateTime.Now + " Finished send Response");
            screenshot.Apply(false, true);
            UnityEngine.GameObject.DestroyImmediate(screenshot);
            AltUnityRunner._altUnityRunner.destroyHightlight = true;
            return Newtonsoft.Json.JsonConvert.SerializeObject(fullResponse);
        }
    }
}
