#if ENABLE_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using Altom.AltUnityTester;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Altom.AltUnityTester
{
    public class NewInputSystem : MonoBehaviour
    {
        private static float keyDownPower;

        public static InputTestFixture InputTestFixture = new InputTestFixture();
        public static NewInputSystem Instance;
        public static Keyboard Keyboard;
        public static Mouse Mouse;
        public static Gamepad Gamepad;
        public static Touchscreen Touchscreen;
        public static Accelerometer Accelerometer;
        public void Awake()
        {
            if (Instance == null)
                Instance = this;
            InputTestFixture = new InputTestFixture();
            Keyboard = (Keyboard)InputSystem.GetDevice("AltUnityKeyboard");
            if (Keyboard == null)
            {
                Keyboard = InputSystem.AddDevice<Keyboard>("AltUnityKeyboard");
            }

            Mouse = (Mouse)InputSystem.GetDevice("AltUnityMouse");
            if (Mouse == null)
            {
                Mouse = InputSystem.AddDevice<Mouse>("AltUnityMouse");

            }
            Gamepad = (Gamepad)InputSystem.GetDevice("AltUnityGamepad");
            if (Gamepad == null)
            {
                Gamepad = InputSystem.AddDevice<Gamepad>("AltUnityGamepad");

            }
            Touchscreen = (Touchscreen)InputSystem.GetDevice("AltUnityTouchscreen");
            if (Touchscreen == null)
            {
                Touchscreen = InputSystem.AddDevice<Touchscreen>("AltUnityTouchscreen");
            }
            Accelerometer = (Accelerometer)InputSystem.GetDevice("AltUnityAccelerometer");
            if (Accelerometer == null)
            {
                Accelerometer = InputSystem.AddDevice<Accelerometer>("AltUnityAccelerometer");
            }

        }
        internal static IEnumerator ScrollLifeCycle(float speed, float duration)
        {
            float currentTime = 0;
            float frameTime = 0;// using this because of a bug with yield return which waits only every other iteration
            while (currentTime <= duration - Time.fixedUnscaledDeltaTime)
            {
                InputTestFixture.Set(Mouse.scroll, new Vector2(speed * frameTime / duration, speed * frameTime / duration), queueEventOnly: true);
                var initialTime = Time.fixedUnscaledTime;
                yield return null;
                var afterTime = Time.fixedUnscaledTime;
                frameTime = afterTime - initialTime;
                currentTime += frameTime;
            }
            InputTestFixture.Set(Mouse.scroll, new Vector2(0, 0), queueEventOnly: true);
        }

        internal static IEnumerator MoveMouseCycle(UnityEngine.Vector2 location, float duration)
        {
            float time = 0;
            Mouse.MakeCurrent();
            var mousePosition = Mouse.current.position;
            var distance = location - new UnityEngine.Vector2(mousePosition.x.ReadValue(), mousePosition.y.ReadValue());
            do
            {
                UnityEngine.Vector2 delta;
                if (time + UnityEngine.Time.unscaledDeltaTime < duration)
                {
                    delta = distance * UnityEngine.Time.unscaledDeltaTime / duration;
                }
                else
                {
                    delta = location - new UnityEngine.Vector2(mousePosition.x.ReadValue(), mousePosition.y.ReadValue());
                }

                InputTestFixture.Move(Mouse.current.position, delta);
                yield return null;
                time += UnityEngine.Time.unscaledDeltaTime;
            } while (time < duration);
        }

        internal static IEnumerator TapElementCycle(GameObject target, int count, float interval)
        {
            Touchscreen.MakeCurrent();
            var touchId = 0;
            UnityEngine.Vector3 screenPosition;
            AltUnityRunner._altUnityRunner.FindCameraThatSeesObject(target, out screenPosition);
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                InputTestFixture.BeginTouch(touchId, screenPosition, screen: Touchscreen);
                yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
                time += Time.fixedUnscaledDeltaTime;
                InputTestFixture.EndTouch(touchId, screenPosition, screen: Touchscreen);
                if (i != count - 1 && time < interval)
                    yield return new WaitForSecondsRealtime(interval - time);
            }
        }
        internal static IEnumerator TapCoordinatesCycle(UnityEngine.Vector2 screenPosition, int count, float interval)
        {
            Touchscreen.MakeCurrent();
            var touchId = 0;
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                InputTestFixture.BeginTouch(touchId, screenPosition, screen: Touchscreen);
                yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
                time += Time.fixedUnscaledDeltaTime;
                InputTestFixture.EndTouch(touchId, screenPosition, screen: Touchscreen);
                if (i != count - 1 && time < interval)
                    yield return new WaitForSecondsRealtime(interval - time);
            }
        }

        internal static IEnumerator ClickElementLifeCycle(GameObject target, int count, float interval)
        {
            Mouse.MakeCurrent();
            UnityEngine.Vector3 screenPosition;
            AltUnityRunner._altUnityRunner.FindCameraThatSeesObject(target, out screenPosition);
            InputTestFixture.Set(Mouse.current.position, screenPosition);
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                InputTestFixture.Press(Mouse.leftButton);
                yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
                time += Time.fixedUnscaledDeltaTime;
                InputTestFixture.Release(Mouse.leftButton);
                if (i != count - 1 && time < interval)
                    yield return new WaitForSecondsRealtime(interval - time);
            }
        }
        internal static IEnumerator ClickCoordinatesLifeCycle(UnityEngine.Vector2 screenPosition, int count, float interval)
        {
            Mouse.MakeCurrent();
            InputTestFixture.Set(Mouse.current.position, screenPosition);
            for (int i = 0; i < count; i++)
            {
                float time = 0;
                InputTestFixture.Press(Mouse.leftButton);
                yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
                time += Time.fixedUnscaledDeltaTime;
                InputTestFixture.Release(Mouse.leftButton);
                if (i != count - 1 && time < interval)
                    yield return new WaitForSecondsRealtime(interval - time);
            }
        }

        internal static void KeyDown(KeyCode keyCode, float power)
        {
            keyDownPower = power;
            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, power);
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(power, buttonControl);
            else
                InputTestFixture.Press(buttonControl);
        }

        internal static void KeyUp(KeyCode keyCode)
        {
            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, keyDownPower);
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(0, buttonControl);
            else
                InputTestFixture.Release(buttonControl);
        }

        internal static IEnumerator KeyPressLifeCycle(KeyCode keyCode, float power, float duration)
        {
            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, power);
            yield return null;
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(power, buttonControl);
            else
                InputTestFixture.Press(buttonControl);
            yield return new WaitForSeconds(duration);
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(0, buttonControl);
            else
                InputTestFixture.Release(buttonControl, queueEventOnly: true);
        }

        internal static IEnumerator AccelerationLifeCycle(Vector3 accelerationValue, float duration)
        {
            float currentTime = 0;
            float frameTime = 0;// using this because of a bug with yield return which waits only every other iteration
            InputSystem.EnableDevice(Accelerometer);
            while (currentTime <= duration - Time.fixedUnscaledDeltaTime)
            {
                InputTestFixture.Set(Accelerometer.acceleration, accelerationValue * frameTime / duration, queueEventOnly: true);
                var initialTime = Time.fixedUnscaledTime;
                yield return null;
                var afterTime = Time.fixedUnscaledTime;
                frameTime = afterTime - initialTime;
                currentTime += frameTime;
            }
            InputTestFixture.Set(Accelerometer.acceleration, Vector3.zero);
            InputSystem.DisableDevice(Accelerometer);
        }

        #region private interface
        private static ButtonControl keyCodeToButtonControl(KeyCode keyCode, float power = 1)
        {
            foreach (var e in AltUnityKeyMapping.StringToKeyCode)
                if (e.Value == keyCode)
                    return Keyboard.current[AltUnityKeyMapping.StringToKey[e.Key]];
            foreach (var e in AltUnityKeyMapping.mouseKeyCodeToButtonControl)
                if (e.Key == keyCode)
                    return e.Value;
            AltUnityKeyMapping altUnityKeyMapping = new AltUnityKeyMapping(power);
            foreach (var e in altUnityKeyMapping.joystickKeyCodeToGamepad)
                if (e.Key == keyCode)
                    return e.Value;
            return null;
        }

        private static void setStick(float value, ButtonControl buttonControl)
        {
            if (buttonControl == Gamepad.current.leftStick.up)
                InputTestFixture.Set(Gamepad.current.leftStick.y, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.leftStick.down)
                InputTestFixture.Set(Gamepad.current.leftStick.y, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.leftStick.right)
                InputTestFixture.Set(Gamepad.current.leftStick.x, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.leftStick.left)
                InputTestFixture.Set(Gamepad.current.leftStick.x, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.rightStick.up)
                InputTestFixture.Set(Gamepad.current.rightStick.y, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.rightStick.down)
                InputTestFixture.Set(Gamepad.current.rightStick.y, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.rightStick.right)
                InputTestFixture.Set(Gamepad.current.rightStick.x, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.rightStick.left)
                InputTestFixture.Set(Gamepad.current.rightStick.x, value, queueEventOnly: true);
        }
        #endregion
    }

}
#else
namespace Altom.AltUnityTester
{
    public class NewInputSystem
    {

    }
}
#endif