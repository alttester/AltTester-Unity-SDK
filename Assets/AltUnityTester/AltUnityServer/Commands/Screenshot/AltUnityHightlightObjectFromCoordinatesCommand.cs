using System;
using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Boo.Lang;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHightlightObjectFromCoordinatesCommand : AltUnityCommand
    {
        UnityEngine.Vector2 screenCoordinates;
        string ColorAndWidth;
        UnityEngine.Vector2 size;
        int quality;
        AltClientSocketHandler handler;

        private static List<GameObject> previousResults = null;
        private static UnityEngine.Vector2 previousScreenCoordinates;


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

            GameObject selectedObject = null;

            UnityEngine.Color color = new UnityEngine.Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);

            var getScreenshotCommand = new AltUnityGetScreenshotCommand(handler, Parameters[0], Parameters[1], Parameters[4], Parameters[5]);
            AltUnityMockUpPointerInputModule mockUp = new AltUnityMockUpPointerInputModule();
            UnityEngine.EventSystems.PointerEventData pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointerEventData.position = screenCoordinates;
            System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> hitUI;
            List<GameObject> currentResults = new List<GameObject>();
            mockUp.GetAllRaycastResults(pointerEventData, out hitUI);
            for (int i = 0; i < hitUI.Count; i++)
            {
                currentResults.Add(hitUI[i].gameObject);
                if (previousResults == null || previousScreenCoordinates != screenCoordinates || previousResults.Count <= i || previousResults[i] != hitUI[i].gameObject)
                {
                    selectedObject = hitUI[i].gameObject;
                    break;
                }
            }


            if (selectedObject == null)
            {
                foreach (var camera in UnityEngine.Camera.allCameras)
                {

                    UnityEngine.Ray ray = camera.ScreenPointToRay(screenCoordinates);
                    UnityEngine.RaycastHit[] hits;
                    hits = UnityEngine.Physics.RaycastAll(ray);
                    if (hits.Length > 0)
                    {
                        currentResults.Add(hits[hits.Length - 1].transform.gameObject);
                        if (previousResults == null || previousScreenCoordinates != screenCoordinates || previousResults.Count < currentResults.Count || previousResults[currentResults.Count - 1] != currentResults[currentResults.Count - 1])
                        {
                            selectedObject = hits[hits.Length - 1].transform.gameObject;
                            break;
                        }

                    }
                }
            }

            previousScreenCoordinates = screenCoordinates;
            previousResults = currentResults;
            if (selectedObject == null && currentResults.Count != 0)
            {
                selectedObject = currentResults[0];
                previousResults.Clear();
                previousResults.Add(selectedObject);
            }

            if (selectedObject != null)
            {
                handler.SendResponse(this, Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(selectedObject)));
                AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(selectedObject, color, width, getScreenshotCommand));
                return "Ok";
            }



            handler.SendResponse(this, Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObject("Null")));
            getScreenshotCommand.Execute();
            return "Ok";
        }
    }
}
