using System;
using System.Linq;
using Altom.Server.Logging;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public abstract class AltUnityBaseScreenshotCommand : AltUnityCommand
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private readonly AltClientSocketHandler handler;

        protected AltUnityBaseScreenshotCommand(AltClientSocketHandler handler, string[] parameters, int expectedParametersCount) : base(parameters, expectedParametersCount)
        {
            this.handler = handler;
        }

        public abstract override string Execute();


        protected System.Collections.IEnumerator SendTexturedScreenshotCoroutine(UnityEngine.Vector2 size, int quality)
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            sendTexturedScreenshotResponse(size, quality);
        }

        protected System.Collections.IEnumerator SendPNGScreenshotCoroutine()
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            var response = ExecuteHandleErrors(() => getPNGScreenshot());
            handler.SendResponse(MessageId, CommandName, response.Item1, response.Item2);
        }

        protected System.Collections.IEnumerator SendScreenshotObjectHighlightedCoroutine(UnityEngine.Vector2 size, int quality, UnityEngine.GameObject gameObject, UnityEngine.Color color, float width)
        {
            UnityEngine.Renderer renderer = gameObject.GetComponent<UnityEngine.Renderer>();
            if (renderer != null)
            {
                var originalMaterials = renderer.materials.ToArray();
                renderer.materials = new UnityEngine.Material[renderer.materials.Length];
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.materials[i] = new UnityEngine.Material(originalMaterials[i]);
                    renderer.materials[i].shader = AltUnityRunner._altUnityRunner.outlineShader;
                    renderer.materials[i].SetColor("_OutlineColor", color);
                    renderer.materials[i].SetFloat("_OutlineWidth", width);
                }
                yield return new UnityEngine.WaitForEndOfFrame();
                sendTexturedScreenshotResponse(size, quality);

                renderer.materials = originalMaterials;
            }
            else
            {
                var rectTransform = gameObject.GetComponent<UnityEngine.RectTransform>();
                if (rectTransform != null)
                {
                    var panelHighlight = UnityEngine.Object.Instantiate(AltUnityRunner._altUnityRunner.panelHightlightPrefab, rectTransform);
                    panelHighlight.GetComponent<UnityEngine.UI.Image>().color = color;

                    yield return new UnityEngine.WaitForEndOfFrame();
                    sendTexturedScreenshotResponse(size, quality);

                    UnityEngine.Object.Destroy(panelHighlight);
                }
                else
                {
                    yield return new UnityEngine.WaitForEndOfFrame();
                    sendTexturedScreenshotResponse(size, quality);
                }
            }
        }

        private void sendTexturedScreenshotResponse(UnityEngine.Vector2 size, int quality)
        {
            var response = ExecuteHandleErrors(() => getTexturedScreenshot(size, quality));
            handler.SendResponse(this.MessageId, this.CommandName, response.Item1, response.Item2);
        }

        private string getTexturedScreenshot(UnityEngine.Vector2 size, int quality)
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

            fullResponse[0] = JsonConvert.SerializeObject(new UnityEngine.Vector2(screenshot.width, screenshot.height), new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            width = width * quality / 100;
            height = height * quality / 100;
            AltUnityTextureScale.Bilinear(screenshot, width, height);
            screenshot.Compress(false);
            screenshot.Apply(false);

            var screenshotSerialized = screenshot.GetRawTextureData();

            logger.Trace("Start Compression");
            var screenshotCompressed = AltUnityRunner.CompressScreenshot(screenshotSerialized);
            logger.Trace("Finished Compression");

            var length = screenshotCompressed.LongLength;
            fullResponse[1] = length.ToString();

            var format = screenshot.format;
            fullResponse[2] = format.ToString();

            var newSize = new UnityEngine.Vector3(screenshot.width, screenshot.height);
            fullResponse[3] = JsonConvert.SerializeObject(newSize, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            logger.Trace("Start serialize screenshot");
            fullResponse[4] = JsonConvert.SerializeObject(screenshotCompressed, new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });
            logger.Trace("Finished Serialize Screenshot");

            screenshot.Apply(false, true);
            UnityEngine.Object.DestroyImmediate(screenshot);
            return JsonConvert.SerializeObject(fullResponse);
        }

        private string getPNGScreenshot()
        {
            var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
            var bytesPNG = UnityEngine.ImageConversion.EncodeToPNG(screenshot);
            var pngAsString = Convert.ToBase64String(bytesPNG);
            UnityEngine.Object.DestroyImmediate(screenshot);
            return pngAsString;
        }
    }
}
