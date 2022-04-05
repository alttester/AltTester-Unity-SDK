using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Altom.AltUnityTester;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class NewInputSystem : MonoBehaviour
{
    public static InputTestFixture InputTestFixture = new InputTestFixture();
    public static NewInputSystem Instance;
    public static Keyboard Keyboard;
    public static Mouse Mouse;
    public static Gamepad Gamepad;
    public static Touchscreen Touchscreen;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        InputTestFixture = new InputTestFixture();
        InputTestFixture.Setup();
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

    }
    internal static IEnumerator ScrollLifeCycle(float speed, float duration)
    {
        float currentTime = 0;
        float frameTime = 0;// using this because of a bug with yield return which waits only every other iteration
        while (currentTime <= duration - Time.fixedUnscaledDeltaTime)
        {
            InputTestFixture.Move(Mouse.current.scroll, new Vector2(speed * frameTime / duration, speed * frameTime / duration));
            var initialTime = Time.fixedUnscaledTime;
            yield return null;
            var afterTime = Time.fixedUnscaledTime;
            frameTime = afterTime - initialTime;
            currentTime += frameTime;
        }
        InputTestFixture.Set(Mouse.current.scroll, new Vector2(0, 0));
    }

    internal static void KeyDownLifeCycle(KeyCode keyCode, float power)
    {
        ButtonControl buttonControl = keyCodeToButtonControl(keyCode, power);
        if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
        {
            if (buttonControl == Gamepad.current.leftStick.up)
                InputTestFixture.Set(Gamepad.current.leftStick.y, power);
            else if (buttonControl == Gamepad.current.leftStick.down)
                InputTestFixture.Set(Gamepad.current.leftStick.y, power);
            else if (buttonControl == Gamepad.current.leftStick.right)
                InputTestFixture.Set(Gamepad.current.leftStick.x, power);
            else if (buttonControl == Gamepad.current.leftStick.left)
                InputTestFixture.Set(Gamepad.current.leftStick.x, power);
            else if (buttonControl == Gamepad.current.rightStick.up)
                InputTestFixture.Set(Gamepad.current.rightStick.y, power);
            else if (buttonControl == Gamepad.current.rightStick.down)
                InputTestFixture.Set(Gamepad.current.rightStick.y, power);
            else if (buttonControl == Gamepad.current.rightStick.right)
                InputTestFixture.Set(Gamepad.current.rightStick.x, power);
            else if (buttonControl == Gamepad.current.rightStick.left)
                InputTestFixture.Set(Gamepad.current.rightStick.x, power);
        }
        else
            InputTestFixture.Press(buttonControl);
    }

    internal static void KeyUpLifeCycle(KeyCode keyCode)
       => InputTestFixture.Release(keyCodeToButtonControl(keyCode));

    internal static IEnumerator KeyPressLifeCycle(KeyCode keyCode, float power, float duration)
    {
        ButtonControl buttonControl = keyCodeToButtonControl(keyCode, power);
        yield return null;
        if (keyCode >= KeyCode.JoystickButton16 && keyCode <= KeyCode.JoystickButton19)
        {
            if (buttonControl == Gamepad.current.leftStick.up)
                InputTestFixture.Set(Gamepad.current.leftStick.y, power);
            else if (buttonControl == Gamepad.current.leftStick.down)
                InputTestFixture.Set(Gamepad.current.leftStick.y, power);
            else if (buttonControl == Gamepad.current.leftStick.right)
                InputTestFixture.Set(Gamepad.current.leftStick.x, power);
            else if (buttonControl == Gamepad.current.leftStick.left)
                InputTestFixture.Set(Gamepad.current.leftStick.x, power);
            else if (buttonControl == Gamepad.current.rightStick.up)
                InputTestFixture.Set(Gamepad.current.rightStick.y, power);
            else if (buttonControl == Gamepad.current.rightStick.down)
                InputTestFixture.Set(Gamepad.current.rightStick.y, power);
            else if (buttonControl == Gamepad.current.rightStick.right)
                InputTestFixture.Set(Gamepad.current.rightStick.x, power);
            else if (buttonControl == Gamepad.current.rightStick.left)
                InputTestFixture.Set(Gamepad.current.rightStick.x, power);
        }
        else

            InputTestFixture.Press(buttonControl);
        yield return new WaitForSeconds(duration);
        InputTestFixture.Release(buttonControl);
    }

    #region private interface
    private static ButtonControl keyCodeToButtonControl(KeyCode keyCode, float power = 1)
    {
        foreach (var e in AltUnityKeyMapping.StringToKeyCode)
            if (e.Value == keyCode)
                foreach (var e2 in AltUnityKeyMapping.StringToKey)
                    if (e2.Key.Equals(e.Key))
                        return Keyboard.current[e2.Value];
        foreach (var e in AltUnityKeyMapping.mouseKeyCodeToButtonControl)
            if (e.Key == keyCode)
                return e.Value;
        AltUnityKeyMapping altUnityKeyMapping = new AltUnityKeyMapping(power);
        foreach (var e in altUnityKeyMapping.joystickKeyCodeToGamepad)
            if (e.Key == keyCode)
                return e.Value;
        return null;
    }
    #endregion
}



