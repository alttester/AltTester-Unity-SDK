using System;
using System.Globalization;
using System.Linq;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Logging;
using Altom.AltUnityTester.Communication;
using Newtonsoft.Json;
using NLog;

namespace Altom.AltUnityTester.Commands
{
    public abstract class AltUnityBaseScreenshotCommand<TParams, TResult> : AltUnityCommand<TParams, TResult> where TParams : CommandParams
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        protected readonly ICommandHandler Handler;

        protected AltUnityBaseScreenshotCommand(ICommandHandler handler, TParams cmdParams) : base(cmdParams)
        {
            this.Handler = handler;
        }

        public abstract override TResult Execute();


        protected System.Collections.IEnumerator SendTexturedScreenshotCoroutine(UnityEngine.Vector2 size, int quality)
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            sendTexturedScreenshotResponse(size, quality);
        }

        protected System.Collections.IEnumerator SendPNGScreenshotCoroutine()
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            var response = ExecuteAndSerialize(getPNGScreenshot);
            Handler.Send(response);
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
            var result = ExecuteHandleErrors(() => getTexturedScreenshot(size, quality));

            string data = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = CultureInfo.InvariantCulture,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });

            Handler.Send(data);

        }

        private AltUnityGetScreenshotResponse getTexturedScreenshot(UnityEngine.Vector2 size, int quality)
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

            AltUnityGetScreenshotResponse response = new AltUnityGetScreenshotResponse();

            response.scaleDifference = new AltUnityVector2(screenshot.width, screenshot.height);

            width = width * quality / 100;
            height = height * quality / 100;
            AltUnityTextureScale.Bilinear(screenshot, width, height);
            screenshot.Compress(false);
            screenshot.Apply(false);

            var screenshotSerialized = screenshot.GetRawTextureData();

            logger.Trace("Start Compression");
            var screenshotCompressed = AltUnityRunner.CompressScreenshot(screenshotSerialized);
            logger.Trace("Finished Compression");

            response.textureFormat = (AltUnityTextureFormat)screenshot.format;
            response.textureSize = new AltUnityVector3(screenshot.width, screenshot.height);
            response.compressedImage = screenshotCompressed; // todo StringEscapeHandling = StringEscapeHandling.EscapeNonAscii

            screenshot.Apply(false, true);
            UnityEngine.Object.DestroyImmediate(screenshot);
            return response;
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
