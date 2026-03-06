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
using UnityEngine.InputSystem.LowLevel;

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
        InputSystem.onEvent += OnInputEvent;
    }

    void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device is Keyboard keyboard && eventPtr.IsA<StateEvent>())
            HandleKeyboardEvent(keyboard, eventPtr);
        else if (device is Mouse mouse)
            HandleMouseEvent(mouse, eventPtr);
        else if (device is Gamepad gamepad && eventPtr.IsA<StateEvent>())
            HandleGamepadEvent(gamepad, eventPtr);
    }

    private void HandleKeyboardEvent(Keyboard keyboard, InputEventPtr eventPtr)
    {
        foreach (var key in keyboard.allKeys)
        {
            float newValue = key.ReadValueFromEvent(eventPtr);
            bool wasPressed = key.isPressed;
            bool isNowPressed = newValue >= key.pressPoint;

            if (!wasPressed && isNowPressed)
            {
                pressedKey = key;
                KeyPressed = new List<int>();
                foreach (var e in AltKeyMapping.StringToKey)
                    if (keyboard[e.Value] == key)
                        KeyPressed.Add((int)AltKeyMapping.StringToKeyCode[e.Key]);
            }
            else if (wasPressed && !isNowPressed && key == pressedKey)
            {
                KeyReleased = new List<int>();
                foreach (var e in AltKeyMapping.StringToKey)
                    if (keyboard[e.Value] == key)
                        KeyReleased.Add((int)AltKeyMapping.StringToKeyCode[e.Key]);
            }
        }
    }

    private void HandleMouseEvent(Mouse mouse, InputEventPtr eventPtr)
    {
        // Scroll uses DeltaStateEvent
        if (eventPtr.IsA<DeltaStateEvent>())
        {
            if (mouse.scroll.ReadValueFromEvent(eventPtr) != Vector2.zero)
                wasScrolled = true;
            return;
        }

        if (!eventPtr.IsA<StateEvent>()) return;

        var allMouseControls = new List<ButtonControl>
        {
            mouse.leftButton, mouse.rightButton, mouse.middleButton,
            mouse.forwardButton, mouse.backButton
        };

        foreach (var mouseCtrl in allMouseControls)
        {
            float newValue = mouseCtrl.ReadValueFromEvent(eventPtr);
            bool wasPressed = mouseCtrl.isPressed;
            bool isNowPressed = newValue >= mouseCtrl.pressPoint;

            if (!wasPressed && isNowPressed)
            {
                pressedButton = mouseCtrl;
                var altKeyMapping = new AltKeyMapping(power);
                foreach (var e in altKeyMapping.mouseKeyCodeToButtonControl)
                    if (mouseCtrl == e.Value)
                        MousePressed = e.Key.ToString();
            }
            else if (wasPressed && !isNowPressed && pressedButton == mouseCtrl)
            {
                var altKeyMapping = new AltKeyMapping(power);
                foreach (var e in altKeyMapping.mouseKeyCodeToButtonControl)
                    if (mouseCtrl == e.Value)
                        MouseReleased = e.Key.ToString();
            }
        }
    }

    private void HandleGamepadEvent(Gamepad gamepad, InputEventPtr eventPtr)
    {
        foreach (var control in gamepad.allControls)
        {
            if (!(control is ButtonControl joystick)) continue;

            float newValue = joystick.ReadValueFromEvent(eventPtr);
            bool wasPressed = joystick.isPressed;
            bool isNowPressed = newValue >= joystick.pressPoint;

            if (!wasPressed && isNowPressed)
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
            else if (wasPressed && !isNowPressed && joystick == pressedButton)
            {
                var altKeyMapping = new AltKeyMapping(power);
                foreach (var e in altKeyMapping.joystickKeyCodeToGamepad)
                    if (joystick == e.Value)
                        JoystickReleased = e.Key.ToString();
            }
        }
    }
#endif
}
