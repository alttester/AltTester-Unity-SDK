using System;
using System.Collections.Generic;
using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHightlightObjectFromCoordinatesCommand : AltUnityBaseScreenshotCommand
    {
        readonly AltClientSocketHandler handler;
        private Vector2 screenCoordinates;
        readonly string colorAndWidth;
        private readonly Vector2 size;
        private readonly int quality;
        private static List<GameObject> previousResults = null;
        private static Vector2 previousScreenCoordinates;


        public AltUnityHightlightObjectFromCoordinatesCommand(AltClientSocketHandler handler, params string[] parameters) : base(handler, parameters, 6)
        {
            this.handler = handler;
            this.screenCoordinates = JsonConvert.DeserializeObject<Vector2>(parameters[2]);
            colorAndWidth = parameters[3];
            this.size = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[4]);
            this.quality = JsonConvert.DeserializeObject<int>(parameters[5]);
        }

        public override string Execute()
        {
            var pieces = colorAndWidth.Split(new[] { "!-!" }, StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]);


            Color color = new Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);

            GameObject selectedObject = getObjectAtCoordinates();

            if (selectedObject != null)
            {
                handler.SendResponse(MessageId, CommandName, JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(selectedObject)), string.Empty);
                AltUnityRunner._altUnityRunner.StartCoroutine(SendScreenshotObjectHighlightedCoroutine(size, quality, selectedObject, color, width));
            }
            else
            {
                handler.SendResponse(MessageId, CommandName, JsonConvert.SerializeObject(new AltUnityObject("Null")), string.Empty);
                AltUnityRunner._altUnityRunner.StartCoroutine(SendTexturedScreenshotCoroutine(size, quality));
            }
            return "Ok";
        }

        private GameObject getObjectAtCoordinates()
        {
            GameObject selectedObject = null;
            AltUnityMockUpPointerInputModule mockUp = new AltUnityMockUpPointerInputModule();
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
            {
                position = screenCoordinates
            };
            List<GameObject> currentResults = new List<GameObject>();
            List<UnityEngine.EventSystems.RaycastResult> hitUI;
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

            return selectedObject;
        }
    }
}
