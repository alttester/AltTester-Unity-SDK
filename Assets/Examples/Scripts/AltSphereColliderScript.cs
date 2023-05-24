using System.Collections.Generic;
using UnityEngine;

public class AltSphereColliderScript : MonoBehaviour
{
    private HashSet<string> monoBehaviourEventsRaised = new HashSet<string>();

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
    protected void OnMouseOver()
    {
        monoBehaviourEventsRaised.Add("OnMouseOver");
    }
}
