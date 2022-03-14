using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltUnityNIPDebugScript : MonoBehaviour
{
    public bool wasScrolled = false;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.scroll.ReadValue() != Vector2.zero)
            wasScrolled = true;
    }
}
