using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    private static readonly KeyCode[] mouseKeyCodes = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2 };
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            generateButtonControlLists();
            mapKeyCodeToKey();
        }
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
    private static Dictionary<KeyCode, ButtonControl> letters, f, alphaDigits, keypadNumpads, specialKeysDict;
    private static List<KeyCode> keyCodeLetters, keyCodeF, alphas, keypads, specialKeys;
    private static List<ButtonControl> buttonControlLetters, buttonControlF, digits, numpads, specialButtonControls;

    private static void generateButtonControlLists()
    {
        (letters, f, alphaDigits, keypadNumpads, specialKeysDict) = (new Dictionary<KeyCode, ButtonControl>(), new Dictionary<KeyCode, ButtonControl>(), new Dictionary<KeyCode, ButtonControl>(), new Dictionary<KeyCode, ButtonControl>(), new Dictionary<KeyCode, ButtonControl>());
        (keyCodeLetters, keyCodeF, alphas, keypads, specialKeys) = (new List<KeyCode>() { }, new List<KeyCode>() { }, new List<KeyCode>() { }, new List<KeyCode>() { }, new List<KeyCode>() { });
        for (KeyCode keyCode = KeyCode.A; keyCode <= KeyCode.Z; keyCode++)
            keyCodeLetters.Add(keyCode);
        buttonControlLetters = new List<ButtonControl>() { Keyboard.current.aKey, Keyboard.current.bKey, Keyboard.current.cKey, Keyboard.current.dKey, Keyboard.current.eKey, Keyboard.current.fKey, Keyboard.current.gKey, Keyboard.current.hKey, Keyboard.current.iKey, Keyboard.current.jKey, Keyboard.current.kKey, Keyboard.current.lKey, Keyboard.current.mKey, Keyboard.current.nKey, Keyboard.current.oKey, Keyboard.current.pKey, Keyboard.current.qKey, Keyboard.current.rKey, Keyboard.current.sKey, Keyboard.current.tKey, Keyboard.current.uKey, Keyboard.current.vKey, Keyboard.current.wKey, Keyboard.current.xKey, Keyboard.current.yKey, Keyboard.current.zKey };
        for (KeyCode keyCode = KeyCode.F1; keyCode <= KeyCode.F12; keyCode++)
            keyCodeF.Add(keyCode);
        buttonControlF = new List<ButtonControl>() { Keyboard.current.f1Key, Keyboard.current.f2Key, Keyboard.current.f3Key, Keyboard.current.f4Key, Keyboard.current.f5Key, Keyboard.current.f6Key, Keyboard.current.f7Key, Keyboard.current.f8Key, Keyboard.current.f9Key, Keyboard.current.f10Key, Keyboard.current.f11Key, Keyboard.current.f12Key };
        for (KeyCode keyCode = KeyCode.Alpha0; keyCode <= KeyCode.Alpha9; keyCode++)
            alphas.Add(keyCode);
        digits = new List<ButtonControl>() { Keyboard.current.digit0Key, Keyboard.current.digit1Key, Keyboard.current.digit2Key, Keyboard.current.digit3Key, Keyboard.current.digit4Key, Keyboard.current.digit5Key, Keyboard.current.digit6Key, Keyboard.current.digit7Key, Keyboard.current.digit8Key, Keyboard.current.digit9Key };
        for (KeyCode keyCode = KeyCode.Keypad0; keyCode <= KeyCode.KeypadEquals; keyCode++)
            keypads.Add(keyCode);
        numpads = new List<ButtonControl>() { Keyboard.current.numpad0Key, Keyboard.current.numpad1Key, Keyboard.current.numpad2Key, Keyboard.current.numpad3Key, Keyboard.current.numpad4Key, Keyboard.current.numpad5Key, Keyboard.current.numpad6Key, Keyboard.current.numpad7Key, Keyboard.current.numpad8Key, Keyboard.current.numpad9Key, Keyboard.current.numpadPeriodKey, Keyboard.current.numpadDivideKey, Keyboard.current.numpadMultiplyKey, Keyboard.current.numpadMinusKey, Keyboard.current.numpadPlusKey, Keyboard.current.numpadEnterKey, Keyboard.current.numpadEqualsKey };
        for (KeyCode keyCode = KeyCode.Backspace; keyCode <= KeyCode.Slash; keyCode++)
            if (Enum.IsDefined(typeof(KeyCode), keyCode))
                specialKeys.Add(keyCode);
        for (KeyCode keyCode = KeyCode.Colon; keyCode <= KeyCode.BackQuote; keyCode++)
            if (Enum.IsDefined(typeof(KeyCode), keyCode))
                specialKeys.Add(keyCode);

        for (KeyCode keyCode = KeyCode.LeftCurlyBracket; keyCode <= KeyCode.Delete; keyCode++)
            if (Enum.IsDefined(typeof(KeyCode), keyCode))
                specialKeys.Add(keyCode);

        for (KeyCode keyCode = KeyCode.UpArrow; keyCode <= KeyCode.PageDown; keyCode++)
            specialKeys.Add(keyCode);
        for (KeyCode keyCode = KeyCode.Numlock; keyCode <= KeyCode.Menu; keyCode++)
            if (Enum.IsDefined(typeof(KeyCode), keyCode))
                specialKeys.Add(keyCode);
        specialButtonControls = new List<ButtonControl>() {Keyboard.current.backspaceKey, Keyboard.current.tabKey, Keyboard.current.deleteKey, Keyboard.current.enterKey, Keyboard.current.pauseKey, Keyboard.current.escapeKey, Keyboard.current.spaceKey, Keyboard.current.digit1Key, Keyboard.current.quoteKey, Keyboard.current.digit3Key, Keyboard.current.digit4Key, Keyboard.current.digit5Key, Keyboard.current.digit7Key, Keyboard.current.quoteKey, Keyboard.current.digit9Key, Keyboard.current.digit0Key, Keyboard.current.digit8Key, Keyboard.current.numpadPlusKey, Keyboard.current.commaKey, Keyboard.current.minusKey, Keyboard.current.periodKey, Keyboard.current.slashKey,
            Keyboard.current.semicolonKey, Keyboard.current.semicolonKey, Keyboard.current.commaKey, Keyboard.current.numpadPlusKey, Keyboard.current.periodKey, Keyboard.current.slashKey, Keyboard.current.digit2Key, Keyboard.current.leftBracketKey, Keyboard.current.backslashKey, Keyboard.current.rightBracketKey, Keyboard.current.digit6Key, Keyboard.current.minusKey, Keyboard.current.backquoteKey,
            Keyboard.current.leftBracketKey, Keyboard.current.backslashKey, Keyboard.current.rightBracketKey, Keyboard.current.backquoteKey, Keyboard.current.deleteKey,
            Keyboard.current.upArrowKey, Keyboard.current.downArrowKey, Keyboard.current.rightArrowKey, Keyboard.current.leftArrowKey, Keyboard.current.insertKey, Keyboard.current.homeKey, Keyboard.current.endKey, Keyboard.current.pageUpKey, Keyboard.current.pageDownKey,
            Keyboard.current.numLockKey, Keyboard.current.capsLockKey, Keyboard.current.scrollLockKey, Keyboard.current.rightShiftKey, Keyboard.current.leftShiftKey, Keyboard.current.rightCtrlKey, Keyboard.current.leftCtrlKey, Keyboard.current.rightAltKey, Keyboard.current.leftAltKey, Keyboard.current.rightCommandKey, Keyboard.current.leftCommandKey, Keyboard.current.leftWindowsKey, Keyboard.current.rightWindowsKey, Keyboard.current.rightAltKey, Keyboard.current.f1Key, Keyboard.current.printScreenKey, Keyboard.current.printScreenKey, Keyboard.current.deleteKey, Keyboard.current.contextMenuKey};
    }

    private static void mapKeyCodeToKey()
    {
        for (int i = 0; i < keyCodeLetters.Count; i++)
            letters.Add(keyCodeLetters[i], buttonControlLetters[i]);
        for (int i = 0; i < keyCodeF.Count; i++)
            f.Add(keyCodeF[i], buttonControlF[i]);
        for (int i = 0; i < alphas.Count; i++)
            alphaDigits.Add(alphas[i], digits[i]);
        for (int i = 0; i < keypads.Count; i++)
            keypadNumpads.Add(keypads[i], numpads[i]);
        for (int i = 0; i < specialKeys.Count; i++)
            specialKeysDict.Add(specialKeys[i], specialButtonControls[i]);
    }
    private static ButtonControl keyCodeToButtonControl(KeyCode keyCode)
    {
        ButtonControl buttonControl = null;
        if (keyCodeLetters.Contains(keyCode))
            buttonControl = letters[keyCode];
        else if (keyCodeF.Contains(keyCode))
            buttonControl = f[keyCode];
        else if (alphas.Contains(keyCode))
            buttonControl = alphaDigits[keyCode];
        else if (keypads.Contains(keyCode))
            buttonControl = keypadNumpads[keyCode];
        else if (specialKeys.Contains(keyCode))
            buttonControl = specialKeysDict[keyCode];
        return buttonControl;
    }

    internal static void KeyDownLifeCycle(KeyCode keyCode, float power)
       => InputTestFixture.Press(keyCodeToButtonControl(keyCode));

    internal static void KeyUpLifeCycle(KeyCode keyCode)
       => InputTestFixture.Release(keyCodeToButtonControl(keyCode));

    internal static IEnumerator KeyPressLifeCycle(KeyCode keyCode, float power, float duration)
    {
        ButtonControl buttonControl = keyCodeToButtonControl(keyCode);
        InputTestFixture.Press(buttonControl);
        yield return new WaitForSeconds(duration);
        InputTestFixture.Release(buttonControl);
    }

}



