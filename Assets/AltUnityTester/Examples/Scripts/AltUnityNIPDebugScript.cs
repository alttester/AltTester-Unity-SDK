using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class AltUnityNIPDebugScript : MonoBehaviour
{
    public bool wasScrolled = false;
    public string keyPressed;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.scroll.ReadValue() != Vector2.zero)
            wasScrolled = true;
        var allKeys = Keyboard.current.allKeys;
        foreach (var key in allKeys)
            if (key.isPressed)
                keyPressed = key.keyCode.ToString();
        var allMouseControls = new List<ButtonControl> { Mouse.current.leftButton, Mouse.current.rightButton, Mouse.current.middleButton, Mouse.current.forwardButton, Mouse.current.backButton };
        foreach (var mouseCtrl in allMouseControls)
            if (mouseCtrl.isPressed)
                keyPressed = mouseCtrl.displayName;
        var allJoysticks = Gamepad.current.allControls;
        foreach (var joystick in allJoysticks)
            if (joystick.IsPressed() && joystick.GetType() == typeof(ButtonControl))
                keyPressed = joystick.displayName;
    }
}
