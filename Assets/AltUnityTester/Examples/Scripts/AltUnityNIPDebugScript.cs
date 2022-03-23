using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltUnityNIPDebugScript : MonoBehaviour
{
    public bool wasScrolled = false;
    public string keyPressed = "no key";

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.scroll.ReadValue() != Vector2.zero)
            wasScrolled = true;
        var allKeys = Keyboard.current.allKeys;
        foreach(var key in allKeys)
            if (key.wasPressedThisFrame)
                keyPressed = key.displayName;
    }
}
