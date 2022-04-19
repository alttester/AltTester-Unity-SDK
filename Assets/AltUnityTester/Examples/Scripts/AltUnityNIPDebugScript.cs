using System.Collections;
using System.Collections.Generic;
using Altom.AltUnityTester;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class AltUnityNIPDebugScript : MonoBehaviour
{
    private float power;

    public bool wasScrolled = false;
    public List<int> KeyPressed;
    public List<int> KeyReleased;
    public string MousePressed;
    public string MouseReleased;
    public string JoystickPressed;
    public string JoystickReleased;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.scroll.ReadValue() != Vector2.zero)
            wasScrolled = true;
        var allKeys = Keyboard.current.allKeys;
        foreach (var key in allKeys)
            if (key.wasPressedThisFrame)
            {
                KeyPressed = new List<int>() { };
                foreach (var e in AltUnityKeyMapping.StringToKey)
                    if (Keyboard.current[e.Value] == key)
                        KeyPressed.Add((int)AltUnityKeyMapping.StringToKeyCode[e.Key]);
            }
            else if (key.wasReleasedThisFrame)
            {
                KeyReleased = new List<int>() { };
                foreach (var e in AltUnityKeyMapping.StringToKey)
                    if (Keyboard.current[e.Value] == key)
                        KeyReleased.Add((int)AltUnityKeyMapping.StringToKeyCode[e.Key]);
            }
        var allMouseControls = new List<ButtonControl> { Mouse.current.leftButton, Mouse.current.rightButton, Mouse.current.middleButton, Mouse.current.forwardButton, Mouse.current.backButton };
        foreach (var mouseCtrl in allMouseControls)
            if (mouseCtrl.wasPressedThisFrame)
                foreach (var e in AltUnityKeyMapping.mouseKeyCodeToButtonControl)
                {
                    if (mouseCtrl == e.Value)
                        MousePressed = e.Key.ToString();
                }
            else if (mouseCtrl.wasReleasedThisFrame)
            {
                foreach (var e in AltUnityKeyMapping.mouseKeyCodeToButtonControl)
                {
                    if (mouseCtrl == e.Value)
                        MouseReleased = e.Key.ToString();
                }
            }
        var allJoysticks = Gamepad.current.allControls;
        foreach (var joystick in allJoysticks)
            if (joystick.GetType() == typeof(ButtonControl) && ((ButtonControl)joystick).wasPressedThisFrame)
            {
                if (joystick.parent.ReadValueAsObject().GetType() == typeof(Vector2))
                {
                    Vector2 axis = (Vector2)joystick.parent.ReadValueAsObject();
                    power = axis.x < 0 || axis.y < 0 ? -1 : 1;
                }
                else
                    power = 1;
                var altUnityKeyMapping = new AltUnityKeyMapping(power);
                foreach (var e in altUnityKeyMapping.joystickKeyCodeToGamepad)
                    if (joystick == e.Value)
                        JoystickPressed = e.Key.ToString();
            }
            else if (joystick.GetType() == typeof(ButtonControl) && ((ButtonControl)joystick).wasReleasedThisFrame)
            {
                var altUnityKeyMapping = new AltUnityKeyMapping(power);
                foreach (var e in altUnityKeyMapping.joystickKeyCodeToGamepad)
                    if (joystick == e.Value)
                        JoystickReleased = e.Key.ToString();
            }
    }
}