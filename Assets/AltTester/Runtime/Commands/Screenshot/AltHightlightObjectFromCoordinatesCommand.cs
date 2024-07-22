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

using System.Collections.Generic;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltHighlightObjectFromCoordinatesCommand : AltBaseScreenshotCommand<AltHighlightObjectFromCoordinatesScreenshotParams, string>
    {
        private static List<GameObject> previousResults = null;
        private static Vector2 previousScreenCoordinates;


        public AltHighlightObjectFromCoordinatesCommand(ICommandHandler handler, AltHighlightObjectFromCoordinatesScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            var color = new UnityEngine.Color(CommandParams.color.r, CommandParams.color.g, CommandParams.color.b, CommandParams.color.a);

            GameObject selectedObject = getObjectAtCoordinates();

            if (selectedObject != null)
            {
                Handler.Send(ExecuteAndSerialize(() => AltRunner._altRunner.GameObjectToAltObject(selectedObject)));
                AltRunner._altRunner.StartCoroutine(SendScreenshotObjectHighlightedCoroutine(new UnityEngine.Vector2(CommandParams.size.x, CommandParams.size.y), CommandParams.quality, selectedObject, color, CommandParams.width));
            }
            else
            {
                Handler.Send(ExecuteAndSerialize(() => new AltObject("Null")));
                AltRunner._altRunner.StartCoroutine(SendTexturedScreenshotCoroutine(new UnityEngine.Vector2(CommandParams.size.x, CommandParams.size.y), CommandParams.quality));
            }
            return "Ok";
        }

        private GameObject getObjectAtCoordinates()//TODO refactor this to use FindObjectViaRaycast class
        {
            GameObject selectedObject = null;
            AltMockUpPointerInputModule mockUp = new AltMockUpPointerInputModule();
            var screenCoordinates = new Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y);
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
            if (selectedObject == null)
            {
                foreach (var camera in Camera.allCameras)
                {

                    Vector2 worldPoint = camera.ScreenToWorldPoint(screenCoordinates);
                    RaycastHit2D[] hits;
                    hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);
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
