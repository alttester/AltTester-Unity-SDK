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

#if ALTTESTER && ENABLE_INPUT_SYSTEM

using System;
using System.Collections;
using System.Collections.Generic;
using AltTester;
#if USE_INPUT_SYSTEM_1_3
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using InputTouchPhase = UnityEngine.InputSystem.TouchPhase;
using System.Linq;

namespace AltTester.AltTesterUnitySDK.InputModule
{
    public class NewInputSystem : MonoBehaviour
    {
        private static Vector2 endTouchScreenPos;
        private static float keyDownPower;

        public static NewInputSystem Instance;
        public static Keyboard Keyboard;
        public static Mouse Mouse;
        public static Gamepad Gamepad;
        public static Touchscreen Touchscreen;
        public static Accelerometer Accelerometer;

        private static KeyboardState currentKeyboardState = new KeyboardState();
        private static MouseState currentMouseState = new MouseState();
        private static GamepadState currentGamepadState = new GamepadState();
        public static bool[] touches = new bool[] { false, true, true, true, true, true, true, true, true, true, true };
        public void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                return;
            }
#if USE_INPUT_SYSTEM_1_3
            Application.runInBackground = true;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif

            initiateDevices();
            currentMouseState = new MouseState { position = new Vector2(0, 0) };
            InputSystem.QueueStateEvent(Mouse, currentMouseState);
            EnableDefaultDevicesAndDisableAltDevices();

        }

        private static void initiateDevices()
        {
            Keyboard = (Keyboard)InputSystem.GetDevice("AltKeyboard");
            if (Keyboard == null)
            {
                Keyboard = InputSystem.AddDevice<Keyboard>("AltKeyboard");
            }

            Mouse = (Mouse)InputSystem.GetDevice("AltMouse");
            if (Mouse == null)
            {
                Mouse = InputSystem.AddDevice<Mouse>("AltMouse");
            }
            Gamepad = (Gamepad)InputSystem.GetDevice("AltGamepad");
            if (Gamepad == null)
            {
                Gamepad = InputSystem.AddDevice<Gamepad>("AltGamepad");

            }
            Touchscreen = (Touchscreen)InputSystem.GetDevice("AltTouchscreen");
            if (Touchscreen == null)
            {
                Touchscreen = InputSystem.AddDevice<Touchscreen>("AltTouchscreen");
            }
            Accelerometer = (Accelerometer)InputSystem.GetDevice("AltAccelerometer");
            if (Accelerometer == null)
            {
                Accelerometer = InputSystem.AddDevice<Accelerometer>("AltAccelerometer");
            }
        }

        public void ResetInput()
        {

#if USE_INPUT_SYSTEM_1_3
            Application.runInBackground = true;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;

            InputSystem.ResetDevice(Keyboard, true);
            InputSystem.ResetDevice(Mouse, true);
            InputSystem.ResetDevice(Gamepad, true);
            InputSystem.ResetDevice(Touchscreen, true);
            InputSystem.ResetDevice(Accelerometer, true);
#else

            InputSystem.RemoveDevice(Keyboard);
            InputSystem.RemoveDevice(Mouse);
            InputSystem.RemoveDevice(Gamepad);
            InputSystem.RemoveDevice(Touchscreen);
            InputSystem.RemoveDevice(Accelerometer);
            initiateDevices();
#endif
            touches = new bool[] { false, true, true, true, true, true, true, true, true, true, true };


        }

        public static void DisableDefaultDevicesAndEnableAltDevices()
        {
            foreach (var device in InputSystem.devices)
            {
                if (device.name.Contains("Alt"))
                {
                    if (device != null)
                    {
                        InputSystem.EnableDevice(device);
                        device.MakeCurrent();
                    }
                }
                else
                {
                    InputSystem.DisableDevice(device);
                }
            }

        }
        public static void EnableDefaultDevicesAndDisableAltDevices()
        {
            foreach (var device in InputSystem.devices)
            {
                if (device.name.Contains("Alt"))
                {
                    InputSystem.DisableDevice(device);
                }
                else
                {
                    if (device != null)
                    {
                        InputSystem.EnableDevice(device);
                        device.MakeCurrent();
                    }

                }
            }

        }

        internal static IEnumerator ScrollLifeCycle(float speedVertical, float speedHorizontal, float duration)
        {
            float currentTime = 0;
            while (currentTime <= duration)
            {
                InputSystem.QueueDeltaStateEvent(Mouse.scroll, new Vector2(speedHorizontal * Time.unscaledDeltaTime / duration, speedVertical * Time.unscaledDeltaTime / duration));
                yield return null;
                currentTime += Time.unscaledDeltaTime;
            }
            InputSystem.QueueDeltaStateEvent(Mouse.scroll, Vector2.zero);
        }

        internal static IEnumerator MoveMouseCycle(UnityEngine.Vector2 location, float duration)
        {
            float time = 0;
            yield return null;
            var mousePosition = new Vector2(Mouse.position.x.ReadValue(), Mouse.position.y.ReadValue());
            var distance = location - new UnityEngine.Vector2(mousePosition.x, mousePosition.y);
            var deltaUnchanged = false;
            while (time < duration)
            {
                yield return null;
                time += Time.unscaledDeltaTime;
                UnityEngine.Vector2 delta;
                if (time + Time.unscaledDeltaTime < duration)
                {
                    delta = distance * Time.unscaledDeltaTime / duration;
                }
                else
                {
                    delta = location - new UnityEngine.Vector2(mousePosition.x, mousePosition.y);
                }
                mousePosition += delta;
                if (delta == Vector2.zero)
                {
                    deltaUnchanged = true;
                    break;
                }
                currentMouseState.position = mousePosition;
                InputSystem.QueueStateEvent(Mouse, currentMouseState);
            }
            if (deltaUnchanged)
            {

                currentMouseState.position = mousePosition * 1.01f;
                InputSystem.QueueStateEvent(Mouse, currentMouseState);
                currentMouseState.position = mousePosition;
                InputSystem.QueueStateEvent(Mouse, currentMouseState);
                while (time < duration)
                {
                    time += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
        }


        internal static IEnumerator TapElementCycle(GameObject target, int count, float interval)
        {
            Touchscreen.MakeCurrent();
            var touchId = getFreeTouch(touches);
            touches[touchId] = false;
            UnityEngine.Vector3 screenPosition;
            FindObjectViaRayCast.FindCameraThatSeesObject(target, out screenPosition);
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                InputSystem.QueueStateEvent(Touchscreen, new TouchState { touchId = touchId, phase = InputTouchPhase.Began, position = screenPosition });
                yield return null;
                time += Time.unscaledDeltaTime;
                InputSystem.QueueStateEvent(Touchscreen, new TouchState { touchId = touchId, phase = InputTouchPhase.Ended, position = screenPosition });
                while (i != count - 1 && time < interval)
                {
                    time += Time.unscaledDeltaTime;
                    yield return null;
                }

            }
            touches[touchId] = true;
        }

        internal static IEnumerator TapCoordinatesCycle(UnityEngine.Vector2 screenPosition, int count, float interval)
        {
            Touchscreen.MakeCurrent();
            var touchId = getFreeTouch(touches);
            touches[touchId] = false;
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                InputSystem.QueueStateEvent(Touchscreen, new TouchState { touchId = touchId, phase = InputTouchPhase.Began, position = screenPosition });
                yield return null;
                time += Time.unscaledDeltaTime;
                endTouchScreenPos = screenPosition;
                InputSystem.QueueStateEvent(Touchscreen, new TouchState { touchId = touchId, phase = InputTouchPhase.Ended, position = screenPosition });
                while (i != count - 1 && time < interval)
                {
                    time += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            touches[touchId] = true;
        }

        internal static IEnumerator ClickElementLifeCycle(GameObject target, int count, float interval)
        {
            Mouse.MakeCurrent();
            UnityEngine.Vector3 screenPosition;
            FindObjectViaRayCast.FindCameraThatSeesObject(target, out screenPosition);
            currentMouseState.position = screenPosition;
            InputSystem.QueueStateEvent(Mouse, currentMouseState);
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                currentMouseState.buttons = 1;
                InputSystem.QueueStateEvent(Mouse, currentMouseState);
                yield return null;
                time += Time.unscaledDeltaTime;
                currentMouseState.buttons = 0;
                InputSystem.QueueStateEvent(Mouse, currentMouseState);
                while (i != count - 1 && time < interval)
                {
                    time += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
        }

        internal static IEnumerator ClickCoordinatesLifeCycle(UnityEngine.Vector2 screenPosition, int count, float interval)
        {
            Mouse.MakeCurrent();
            currentMouseState.position = screenPosition;
            InputSystem.QueueStateEvent(Mouse, currentMouseState);
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                currentMouseState.buttons = 1;
                InputSystem.QueueStateEvent(Mouse, currentMouseState);
                yield return null;
                time += Time.unscaledDeltaTime;
                currentMouseState.buttons = 0;
                InputSystem.QueueStateEvent(Mouse, currentMouseState);
                while (i != count - 1 && time < interval)
                {
                    time += Time.unscaledDeltaTime;
                    yield return null;
                }

            }
        }

        internal static void KeyDown(KeyCode keyCode, float power)
        {
            keyDownPower = power;
            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, power);
            keyDown(keyCode, power, buttonControl);
        }

        internal static void KeyUp(KeyCode keyCode)
        {

            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, keyDownPower);
            keyUp(keyCode, buttonControl);
        }

        internal static IEnumerator KeyPressLifeCycle(KeyCode keyCode, float power, float duration)
        {
            keyDownPower = power;
            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, power);
            keyDown(keyCode, power, buttonControl);
            float currentTime = 0;
            while (currentTime <= duration)
            {
                yield return null;
                currentTime += Time.unscaledDeltaTime;
            }
            keyUp(keyCode, buttonControl);
        }

        internal static IEnumerator AccelerationLifeCycle(Vector3 accelerationValue, float duration)
        {
            float currentTime = 0;
            while (currentTime <= duration - Time.unscaledDeltaTime)
            {
                InputSystem.QueueDeltaStateEvent(Accelerometer, accelerationValue * Time.unscaledDeltaTime / duration);
                yield return null;
                currentTime += Time.unscaledDeltaTime;
            }
            InputSystem.QueueDeltaStateEvent(Accelerometer, Vector3.zero);
        }


        internal static IEnumerator MultipointSwipeLifeCycle(UnityEngine.Vector2[] positions, float duration)
        {


            Touchscreen.MakeCurrent();
            float oneTouchDuration = duration / (positions.Length - 1);
            var touchId = BeginTouch(positions[0]);
            yield return null;
            for (int i = 1; i < positions.Length; i++)
            {

                float time = 0;
                Vector2 currentPosition = positions[i - 1];
                var distance = positions[i] - currentPosition;
                while (time < oneTouchDuration)
                {
                    yield return null;
                    time += UnityEngine.Time.unscaledDeltaTime;
                    UnityEngine.Vector2 delta;

                    if (time < oneTouchDuration)
                    {
                        delta = distance * UnityEngine.Time.unscaledDeltaTime / oneTouchDuration;
                    }
                    else
                    {
                        delta = positions[i] - currentPosition;
                    }
                    currentPosition += delta;

                    MoveTouch(touchId, currentPosition);
                }
            }
            endTouchScreenPos = positions[positions.Length - 1];
            yield return CoroutineManager.Instance.StartCoroutine(EndTouch(touchId));

        }
        internal static int BeginTouch(Vector3 screenPosition)
        {
            var fingerId = getFreeTouch(touches);
            touches[fingerId] = false;
            InputSystem.QueueStateEvent(Touchscreen, new TouchState { touchId = fingerId, phase = InputTouchPhase.Began, position = screenPosition });
            endTouchScreenPos = screenPosition;
            return fingerId;
        }

        internal static void MoveTouch(int fingerId, Vector3 screenPosition)
        {
            InputSystem.QueueStateEvent(Touchscreen, new TouchState { touchId = fingerId, phase = InputTouchPhase.Moved, position = screenPosition });
            endTouchScreenPos = screenPosition;
        }

        internal static IEnumerator EndTouch(int fingerId)
        {
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
                yield return new UnityEngine.WaitForEndOfFrame();


            InputSystem.QueueStateEvent(Touchscreen, new TouchState { touchId = fingerId, phase = InputTouchPhase.Ended, position = endTouchScreenPos });
            touches[fingerId] = true;

        }

        #region private interface
        private static ButtonControl keyCodeToButtonControl(KeyCode keyCode, float power = 1)
        {
            foreach (var e in AltKeyMapping.StringToKeyCode)
                if (e.Value == keyCode)
                    return Keyboard[AltKeyMapping.StringToKey[e.Key]];
            AltKeyMapping altKeyMapping = new AltKeyMapping(power);
            foreach (var e in altKeyMapping.mouseKeyCodeToButtonControl)
                if (e.Key == keyCode)
                    return e.Value;
            foreach (var e in altKeyMapping.joystickKeyCodeToGamepad)
                if (e.Key == keyCode)
                    return e.Value;
            return null;
        }

        private static void setStick(float value, ButtonControl buttonControl)
        {
            if (buttonControl == Gamepad.leftStick.up || buttonControl == Gamepad.leftStick.down)
                InputSystem.QueueDeltaStateEvent(Gamepad.leftStick.y, value);
            else if (buttonControl == Gamepad.leftStick.right || buttonControl == Gamepad.leftStick.left)
                InputSystem.QueueDeltaStateEvent(Gamepad.leftStick.x, value);
            else if (buttonControl == Gamepad.rightStick.up || buttonControl == Gamepad.rightStick.down)
                InputSystem.QueueDeltaStateEvent(Gamepad.rightStick.y, value);
            else if (buttonControl == Gamepad.rightStick.right || buttonControl == Gamepad.rightStick.left)
                InputSystem.QueueDeltaStateEvent(Gamepad.rightStick.x, value);
        }

        private static void keyDown(KeyCode keyCode, float power, ButtonControl buttonControl)
        {
            if (buttonControl == null) return;
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(power, buttonControl);

            // Keyboard key
            if (buttonControl.device is Keyboard)
            {
                string key = AltKeyMapping.StringToKeyCode.FirstOrDefault(x => x.Value == keyCode).Key;
                if (!string.IsNullOrEmpty(key) && AltKeyMapping.StringToKey.TryGetValue(key, out var keyboardKey))
                {
                    currentKeyboardState.Press(keyboardKey);
                    InputSystem.QueueStateEvent(Keyboard, currentKeyboardState); // 3. Set the new state

                }
            }
            // Mouse button
            else if (buttonControl.device is Mouse)
            {
                // A more robust way to check buttons
                if (buttonControl == Mouse.leftButton) currentMouseState.buttons |= (1 << 0);
                else if (buttonControl == Mouse.rightButton) currentMouseState.buttons |= (1 << 1);
                else if (buttonControl == Mouse.middleButton) currentMouseState.buttons |= (1 << 2);
                else if (buttonControl == Mouse.forwardButton) currentMouseState.buttons |= (1 << 3);
                else if (buttonControl == Mouse.backButton) currentMouseState.buttons |= (1 << 4);


                InputSystem.QueueStateEvent(Mouse, currentMouseState);
            }
            // Gamepad button
            else if (buttonControl.device is Gamepad)
            {
                currentGamepadState.buttons |= AltKeyMapping.JoystickKeyCodeToGamepadUInt[keyCode];
                InputSystem.QueueStateEvent(Gamepad, currentGamepadState);
            }

        }

        private static void keyUp(KeyCode keyCode, ButtonControl buttonControl, bool queueEventOnly = false)
        {
            if (buttonControl == null) return;
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(0, buttonControl);

            // Keyboard key
            if (buttonControl.device is Keyboard)
            {
                string key = AltKeyMapping.StringToKeyCode.FirstOrDefault(x => x.Value == keyCode).Key;
                if (!string.IsNullOrEmpty(key) && AltKeyMapping.StringToKey.TryGetValue(key, out var keyValue))
                {
                    currentKeyboardState.Release(keyValue);
                    InputSystem.QueueStateEvent(Keyboard, currentKeyboardState); // 3. Set the new state
                }
            }
            // Mouse button
            else if (buttonControl.device is Mouse)
            {
                if (buttonControl == Mouse.leftButton) currentMouseState.buttons = (ushort)(currentMouseState.buttons & ~(1 << 0));
                else if (buttonControl == Mouse.rightButton) currentMouseState.buttons = (ushort)(currentMouseState.buttons & ~(1 << 1));
                else if (buttonControl == Mouse.middleButton) currentMouseState.buttons = (ushort)(currentMouseState.buttons & ~(1 << 2));
                else if (buttonControl == Mouse.forwardButton) currentMouseState.buttons = (ushort)(currentMouseState.buttons & ~(1 << 3));
                else if (buttonControl == Mouse.backButton) currentMouseState.buttons = (ushort)(currentMouseState.buttons & ~(1 << 4));

                InputSystem.QueueStateEvent(Mouse, currentMouseState);
            }
            // Gamepad button
            else if (buttonControl.device is Gamepad)
            {
                currentGamepadState.buttons &= ~AltKeyMapping.JoystickKeyCodeToGamepadUInt[keyCode];
                InputSystem.QueueStateEvent(Gamepad, currentGamepadState);
            }
        }

        private static int getFreeTouch(bool[] touches)
        {
            for (int i = 1; i < touches.Length; i++)
            {
                if (touches[i]) return i;
            }
            return 0;
        }
        #endregion
    }

}

#else
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.InputModule
{
    public class NewInputSystem : MonoBehaviour
    {

    }
}
#endif
