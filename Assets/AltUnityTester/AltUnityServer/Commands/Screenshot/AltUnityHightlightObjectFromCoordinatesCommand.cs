using System;
using Newtonsoft.Json;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHightlightObjectFromCoordinatesCommand : AltUnityCommand
    {
        UnityEngine.Vector2 screenCoordinates;
        string ColorAndWidth;
        UnityEngine.Vector2 size;
        int quality;
        AltClientSocketHandler handler;


        public AltUnityHightlightObjectFromCoordinatesCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 6)
        {
            this.handler = handler;
            this.screenCoordinates = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            ColorAndWidth = parameters[3];
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[4]);
            this.quality = Int32.Parse(parameters[5]);
        }

        public override string Execute()
        {
            LogMessage("HightlightObject with coordinates: " + screenCoordinates);
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, System.StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, System.StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]);

            UnityEngine.Color color = new UnityEngine.Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);

            var getScreenshotCommand = new AltUnityGetScreenshotCommand(handler, Parameters[0], Parameters[1], Parameters[4], Parameters[5]);
            AltUnityMockUpPointerInputModule mockUp = new AltUnityMockUpPointerInputModule();
            UnityEngine.EventSystems.PointerEventData pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointerEventData.position = screenCoordinates;
            UnityEngine.EventSystems.RaycastResult raycastResult;
            System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> hitUI;
            mockUp.GetFirstRaycastResult(pointerEventData, out raycastResult, out hitUI);
            if (raycastResult.isValid)
            {
                handler.SendResponse(this, Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(raycastResult.gameObject)));
                AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(raycastResult.gameObject, color, width, getScreenshotCommand));
                return "Ok";
            }
            foreach (var camera in UnityEngine.Camera.allCameras)
            {

                UnityEngine.Ray ray = camera.ScreenPointToRay(screenCoordinates);
                UnityEngine.RaycastHit[] hits;
                hits = UnityEngine.Physics.RaycastAll(ray);
                if (hits.Length > 0)
                {
                    handler.SendResponse(this, Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(hits[hits.Length - 1].transform.gameObject)));
                    AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(hits[hits.Length - 1].transform.gameObject, color, width, getScreenshotCommand));
                    return "Ok";
                }
            }

            handler.SendResponse(this, Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObject("Null")));
            getScreenshotCommand.Execute();
            return "Ok";
        }
    }
}
