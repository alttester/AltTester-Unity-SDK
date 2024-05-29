/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using System.Collections.Generic;
using AltTester.AltTesterUnitySDK;
using AltTester.AltTesterUnitySDK.InputModule;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class AltNIPDebugScript : MonoBehaviour
{
    private float power;

    public bool wasScrolled = false;
    public List<int> KeyPressed;
    public List<int> KeyReleased;
    public string MousePressed;
    public string MouseReleased;
    public string JoystickPressed;
    public string JoystickReleased;
    public KeyControl pressedKey;
    public ButtonControl pressedButton;

    // Update is called once per frame
    void Update()
    {
#if ALTTESTER && ENABLE_INPUT_SYSTEM

        if (Mouse.current.scroll.ReadValue() != Vector2.zero)
            wasScrolled = true;
        var allKeys = Keyboard.current.allKeys;
        foreach (var key in allKeys)
            if (key.isPressed)
            {
                pressedKey = key;
                KeyPressed = new List<int>() { };
                foreach (var e in AltKeyMapping.StringToKey)
                {
                    if (Keyboard.current[e.Value] == key)
                    {
                        KeyPressed.Add((int)AltKeyMapping.StringToKeyCode[e.Key]);
                    }
                }
            }
            else if (key == pressedKey && !key.isPressed)
            {
                KeyReleased = new List<int>() { };
                foreach (var e in AltKeyMapping.StringToKey)
                    if (Keyboard.current[e.Value] == key)
                        KeyReleased.Add((int)AltKeyMapping.StringToKeyCode[e.Key]);
            }
        var allMouseControls = new List<ButtonControl> { Mouse.current.leftButton, Mouse.current.rightButton, Mouse.current.middleButton, Mouse.current.forwardButton, Mouse.current.backButton };
        foreach (var mouseCtrl in allMouseControls)
            if (mouseCtrl.wasPressedThisFrame)
            {
                pressedButton = mouseCtrl;
                var altKeyMapping = new AltKeyMapping(power);
                foreach (var e in altKeyMapping.mouseKeyCodeToButtonControl)
                {
                    if (mouseCtrl == e.Value)
                        MousePressed = e.Key.ToString();
                }
            }
            else if (pressedButton == mouseCtrl && !mouseCtrl.isPressed)
            {
                var altKeyMapping = new AltKeyMapping(power);
                foreach (var e in altKeyMapping.mouseKeyCodeToButtonControl)
                {
                    if (mouseCtrl == e.Value)
                        MouseReleased = e.Key.ToString();
                }
            }
        var allJoysticks = Gamepad.current.allControls;
        foreach (var joystick in allJoysticks)
            if (joystick.GetType() == typeof(ButtonControl) && ((ButtonControl)joystick).wasPressedThisFrame)
            {
                pressedButton = (ButtonControl)joystick;
                if (joystick.parent.ReadValueAsObject().GetType() == typeof(Vector2))
                {
                    Vector2 axis = (Vector2)joystick.parent.ReadValueAsObject();
                    power = axis.x < 0 || axis.y < 0 ? -1 : 1;
                }
                else
                    power = 1;
                var altKeyMapping = new AltKeyMapping(power);
                foreach (var e in altKeyMapping.joystickKeyCodeToGamepad)
                    if (joystick == e.Value)
                        JoystickPressed = e.Key.ToString();
            }
            else if (joystick.GetType() == typeof(ButtonControl) && ((ButtonControl)joystick) == pressedButton && !((ButtonControl)joystick).isPressed)
            {
                var altKeyMapping = new AltKeyMapping(power);
                foreach (var e in altKeyMapping.joystickKeyCodeToGamepad)
                    if (joystick == e.Value)
                        JoystickReleased = e.Key.ToString();
            }
#endif

    }
}
