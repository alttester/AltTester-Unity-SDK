using System;
using System.Collections.Generic;
using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHightlightObjectFromCoordinatesCommand : AltUnityCommand
    {
        readonly AltClientSocketHandler handler;
        private Vector2 screenCoordinates;
        readonly string colorAndWidth;
        private readonly AltUnityGetScreenshotCommand getScreenshotCommand;
        private static List<GameObject> previousResults = null;
        private static Vector2 previousScreenCoordinates;


        public AltUnityHightlightObjectFromCoordinatesCommand(AltClientSocketHandler handler, params string[] parameters) : base(parameters, 6)
        {
            this.handler = handler;
            this.screenCoordinates = JsonConvert.DeserializeObject<Vector2>(parameters[2]);
            colorAndWidth = parameters[3];

            getScreenshotCommand = new AltUnityGetScreenshotCommand(handler, Parameters[0], Parameters[1], Parameters[4], Parameters[5]);
        }

        public override string Execute()
        {
            var pieces = colorAndWidth.Split(new[] { "!-!" }, StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]);

            GameObject selectedObject = null;

            Color color = new Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);

            AltUnityMockUpPointerInputModule mockUp = new AltUnityMockUpPointerInputModule();
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = screenCoordinates
            };
            List<GameObject> currentResults = new List<GameObject>();
            mockUp.GetAllRaycastResults(pointerEventData, out List<UnityEngine.EventSystems.RaycastResult> hitUI);
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
                foreach (var camera in Camera.allCameras)
                {

                    Ray ray = camera.ScreenPointToRay(screenCoordinates);
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(ray);
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
                handler.SendResponse(this, JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(selectedObject)));
                AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.HighLightSelectedObjectCorutine(selectedObject, color, width, getScreenshotCommand));
                return "Ok";
            }

            handler.SendResponse(this, JsonConvert.SerializeObject(new AltUnityObject("Null")));
            getScreenshotCommand.Execute();
            return "Ok";
        }
    }
}
