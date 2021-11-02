using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityHightlightObjectFromCoordinatesCommand : AltUnityBaseScreenshotCommand<AltUnityHightlightObjectFromCoordinatesScreenshotParams, string>
    {
        private static List<GameObject> previousResults = null;
        private static Vector2 previousScreenCoordinates;


        public AltUnityHightlightObjectFromCoordinatesCommand(ICommandHandler handler, AltUnityHightlightObjectFromCoordinatesScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            var color = new UnityEngine.Color(CommandParams.color.r, CommandParams.color.g, CommandParams.color.b, CommandParams.color.a);

            GameObject selectedObject = getObjectAtCoordinates();

            if (selectedObject != null)
            {
                Handler.Send(ExecuteAndSerialize(() => AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(selectedObject)));
                AltUnityRunner._altUnityRunner.StartCoroutine(SendScreenshotObjectHighlightedCoroutine(CommandParams.size.ToUnity(), CommandParams.quality, selectedObject, color, CommandParams.width));
            }
            else
            {
                Handler.Send(ExecuteAndSerialize(() => new AltUnityObject("Null")));
                AltUnityRunner._altUnityRunner.StartCoroutine(SendTexturedScreenshotCoroutine(CommandParams.size.ToUnity(), CommandParams.quality));
            }
            return "Ok";
        }

        private GameObject getObjectAtCoordinates()
        {
            GameObject selectedObject = null;
            AltUnityMockUpPointerInputModule mockUp = new AltUnityMockUpPointerInputModule();
            var screenCoordinates = CommandParams.coordinates.ToUnity();
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
