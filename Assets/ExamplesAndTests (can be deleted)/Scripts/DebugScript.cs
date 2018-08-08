using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugScript : MonoBehaviour {

    public void WahtHappens(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = (PointerEventData)baseEventData;
        Debug.Log(pointerEventData);
    }
}
