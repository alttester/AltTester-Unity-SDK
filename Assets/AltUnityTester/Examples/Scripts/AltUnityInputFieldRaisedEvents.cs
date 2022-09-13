
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class AltUnityInputFieldRaisedEvents : MonoBehaviour, ISubmitHandler
{
    private bool onValueChangedInvoked = false;
    private bool onSubmitInvoked = false;
    private bool onEndEditInvoked = false;
    public void OnValueChanged()
    {
        onValueChangedInvoked = true;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        onSubmitInvoked = true;
    }
    public void OnEndEdit()
    {
        onEndEditInvoked = true;
    }
}