
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using static UnityEngine.EventSystems.ExecuteEvents;

namespace AltTester
{
    public class AltMockUpPointerInputModule : StandaloneInputModule
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
#if ENABLE_INPUT_SYSTEM
                        if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                        {
#endif
                        pointerEventData.pointerEnter = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            ExecuteEvents.pointerEnterHandler);
                        pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            ExecuteEvents.pointerDownHandler);
                        pointerEventData.selectedObject = pointerEventData.pointerPress;
                        pointerEventData.pointerDrag = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            ExecuteEvents.dragHandler);
#if ENABLE_INPUT_SYSTEM
                        }
#endif


                        var monoBehaviourTarget = FindObjectViaRayCast.FindMonoBehaviourObject(pointerEventData.position);
                        if (monoBehaviourTarget != null) monoBehaviourTarget.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
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
                            GameObjectHit = getGameObjectHit(touch);

                            GetFirstRaycastResult(previousData, out raycastResult);
                            previousData.pointerCurrentRaycast = raycastResult;
                            previousData.delta = touch.deltaPosition;
                            previousData.position = touch.position;


                            if (previousData.pointerEnter != previousData.pointerCurrentRaycast.gameObject)
                            {
#if ENABLE_INPUT_SYSTEM
             if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                                {
#endif

                                ExecuteEvents.ExecuteHierarchy(previousData.pointerEnter, previousData,
                                        ExecuteEvents.pointerExitHandler);
                                ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                    ExecuteEvents.pointerEnterHandler);
#if ENABLE_INPUT_SYSTEM
                                }
#endif
                                previousData.pointerEnter = previousData.pointerCurrentRaycast.gameObject;
                            }

                            if (previousData.delta != UnityEngine.Vector2.zero)
                            {
#if ENABLE_INPUT_SYSTEM
                                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
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
#if ENABLE_INPUT_SYSTEM
                            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                                ExecuteEvents.pointerUpHandler);
                            var currentOverGo = previousData.pointerCurrentRaycast.gameObject;
                            var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                            ;
                            if (previousData.pointerPress == pointerUpHandler && previousData.eligibleForClick)
                            {
#if ENABLE_INPUT_SYSTEM
                                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
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
#if ENABLE_INPUT_SYSTEM
                            if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
#endif
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
                previousData.dragging = true;
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                previousData.pointerDrag = ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                        ExecuteEvents.beginDragHandler);
                if (previousData.pointerDrag != null)
                {
                    ExecuteEvents.Execute(previousData.pointerDrag, previousData,
                        ExecuteEvents.dragHandler);
                }
                else

                    previousData.pointerDrag = ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                        ExecuteEvents.dragHandler);
#if ENABLE_INPUT_SYSTEM
                }
#endif
            }
            else
            {
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif

                if (!previousData.dragging)
                {
                    ExecuteEvents.Execute(previousData.pointerDrag, previousData,
                        ExecuteEvents.beginDragHandler);
                    previousData.dragging = true;
                }
                ExecuteEvents.Execute(previousData.pointerDrag, previousData, ExecuteEvents.dragHandler);
#if ENABLE_INPUT_SYSTEM
                }
#endif
            }
        }

        public void ExecuteEndDragPointerEvents(PointerEventData previousData)
        {
            if (previousData.pointerDrag != null)
            {
#if ENABLE_INPUT_SYSTEM
                if (EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                {
#endif
                ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                        ExecuteEvents.endDragHandler);
                ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                    ExecuteEvents.dropHandler);
#if ENABLE_INPUT_SYSTEM
                }
#endif
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
    }
}