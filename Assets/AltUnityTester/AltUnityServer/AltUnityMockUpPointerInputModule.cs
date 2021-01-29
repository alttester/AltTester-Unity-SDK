
using UnityEngine.EventSystems;

public class AltUnityMockUpPointerInputModule : UnityEngine.EventSystems.StandaloneInputModule
{
    public UnityEngine.GameObject gameObjectHit;
    public UnityEngine.EventSystems.PointerEventData ExecuteTouchEvent(UnityEngine.Touch touch, UnityEngine.EventSystems.PointerEventData previousData = null)
    {
        UnityEngine.RaycastHit hit;
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            UnityEngine.EventSystems.RaycastResult raycastResult;
            System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> raycastResults;
            switch (touch.phase)
            {
                case UnityEngine.TouchPhase.Began:
                    UnityEngine.EventSystems.PointerEventData pointerEventData =
                        new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
                        {
                            position = touch.position,
                            delta = touch.deltaPosition,
                            button = UnityEngine.EventSystems.PointerEventData.InputButton.Left,
                            pointerId = touch.fingerId,
                            eligibleForClick = true
                        };

                    gameObjectHit = GetGameObjectHit(touch);
                    GetFirstRaycastResult(pointerEventData, out raycastResult, out raycastResults);
                    pointerEventData.pointerCurrentRaycast = raycastResult;
                    pointerEventData.pointerEnter = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                        UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
                    pointerEventData.pointerPress = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                        UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);

                    if (pointerEventData.pointerPress == null)
                    {
                        pointerEventData.pointerPress = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData,
                            UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
                    }

                    return pointerEventData;

                case UnityEngine.TouchPhase.Moved:
                    if (previousData != null)
                    {
                        if (previousData.pointerDrag == null)
                        {
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.beginDragHandler);
                            previousData.pointerDrag = UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.dragHandler);
                            previousData.dragging = true;
                        }
                        gameObjectHit = GetGameObjectHit(touch);

                        GetFirstRaycastResult(previousData, out raycastResult, out raycastResults);
                        previousData.pointerCurrentRaycast = raycastResult;
                        previousData.delta = touch.deltaPosition;
                        previousData.position = touch.position;
                        raycastResults = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
                        UnityEngine.EventSystems.EventSystem.current.RaycastAll(previousData, raycastResults);
                        raycastResult = UnityEngine.EventSystems.BaseInputModule.FindFirstRaycast(raycastResults);
                        previousData.pointerCurrentRaycast = raycastResult;

                        if (previousData.pointerEnter != previousData.pointerCurrentRaycast.gameObject)
                        {
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerEnter, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
                            previousData.pointerEnter = previousData.pointerCurrentRaycast.gameObject;
                        }

                        if (previousData.delta != UnityEngine.Vector2.zero)
                        {
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.dragHandler);
                        }

                        return previousData;
                    }
                    break;

                case UnityEngine.TouchPhase.Ended:
                    if (previousData != null)
                    {
                        gameObjectHit = GetGameObjectHit(touch);
                        GetFirstRaycastResult(previousData, out raycastResult, out raycastResults);
                        previousData.pointerCurrentRaycast = raycastResult;
                        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                            UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
                        var currentOverGo = previousData.pointerCurrentRaycast.gameObject;
                        var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                        if (previousData.pointerPress == pointerUpHandler && previousData.eligibleForClick)
                        {
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerPress, previousData,
                                  UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
                            previousData.eligibleForClick = false;
                        }

                        if (previousData.pointerDrag != null)
                        {
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerDrag, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.endDragHandler);
                            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                                UnityEngine.EventSystems.ExecuteEvents.dropHandler);
                            previousData.dragging = false;
                        }

                        UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousData.pointerCurrentRaycast.gameObject, previousData,
                            UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
                        return previousData;
                    }
                    break;
            }

            return previousData;

        }

        return null;
    }

    public void GetFirstRaycastResult(UnityEngine.EventSystems.PointerEventData pointerEventData, out UnityEngine.EventSystems.RaycastResult raycastResult, out System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> raycastResults)
    {
        raycastResults = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        }
        raycastResult = UnityEngine.EventSystems.BaseInputModule.FindFirstRaycast(raycastResults);
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

