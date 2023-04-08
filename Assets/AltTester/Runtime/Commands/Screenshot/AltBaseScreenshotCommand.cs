using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;
using AltTester.AltTesterUnitySDK.Logging;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public abstract class AltBaseScreenshotCommand<TParams, TResult> : AltCommand<TParams, TResult> where TParams : CommandParams
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        protected readonly ICommandHandler Handler;

        protected AltBaseScreenshotCommand(ICommandHandler handler, TParams cmdParams) : base(cmdParams)
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
                    renderer.materials[i].shader = AltRunner._altRunner.outlineShader;
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
                    var panelHighlight = UnityEngine.Object.Instantiate(AltRunner._altRunner.panelHightlightPrefab, rectTransform);
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

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UnityEngine.Debug.LogWarning(" Length before serialization:  " + result.data.Length);

            string data = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = CultureInfo.InvariantCulture,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });
            stopWatch.Stop();
            var ts2 = stopWatch.Elapsed;
            string elapsedTime = ts2.Milliseconds.ToString();
            UnityEngine.Debug.LogWarning("Json1: " + elapsedTime + " Length:  " + data.Length);
            Handler.Send(data);

        }

        private AltGetScreenshotResponse getTexturedScreenshot(UnityEngine.Vector2 size, int quality)
        {
            var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
            AltGetScreenshotResponse response = new AltGetScreenshotResponse();
            response.scaleDifference = new AltVector2(screenshot.width, screenshot.height);
            var screenshotSerialized = UnityEngine.ImageConversion.EncodeToJPG(screenshot, quality);
            response.textureSize = new AltVector3(screenshot.width, screenshot.height);
            response.compressedImage = screenshotSerialized;

            UnityEngine.Object.DestroyImmediate(screenshot);
            return response;
        }

        private string getPNGScreenshot()
        {
            var screenShot = new UnityEngine.Texture2D(UnityEngine.Screen.width, UnityEngine.Screen.height, UnityEngine.TextureFormat.RGB24, false);
            screenShot.ReadPixels(new UnityEngine.Rect(0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height), 0, 0);
            screenShot.Apply();
            var bytesPNG = UnityEngine.ImageConversion.EncodeToPNG(screenShot);
            var pngAsString = Convert.ToBase64String(bytesPNG);
            UnityEngine.Object.DestroyImmediate(screenShot);
            return pngAsString;
        }
    }
}
