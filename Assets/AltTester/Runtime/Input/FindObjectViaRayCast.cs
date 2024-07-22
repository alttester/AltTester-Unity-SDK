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
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AltTester.AltTesterUnitySDK.InputModule
{
    public class FindObjectViaRayCast
    {
        /// <summary>
        /// Finds element at given pointerEventData for which we raise EventSystem input events
        /// </summary>
        /// <param name="pointerEventData"></param>
        /// <returns>the found gameObject</returns>
        private static GameObject findEventSystemObject(PointerEventData pointerEventData)
        {
            List<RaycastResult> firstRaycastResult;
            GetAllRaycastResults(pointerEventData, out firstRaycastResult);
            foreach (var result in firstRaycastResult)
            {
                if (ExecuteEvents.CanHandleEvent<IPointerClickHandler>(result.gameObject))
                {
                    return result.gameObject;
                }
            }
            foreach (var result in firstRaycastResult)//if nothing has click handler then return the first UI element encountered
            {
                return result.gameObject;
            }
            return null;
        }
        public static GameObject FindObjectAtCoordinates(Vector2 screenPosition)
        {
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition,
                button = PointerEventData.InputButton.Left,
                eligibleForClick = true,
                pressPosition = screenPosition
            };
            var eventSystemTarget = findEventSystemObject(pointerEventData);
            if (eventSystemTarget != null) return eventSystemTarget;
            var monoBehaviourTarget = FindMonoBehaviourObject(screenPosition);
            return monoBehaviourTarget;
        }
        /// <summary>
        /// Finds element(s) at given coordinates for which we raise MonoBehaviour input events
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns>the found gameObject</returns>
        public static GameObject FindMonoBehaviourObject(Vector2 coordinates)
        {
            var target = GetGameObjectHitMonoBehaviour(coordinates);
            if (target == null)
                return null;

            var rigidBody = target.GetComponentInParent<Rigidbody>();
            if (rigidBody != null)
                return rigidBody.gameObject;
            var rigidBody2D = target.GetComponentInParent<Rigidbody2D>();
            if (rigidBody2D != null)
                return rigidBody2D.gameObject;
            return target;
        }
        public static GameObject GetGameObjectHitMonoBehaviour(Vector2 coordinates)
        {
            foreach (var camera in Camera.allCameras.OrderByDescending(c => c.depth))
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(coordinates);
                GameObject gameObject3d = null;
                GameObject gameObject2d = null;
                Vector3 hitPosition3d = Vector3.zero;
                Vector3 hitPosition2d = Vector3.zero;
                if (Physics.Raycast(ray, out hit))
                {
                    hitPosition3d = hit.point;
                    gameObject3d = hit.transform.gameObject;
                }
                RaycastHit2D hit2d = Physics2D.Raycast(coordinates, Vector2.zero);//If UI has colliders2D
                if (hit2d.collider != null)
                {
                    hitPosition2d = hit2d.point;
                    gameObject2d = hit2d.transform.gameObject;
                }
                else
                {
                    hit2d = Physics2D.GetRayIntersection(camera.ScreenPointToRay(coordinates));//For 2D Objects in scenes
                    if (hit2d.collider != null)
                    {
                        hitPosition2d = hit2d.point;
                        gameObject2d = hit2d.transform.gameObject;
                    }
                }




                if (gameObject2d != null && gameObject3d != null)
                {
                    return Vector3.Distance(camera.transform.position, hitPosition2d) < Vector3.Distance(camera.transform.position, hitPosition3d)
                        ? gameObject2d
                        : gameObject3d;
                }
                if (gameObject2d != null)
                {
                    return gameObject2d;
                }

                if (gameObject3d != null)
                {
                    return gameObject3d;
                }
            }
            return null;
        }
        ///<summary>
        /// Iterate through all cameras until finds one that sees the object.
        /// If no camera sees the object return the position from the last camera
        ///</summary>
        public static int FindCameraThatSeesObject(GameObject gameObject, out Vector3 position)
        {
            position = Vector3.one * -1;
            int cameraId = -1;
            if (Camera.allCamerasCount == 0)
            {
                var rectTransform = gameObject.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    var canvas = rectTransform.GetComponentInParent<Canvas>();
                    if (canvas != null)
                        position = RectTransformUtility.PixelAdjustPoint(rectTransform.position, rectTransform, canvas.rootCanvas);
                }
                return cameraId;
            }
            foreach (var camera1 in Camera.allCameras)
            {
                position = GetObjectScreenPosition(gameObject, camera1);
                cameraId = camera1.GetInstanceID();
                if (position.x > 0 &&
                    position.y > 0 &&
                    position.x < Screen.width &&
                    position.y < Screen.height &&
                    position.z >= 0)//Check if camera sees the object
                {
                    break;
                }
            }
            return cameraId;
        }
        public static Vector3 GetObjectScreenPosition(GameObject gameObject, Camera camera)
        {
            var selectedCamera = camera;
            var position = gameObject.transform.position;
            Canvas canvas = gameObject.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                if (gameObject.GetComponent<RectTransform>() == null)
                    return camera.WorldToScreenPoint(gameObject.transform.position);

                Vector3[] vector3S = new Vector3[4];
                gameObject.GetComponent<RectTransform>().GetWorldCorners(vector3S);
                position = new Vector3((vector3S[0].x + vector3S[2].x) / 2, (vector3S[0].y + vector3S[2].y) / 2, (vector3S[0].z + vector3S[2].z) / 2);

                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    return position;
                }
                if (canvas.worldCamera != null)
                {
                    selectedCamera = canvas.worldCamera;
                }
                return selectedCamera.WorldToScreenPoint(position);

            }

            var collider = gameObject.GetComponent<Collider>();
            if (collider != null)
            {
                position = collider.bounds.center;
            }

            return camera.WorldToScreenPoint(position);
        }
        public static void GetAllRaycastResults(PointerEventData pointerEventData, out List<RaycastResult> raycastResults)
        {
            raycastResults = new List<RaycastResult>();
            if (EventSystem.current != null)
            {
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            }
        }

    }


}
