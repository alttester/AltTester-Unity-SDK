/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltInputFieldRaisedEvents : MonoBehaviour, ISubmitHandler
{
#pragma warning disable CS0414
    private bool onValueChangedInvoked = false;
    private bool onSubmitInvoked = false;
    private bool onEndEditInvoked = false;
#pragma warning restore CS0414
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
