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

using System;
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Logging;
using Newtonsoft.Json;
using UnityEngine;

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
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new UnityEngine.WaitForEndOfFrame();
            sendTexturedScreenshotResponse(size, quality);
        }

        protected System.Collections.IEnumerator SendPNGScreenshotCoroutine()
        {
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
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
                if (Application.isBatchMode)
                {
                    yield return null;
                }
                else
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

                    if (Application.isBatchMode)
                    {
                        yield return null;
                    }
                    else
                        yield return new UnityEngine.WaitForEndOfFrame();
                    sendTexturedScreenshotResponse(size, quality);

                    UnityEngine.Object.Destroy(panelHighlight);
                }
                else
                {
                    if (Application.isBatchMode)
                    {
                        yield return null;
                    }
                    else
                        yield return new UnityEngine.WaitForEndOfFrame();
                    sendTexturedScreenshotResponse(size, quality);
                }
            }
        }

        private void sendTexturedScreenshotResponse(UnityEngine.Vector2 size, int quality)
        {
            var result = ExecuteHandleErrors(() => getTexturedScreenshot(size, quality));

            string data = JsonConvert.SerializeObject(result, JsonSerializerSettings);

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
