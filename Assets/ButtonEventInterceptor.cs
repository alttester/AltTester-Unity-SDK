using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEventInterceptor : MonoBehaviour,
                                        IPointerClickHandler,
                                        IDeselectHandler,
                                        IMoveHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, ISubmitHandler, ISelectHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler, IDragHandler
{
    public DisplayOrderOfEvents DisplayOrderOfEvents;
    public void Start()
    {
        DisplayOrderOfEvents = GameObject.FindObjectOfType<DisplayOrderOfEvents>();
    }
    // Called when the button is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnPointerClickEventDetected();
        // Perform custom logic when the button is clicked
    }

    // Called when the button is deselected (loses focus)
    public void OnDeselect(BaseEventData eventData)
    {
        DisplayOrderOfEvents.OnDeselectEventDetected();
        // Perform custom logic when the button is deselected
    }

    // Called when the button receives a movement event (e.g., arrow key or joystick input)
    public void OnMove(AxisEventData eventData)
    {
        DisplayOrderOfEvents.OnMoveEventDetected();
        // Perform custom logic when the button is moved (e.g., arrow key navigation)
    }

    public void OnSelect(BaseEventData eventData)
    {
        DisplayOrderOfEvents.OnSelectEventDetected();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        DisplayOrderOfEvents.OnSubmitEventDetected();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnPointerDownEventDetected();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnPointerUpEventDetected();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnPointerExitEventDetected();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnPointerEnterEventDetected();
    }

    public void OnDrag(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnDragEventDetected();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnInitializePotentialDragEventDetected();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnEndDragEventDetected();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DisplayOrderOfEvents.OnBeginDragEventDetected();
    }


    public void DestroyObject()
    {
        DestroyImmediate(gameObject);
    }
    public void MoveObject()
    {
        var position = gameObject.transform.position;
        gameObject.transform.position = new Vector3(position.x, position.y + 200, position.z);
    }
}
