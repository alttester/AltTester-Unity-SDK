/*
    Copyright(C) 2026 Altom Consulting
    
*/

using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using static UnityEngine.EventSystems.ExecuteEvents;

namespace AltTester.AltTesterUnitySDK.InputModule
{
    public class AltMockUpPointerInputModule :
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
InputSystemUIInputModule
#else
StandaloneInputModule
#endif
    {
        public UnityEngine.GameObject GameObjectHit;

        private static bool ShouldHandleEventsManually
        {
#if ENABLE_INPUT_SYSTEM
            get => !(EventSystem.current?.currentInputModule is InputSystemUIInputModule);
#else
            get => true;
#endif
        }

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
                        if (ShouldHandleEventsManually && (raycastResult.gameObject ?? false))
                        {
                            pointerEventData.pointerEnter = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                                ExecuteEvents.pointerEnterHandler);
                            pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                                ExecuteEvents.pointerDownHandler);
                            pointerEventData.selectedObject = pointerEventData.pointerPress;
                            pointerEventData.pointerDrag = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                                ExecuteEvents.dragHandler);
                        }


                        var monoBehaviourTarget = FindObjectViaRayCast.FindMonoBehaviourObject(pointerEventData.position);
                        if (monoBehaviourTarget ?? false) monoBehaviourTarget.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
                        return pointerEventData;
                    case UnityEngine.TouchPhase.Moved:
                        if (previousData != null)
                        {
                            if (previousData.pointerDrag != null)
                            {
                                previousData.pointerPress = null;
                                previousData.eligibleForClick = false;
                            }
                            ExecuteDragPointerEvents(previousData);

                            GetFirstRaycastResult(previousData, out raycastResult);
                            previousData.pointerCurrentRaycast = raycastResult;
                            previousData.delta = touch.deltaPosition;
                            previousData.position = touch.position;


                            if (previousData.pointerEnter != previousData.pointerCurrentRaycast.gameObject)
                            {
                                if (ShouldHandleEventsManually)
                                {
                                    if (previousData.pointerEnter ?? false) ExecuteEvents.ExecuteHierarchy(previousData.pointerEnter, previousData,
                                            ExecuteEvents.pointerExitHandler);
                                    if (previousData.pointerCurrentRaycast.gameObject ?? false) ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                        ExecuteEvents.pointerEnterHandler);
                                }
                                previousData.pointerEnter = previousData.pointerCurrentRaycast.gameObject;
                            }

                            if (previousData.delta != UnityEngine.Vector2.zero)
                            {
                                if (ShouldHandleEventsManually)
                                {
                                    if (previousData.pointerDrag ?? false) ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                                        ExecuteEvents.dragHandler);
                                }
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
                            if (ShouldHandleEventsManually && (previousData.pointerPress ?? false))
                                ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                                    ExecuteEvents.pointerUpHandler);
                            var currentOverGo = previousData.pointerCurrentRaycast.gameObject;
                            var clickTarget = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
                            if (previousData.pointerPress == clickTarget && previousData.eligibleForClick)
                            {
                                if (ShouldHandleEventsManually && (previousData.pointerPress ?? false))
                                    ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                                          ExecuteEvents.pointerClickHandler);
                                previousData.eligibleForClick = false;
                            }

                            ExecuteEndDragPointerEvents(previousData);
                            if (ShouldHandleEventsManually && (previousData.pointerCurrentRaycast.gameObject ?? false))
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
#if ALTTESTER && ENABLE_LEGACY_INPUT_MANAGER
            if (AltInput.MonoBehaviourTargetMouseDown != null)
            {
                AltInput.MonoBehaviourTargetMouseDown.SendMessage("OnMouseDrag", UnityEngine.SendMessageOptions.DontRequireReceiver);
            }
#endif
            if (previousData.pointerDrag == null)
            {
                previousData.dragging = true;
                if (ShouldHandleEventsManually && (previousData.pointerCurrentRaycast.gameObject ?? false))
                {
                    previousData.pointerDrag = ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                            ExecuteEvents.beginDragHandler);
                    if (previousData.pointerDrag != null)
                        ExecuteEvents.Execute(previousData.pointerDrag, previousData, ExecuteEvents.dragHandler);
                    else
                        previousData.pointerDrag = ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                            ExecuteEvents.dragHandler);
                }
            }
            else
            {
                if (ShouldHandleEventsManually)
                {
                    if (!previousData.dragging)
                    {
                        if (previousData.pointerDrag ?? false) ExecuteEvents.Execute(previousData.pointerDrag, previousData,
                            ExecuteEvents.beginDragHandler);
                        previousData.dragging = true;
                    }
                    if (previousData.pointerDrag ?? false) ExecuteEvents.Execute(previousData.pointerDrag, previousData, ExecuteEvents.dragHandler);
                }
            }
        }

        public void ExecuteEndDragPointerEvents(PointerEventData previousData)
        {
#if ALTTESTER && ENABLE_LEGACY_INPUT_MANAGER

            if (AltInput.MonoBehaviourTargetMouseDown != null)
            {
                AltInput.MonoBehaviourTargetMouseDown.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
                AltInput.MonoBehaviourTargetMouseDown.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
                AltInput.MonoBehaviourTargetMouseDown = null;
            }
#endif
            if (previousData.pointerDrag != null)
            {
                if (ShouldHandleEventsManually)
                {
                    if (previousData.pointerDrag ?? false) ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                            ExecuteEvents.endDragHandler);
                    if (previousData.pointerCurrentRaycast.gameObject ?? false) ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                        ExecuteEvents.dropHandler);
                }
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
        public static void GetAllRaycastResults(PointerEventData pointerEventData, out System.Collections.Generic.List<RaycastResult> raycastResults)
        {
            raycastResults = new System.Collections.Generic.List<RaycastResult>();
            if (EventSystem.current != null)
            {
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            }
        }

        private UnityEngine.GameObject getGameObjectHit(UnityEngine.Touch touch)
        {

            foreach (var camera in UnityEngine.Camera.allCameras.OrderByDescending(c => c.depth))
            {
                UnityEngine.Ray ray = camera.ScreenPointToRay(touch.position);
                UnityEngine.RaycastHit hit;
                if (UnityEngine.Physics.Raycast(ray, out hit))
                {
                    return hit.transform.gameObject;
                }
                UnityEngine.RaycastHit2D hit2d = UnityEngine.Physics2D.GetRayIntersection(camera.ScreenPointToRay(touch.position));
                if (hit2d.collider != null)
                {
                    return hit2d.transform.gameObject;
                }
            }
            return null;
        }
    }
}
