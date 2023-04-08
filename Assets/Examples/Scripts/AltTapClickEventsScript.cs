using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AltTapClickEventsScript : MonoBehaviour,
 IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler,
 IPointerClickHandler
{
    private HashSet<string> monoBehaviourEventsRaised = new HashSet<string>();
    private HashSet<string> eventSystemEventsRaised = new HashSet<string>();

    protected void OnMouseEnter()
    {
        monoBehaviourEventsRaised.Add("OnMouseEnter");
    }
    protected void OnMouseDown()
    {
        monoBehaviourEventsRaised.Add("OnMouseDown");
    }
    protected void OnMouseUp()
    {
        monoBehaviourEventsRaised.Add("OnMouseUp");
    }

    protected void OnMouseUpAsButton()
    {
        monoBehaviourEventsRaised.Add("OnMouseUpAsButton");
    }
    protected void OnMouseExit()
    {
        monoBehaviourEventsRaised.Add("OnMouseExit");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerExit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerUp");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        eventSystemEventsRaised.Add("OnPointerClick");
    }
}
