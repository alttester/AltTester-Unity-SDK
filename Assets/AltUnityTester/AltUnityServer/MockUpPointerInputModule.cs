
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MockUpPointerInputModule : StandaloneInputModule
{
    public PointerEventData ExecuteTouchEvent(Touch touch, PointerEventData previousData = null)
    {
        if (EventSystem.current != null)
        {
            RaycastResult raycastResult;
            List<RaycastResult> raycastResults;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    PointerEventData pointerEventData =
                        new PointerEventData(EventSystem.current)
                        {
                            position = touch.position,
                            delta = touch.deltaPosition,
                            button = PointerEventData.InputButton.Left,
                            pointerId = touch.fingerId
                        };
                    raycastResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, raycastResults);
                    raycastResult = BaseInputModule.FindFirstRaycast(raycastResults);
                    pointerEventData.pointerCurrentRaycast = raycastResult;
                    pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;
                    
                    pointerEventData.pointerEnter = ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerCurrentRaycast.gameObject, pointerEventData,
                        ExecuteEvents.pointerEnterHandler);
                    pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerCurrentRaycast.gameObject,pointerEventData,
                        ExecuteEvents.pointerDownHandler);
                    ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerCurrentRaycast.gameObject, pointerEventData,
                        ExecuteEvents.initializePotentialDrag);
                    ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerCurrentRaycast.gameObject, pointerEventData,
                        ExecuteEvents.beginDragHandler);
                    pointerEventData.pointerDrag = ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerCurrentRaycast.gameObject, pointerEventData,
                        ExecuteEvents.dragHandler);
                    
                    if (pointerEventData.pointerPress == null)
                    {
                        pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerCurrentRaycast.gameObject, pointerEventData,
                            ExecuteEvents.pointerClickHandler);
                    }

                    return pointerEventData;
                case TouchPhase.Moved:
                    if (previousData != null)
                    {
                        raycastResults = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(previousData, raycastResults);
                        raycastResult = BaseInputModule.FindFirstRaycast(raycastResults);
                        previousData.pointerCurrentRaycast = raycastResult;
                        previousData.delta = touch.deltaPosition;
                        previousData.position = touch.position;
                        raycastResults = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(previousData, raycastResults);
                        raycastResult = BaseInputModule.FindFirstRaycast(raycastResults);
                        previousData.pointerCurrentRaycast = raycastResult;
                        if (previousData.pointerEnter != previousData.pointerCurrentRaycast.gameObject)
                        {
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerEnter, previousData,
                                ExecuteEvents.pointerExitHandler);
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                ExecuteEvents.pointerEnterHandler);
                            previousData.pointerEnter = previousData.pointerCurrentRaycast.gameObject;
                        }

                        if (previousData.delta != Vector2.zero)
                        {
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData, ExecuteEvents.dragHandler);
                        }

                        return previousData;
                    }

                    break;

                case TouchPhase.Ended:
                    if (previousData != null)
                    {
                        raycastResults = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(previousData, raycastResults);
                        raycastResult = BaseInputModule.FindFirstRaycast(raycastResults);
                        previousData.pointerCurrentRaycast = raycastResult;
                        ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                            ExecuteEvents.pointerUpHandler);
                        ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                            ExecuteEvents.pointerClickHandler);
                        ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                            ExecuteEvents.endDragHandler);
                        ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                            ExecuteEvents.dropHandler);
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
}

