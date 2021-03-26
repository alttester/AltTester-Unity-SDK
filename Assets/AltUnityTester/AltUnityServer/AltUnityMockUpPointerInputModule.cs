
using UnityEngine.EventSystems;

public class AltUnityMockUpPointerInputModule : StandaloneInputModule
{
    public UnityEngine.GameObject gameObjectHit;
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
                            eligibleForClick = true
                        };

                    gameObjectHit = GetGameObjectHit(touch);
                    GetFirstRaycastResult(pointerEventData, out raycastResult);
                    pointerEventData.pointerCurrentRaycast = raycastResult;
                    pointerEventData.pointerEnter = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                        ExecuteEvents.pointerEnterHandler);
                    pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                        ExecuteEvents.pointerDownHandler);

                    if (pointerEventData.pointerPress == null)
                    {
                        pointerEventData.pointerPress = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            ExecuteEvents.pointerClickHandler);
                    }

                    return pointerEventData;

                case UnityEngine.TouchPhase.Moved:
                    if (previousData != null)
                    {
                        if (previousData.pointerDrag == null)
                        {
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                ExecuteEvents.initializePotentialDrag);
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                ExecuteEvents.beginDragHandler);
                            previousData.pointerDrag = ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                ExecuteEvents.dragHandler);
                            previousData.dragging = true;
                        }
                        gameObjectHit = GetGameObjectHit(touch);

                        GetFirstRaycastResult(previousData, out raycastResult);
                        previousData.pointerCurrentRaycast = raycastResult;
                        previousData.delta = touch.deltaPosition;
                        previousData.position = touch.position;
                        GetFirstRaycastResult(previousData, out raycastResult);
                        previousData.pointerCurrentRaycast = raycastResult;

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
                        gameObjectHit = GetGameObjectHit(touch);
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

                        if (previousData.pointerDrag != null)
                        {
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                                ExecuteEvents.endDragHandler);
                            ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                ExecuteEvents.dropHandler);
                            previousData.dragging = false;
                        }

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

    private UnityEngine.GameObject GetGameObjectHit(UnityEngine.Touch touch)
    {
        UnityEngine.RaycastHit hit;

        foreach (var camera in UnityEngine.Camera.allCameras)
        {
            UnityEngine.Ray ray = camera.ScreenPointToRay(touch.position);
            if (UnityEngine.Physics.Raycast(ray, out hit))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }
}

