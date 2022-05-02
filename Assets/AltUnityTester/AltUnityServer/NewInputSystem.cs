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
            InputTestFixture.Set(Mouse.position, new Vector2(0, 0));


        }

        internal static IEnumerator ScrollLifeCycle(float speedVertical, float speedHorizontal, float duration)
        {
            float currentTime = 0;
            float frameTime = 0;// using this because of a bug with yield return which waits only every other iteration
            while (currentTime <= duration - Time.fixedUnscaledDeltaTime)
            {
                InputTestFixture.Set(Mouse.scroll, new Vector2(speedHorizontal * frameTime / duration, speedVertical * frameTime / duration), queueEventOnly: true);
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
            yield return null;
            var mousePosition = new Vector2(Mouse.position.x.ReadValue(),Mouse.position.y.ReadValue());
            var distance = location - new UnityEngine.Vector2(mousePosition.x, mousePosition.y);
            
            var deltaUnchanged=false;
            while(time<duration)
            {
                UnityEngine.Vector2 delta;
                if (time + UnityEngine.Time.unscaledDeltaTime < duration)
                {
                    delta = distance * UnityEngine.Time.unscaledDeltaTime / duration;
                }
                else
                {
                    delta = location - new UnityEngine.Vector2(mousePosition.x, mousePosition.y);
                }

                mousePosition += delta;
                if(delta==Vector2.zero)
                {
                    deltaUnchanged=true;
                    break;
                }
                InputTestFixture.Move(Mouse.position, mousePosition, delta);
                yield return null;
                time += UnityEngine.Time.unscaledDeltaTime;
            }
            if(deltaUnchanged){
                InputTestFixture.Move(Mouse.position, mousePosition*1.01f, Vector2.zero);
                InputTestFixture.Move(Mouse.position, mousePosition, Vector2.zero);
                yield return new WaitForSecondsRealtime(duration - time);
            }
            InputTestFixture.Set(Mouse.position, mousePosition);

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
            keyDown(keyCode, power, buttonControl);
        }

        internal static void KeyUp(KeyCode keyCode)
        {
            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, keyDownPower);
            keyUp(keyCode, buttonControl);
        }

        internal static IEnumerator KeyPressLifeCycle(KeyCode keyCode, float power, float duration)
        {
            ButtonControl buttonControl = keyCodeToButtonControl(keyCode, power);
            yield return null;
            keyDown(keyCode, power, buttonControl);
            yield return new WaitForSeconds(duration);
            keyUp(keyCode, buttonControl, true);
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
            if (buttonControl == Gamepad.current.leftStick.up || buttonControl == Gamepad.current.leftStick.down)
                InputTestFixture.Set(Gamepad.current.leftStick.y, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.leftStick.right || buttonControl == Gamepad.current.leftStick.left)
                InputTestFixture.Set(Gamepad.current.leftStick.x, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.rightStick.up || buttonControl == Gamepad.current.rightStick.down)
                InputTestFixture.Set(Gamepad.current.rightStick.y, value, queueEventOnly: true);
            else if (buttonControl == Gamepad.current.rightStick.right || buttonControl == Gamepad.current.rightStick.left)
                InputTestFixture.Set(Gamepad.current.rightStick.x, value, queueEventOnly: true);
        }

        private static void keyDown(KeyCode keyCode, float power, ButtonControl buttonControl)
        {
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(power, buttonControl);
            else
                InputTestFixture.Press(buttonControl);

        }

        private static void keyUp(KeyCode keyCode, ButtonControl buttonControl, bool queueEventOnly = false)
        {
            if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
                setStick(0, buttonControl);
            else
                InputTestFixture.Release(buttonControl, queueEventOnly: queueEventOnly);

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