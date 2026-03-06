/*
    Copyright(C) 2025 Altom Consulting

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

#if ALTTESTER && ENABLE_INPUT_SYSTEM
    void OnEnable()
    {
        InputSystem.onAfterUpdate += OnAfterInputUpdate;
    }

    void OnDisable()
    {
        InputSystem.onAfterUpdate -= OnAfterInputUpdate;
    }

    private void OnAfterInputUpdate()
    {
        var keyboard = NewInputSystem.Keyboard;
        if (keyboard != null)
        {
            foreach (var key in keyboard.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    pressedKey = key;
                    KeyPressed = new List<int>();
                    foreach (var e in AltKeyMapping.StringToKey)
                        if (keyboard[e.Value] == key)
                            KeyPressed.Add((int)AltKeyMapping.StringToKeyCode[e.Key]);
                }
                if (key.wasReleasedThisFrame && key == pressedKey)
                {
                    KeyReleased = new List<int>();
                    foreach (var e in AltKeyMapping.StringToKey)
                        if (keyboard[e.Value] == key)
                            KeyReleased.Add((int)AltKeyMapping.StringToKeyCode[e.Key]);
                }
            }
        }

        var mouse = NewInputSystem.Mouse;
        if (mouse != null)
        {
            if (mouse.scroll.ReadValue() != Vector2.zero)
                wasScrolled = true;

            var allMouseControls = new List<ButtonControl>
            {
                mouse.leftButton, mouse.rightButton, mouse.middleButton,
                mouse.forwardButton, mouse.backButton
            };

            foreach (var mouseCtrl in allMouseControls)
            {
                if (mouseCtrl.wasPressedThisFrame)
                {
                    pressedButton = mouseCtrl;
                    var altKeyMapping = new AltKeyMapping(power);
                    foreach (var e in altKeyMapping.mouseKeyCodeToButtonControl)
                        if (mouseCtrl == e.Value)
                            MousePressed = e.Key.ToString();
                }
                else if (mouseCtrl.wasReleasedThisFrame && pressedButton == mouseCtrl)
                {
                    var altKeyMapping = new AltKeyMapping(power);
                    foreach (var e in altKeyMapping.mouseKeyCodeToButtonControl)
                        if (mouseCtrl == e.Value)
                            MouseReleased = e.Key.ToString();
                }
            }
        }

        var gamepad = NewInputSystem.Gamepad;
        if (gamepad != null)
        {
            foreach (var control in gamepad.allControls)
            {
                if (!(control is ButtonControl joystick)) continue;

                if (joystick.wasPressedThisFrame)
                {
                    pressedButton = joystick;
                    if (joystick.parent.ReadValueAsObject()?.GetType() == typeof(Vector2))
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
                else if (joystick.wasReleasedThisFrame && joystick == pressedButton)
                {
                    var altKeyMapping = new AltKeyMapping(power);
                    foreach (var e in altKeyMapping.joystickKeyCodeToGamepad)
                        if (joystick == e.Value)
                            JoystickReleased = e.Key.ToString();
                }
            }
        }
    }
#endif
}
