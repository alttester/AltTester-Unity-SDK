
using System.Linq;
using UnityEngine.EventSystems;

namespace Altom.AltUnityTester
{
    public class AltUnityMockUpPointerInputModule : StandaloneInputModule
    {
        public UnityEngine.GameObject GameObjectHit;
        public PointerEventData ExecuteTouchEvent(UnityEngine.Touch touch, PointerEventData previousData = null)
        {
            if (EventSystem.current != null)
            {
                RaycastResult raycastResult;
                switch (touch.phase)
                {
                    case UnityEngine.TouchPhase.Began:
                        var pointerEventData =
                            new PointerEventData(EventSystem.current)
                            {
                                position = touch.position,
                                delta = touch.deltaPosition,
                                button = PointerEventData.InputButton.Left,
                                pointerId = touch.fingerId,
                                eligibleForClick = true,
                                pressPosition = touch.position
                            };

                        GameObjectHit = getGameObjectHit(touch);
                        GetFirstRaycastResult(pointerEventData, out raycastResult);
                        pointerEventData.pointerCurrentRaycast = raycastResult;
                        pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;
                        pointerEventData.pointerEnter = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            ExecuteEvents.pointerEnterHandler);
                        var monoBehaviourTarget = FindMonoBehaviourObject(pointerEventData.position);
                        pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            ExecuteEvents.pointerDownHandler);
                        pointerEventData.pointerDrag = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            ExecuteEvents.initializePotentialDrag);
                        if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
                        return pointerEventData;
                    case UnityEngine.TouchPhase.Moved:
                        if (previousData != null)
                        {
                            ExecuteDragPointerEvents(previousData);
                            GameObjectHit = getGameObjectHit(touch);

                            GetFirstRaycastResult(previousData, out raycastResult);
                            previousData.pointerCurrentRaycast = raycastResult;
                            previousData.delta = touch.deltaPosition;
                            previousData.position = touch.position;


                            if (previousData.pointerEnter != previousData.pointerCurrentRaycast.gameObject)
                            {
                                ExecuteEvents.ExecuteHierarchy(previousData.pointerEnter, previousData,
                                    ExecuteEvents.pointerExitHandler);
                                ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                    ExecuteEvents.pointerEnterHandler);
                                previousData.pointerEnter = previousData.pointerCurrentRaycast.gameObject;
                            }

                            if (previousData.delta != UnityEngine.Vector2.zero)
                            {
                                ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                                    ExecuteEvents.dragHandler);
                            }

                            return previousData;
                        }
                        break;

                    case UnityEngine.TouchPhase.Ended:
                        if (previousData != null)
                        {
                            GameObjectHit = getGameObjectHit(touch);
                            GetFirstRaycastResult(previousData, out raycastResult);
                            previousData.pointerCurrentRaycast = raycastResult;
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                                ExecuteEvents.pointerUpHandler);
                            var currentOverGo = previousData.pointerCurrentRaycast.gameObject;
                            var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                            if (previousData.pointerPress == pointerUpHandler && previousData.eligibleForClick)
                            {
                                ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                                      ExecuteEvents.pointerClickHandler);
                                previousData.eligibleForClick = false;
                            }
                            if (previousData.pointerPress != null)
                            {
                                previousData.pointerPress.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
                                previousData.pointerPress.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
                            }

                            ExecuteEndDragPointerEvents(previousData);

                            ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                ExecuteEvents.pointerExitHandler);
                            return previousData;
                        }
                        break;
                }

                return previousData;
            }

            return null;
        }

        public void ExecuteDragPointerEvents(PointerEventData previousData)
        {
            if (previousData.pointerDrag == null)
            {
                previousData.pointerDrag = ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                    ExecuteEvents.beginDragHandler);
                previousData.dragging = true;
                if (previousData.pointerDrag != null)
                {
                    ExecuteEvents.Execute(previousData.pointerDrag, previousData,
                        ExecuteEvents.dragHandler);
                }
                else
                    previousData.pointerDrag = ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                        ExecuteEvents.dragHandler);
            }
            else
            {
                if (!previousData.dragging)
                {
                    ExecuteEvents.Execute(previousData.pointerDrag, previousData,
                        ExecuteEvents.beginDragHandler);
                    previousData.dragging = true;
                }
                ExecuteEvents.Execute(previousData.pointerDrag, previousData, ExecuteEvents.dragHandler);
            }
        }

        public void ExecuteEndDragPointerEvents(PointerEventData previousData)
        {
            if (previousData.pointerDrag != null)
            {
                ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                    ExecuteEvents.endDragHandler);
                ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                    ExecuteEvents.dropHandler);
                previousData.dragging = false;
            }
        }

        public void GetFirstRaycastResult(PointerEventData pointerEventData, out RaycastResult raycastResult)
        {
            System.Collections.Generic.List<RaycastResult> raycastResults;
            raycastResults = new System.Collections.Generic.List<RaycastResult>();
            if (EventSystem.current != null)
            {
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            }
            raycastResult = FindFirstRaycast(raycastResults);
        }
        public void GetAllRaycastResults(PointerEventData pointerEventData, out System.Collections.Generic.List<RaycastResult> raycastResults)
        {
            raycastResults = new System.Collections.Generic.List<RaycastResult>();
            if (EventSystem.current != null)
            {
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            }
        }

        private UnityEngine.GameObject getGameObjectHit(UnityEngine.Touch touch)
        {

            foreach (var camera in UnityEngine.Camera.allCameras)
            {
                UnityEngine.Ray ray = camera.ScreenPointToRay(touch.position);
                UnityEngine.RaycastHit hit;
                if (UnityEngine.Physics.Raycast(ray, out hit))
                {
                    return hit.transform.gameObject;
                }
            }
            return null;
        }

        public UnityEngine.GameObject GetGameObjectHitMonoBehaviour(UnityEngine.Vector2 coordinates)
        {
            foreach (var camera in UnityEngine.Camera.allCameras.OrderByDescending(c => c.depth))
            {
                UnityEngine.RaycastHit hit;
                UnityEngine.Ray ray = camera.ScreenPointToRay(coordinates);
                UnityEngine.GameObject gameObject3d = null;
                UnityEngine.GameObject gameObject2d = null;
                UnityEngine.Vector3 hitPosition3d = UnityEngine.Vector3.zero;
                UnityEngine.Vector3 hitPosition2d = UnityEngine.Vector3.zero;
                if (UnityEngine.Physics.Raycast(ray, out hit))
                {
                    hitPosition3d = hit.point;
                    gameObject3d = hit.transform.gameObject;
                }
                UnityEngine.RaycastHit2D hit2d;
                if (hit2d = UnityEngine.Physics2D.Raycast(coordinates, UnityEngine.Vector2.zero))
                {
                    hitPosition2d = hit2d.point;
                    gameObject2d = hit2d.transform.gameObject;
                }


                if (gameObject2d != null && gameObject3d != null)
                {
                    if (UnityEngine.Vector3.Distance(camera.transform.position, hitPosition2d) < UnityEngine.Vector3.Distance(camera.transform.position, hitPosition3d))
                        return gameObject2d;
                    else
                        return gameObject3d;
                }
                if (gameObject2d != null) return gameObject2d;
                if (gameObject3d != null) return gameObject3d;
            }
            return null;
        }

        /// <summary>
        /// Finds element(s) at given coordinates for which we raise MonoBehaviour input events
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns>the found gameObject</returns>
        public UnityEngine.GameObject FindMonoBehaviourObject(UnityEngine.Vector2 coordinates)
        {
            var target = GetGameObjectHitMonoBehaviour(coordinates);
            if (target == null)
                return null;

            var rigidBody = target.GetComponentInParent<UnityEngine.Rigidbody>();
            if (rigidBody != null)
                return rigidBody.gameObject;
            var rigidBody2D = target.GetComponentInParent<UnityEngine.Rigidbody2D>();
            if (rigidBody2D != null)
                return rigidBody2D.gameObject;
            return target;
        }
    }
}