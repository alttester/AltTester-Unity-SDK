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


using System;
using System.Collections.Generic;
using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterUnitySDK.InputModule;

#pragma warning disable IDE1006 
public class Input
{


    #region UnityEngine.Input.AltTester.NotImplemented

    public static bool simulateMouseWithTouches
    {
        get { return UnityEngine.Input.simulateMouseWithTouches; }
        set { UnityEngine.Input.simulateMouseWithTouches = value; }
    }

    public static bool mousePresent => UnityEngine.Input.mousePresent;

    public static bool stylusTouchSupported => UnityEngine.Input.stylusTouchSupported;

    public static bool touchSupported => UnityEngine.Input.touchSupported;

    public static bool multiTouchEnabled
    {
        get { return UnityEngine.Input.multiTouchEnabled; }
        set { UnityEngine.Input.multiTouchEnabled = value; }
    }
    public static UnityEngine.LocationService location => UnityEngine.Input.location;

    public static UnityEngine.Compass compass => UnityEngine.Input.compass;
    public static UnityEngine.DeviceOrientation deviceOrientation => UnityEngine.Input.deviceOrientation;

    public static UnityEngine.IMECompositionMode imeCompositionMode
    {
        get { return UnityEngine.Input.imeCompositionMode; }
        set { UnityEngine.Input.imeCompositionMode = value; }
    }

    public static string compositionString => UnityEngine.Input.compositionString;

    public static bool imeIsSelected => UnityEngine.Input.imeIsSelected;

    public static bool touchPressureSupported => UnityEngine.Input.touchPressureSupported;

    public static UnityEngine.Gyroscope gyro => UnityEngine.Input.gyro;

    public static UnityEngine.Vector2 compositionCursorPos
    {
        get { return UnityEngine.Input.compositionCursorPos; }
        set { UnityEngine.Input.compositionCursorPos = value; }
    }

    public static bool BackButtonLeavesApp
    {
        get { return UnityEngine.Input.backButtonLeavesApp; }
        set { UnityEngine.Input.backButtonLeavesApp = value; }
    }

    [System.Obsolete]
    public static bool isGyroAvailable => UnityEngine.Input.isGyroAvailable;

    public static bool compensateSensors
    {
        get { return UnityEngine.Input.compensateSensors; }
        set { UnityEngine.Input.compensateSensors = value; }
    }

    public static UnityEngine.AccelerationEvent GetAccelerationEvent(int index) => UnityEngine.Input.GetAccelerationEvent(index);

    public static string[] GetJoystickNames() => UnityEngine.Input.GetJoystickNames();

    public static void ResetInputAxes() => UnityEngine.Input.ResetInputAxes();

    #endregion

    #region UnityEngine.Input.AltTester

    public static bool anyKey => AltInput.UseCustomInput ? AltInput.KeyCodesPressed.Count > 0 : UnityEngine.Input.anyKey;

    public static bool anyKeyDown => AltInput.UseCustomInput ? AltInput.KeyCodesPressedDown.Count > 0 : UnityEngine.Input.anyKeyDown;

    public static string inputString
    {
        get
        {
            return UnityEngine.Input.inputString;
            //TODO
            // if (AltInput.UseCustomInput)
            // {
            //     string charactersPressedCurrentFrame = "";
            //     foreach (var keyCode in AltInput.KeyCodesPressedDown)
            //     {
            //         //need a Parser from keycode to character every character from keyboard + backspace and enter
            //     }
            //     return charactersPressedCurrentFrame;

            // }
            // else
            // {

            // }
        }
    }

    public static UnityEngine.Vector3 acceleration
    {
        get => AltInput.UseCustomInput ? AltInput.Acceleration : UnityEngine.Input.acceleration;
        set => AltInput.Acceleration = value;
    }

    public static UnityEngine.AccelerationEvent[] accelerationEvents
    {
        get => AltInput.UseCustomInput ? AltInput.AccelerationEvents : UnityEngine.Input.accelerationEvents;
        set => AltInput.AccelerationEvents = value;
    }

    public static int accelerationEventCount => AltInput.UseCustomInput ? AltInput.AccelerationEvents.Length : UnityEngine.Input.accelerationEventCount;

    public static UnityEngine.Touch[] touches
    {
        get => AltInput.UseCustomInput ? AltInput.Touches : UnityEngine.Input.touches;
        set => AltInput.Touches = value;
    }

    public UnityEngine.Touch this[int i]
    {
        get => AltInput.UseCustomInput ? AltInput.Touches[i] : UnityEngine.Input.GetTouch(i);
        set => AltInput.Touches[i] = value;
    }

    public static int touchCount
    {
        get => AltInput.UseCustomInput ? AltInput.TouchCount : UnityEngine.Input.touchCount;
        set => AltInput.TouchCount = value;
    }

    public static UnityEngine.Vector2 mouseScrollDelta => AltInput.UseCustomInput ? AltInput.MouseScrollDelta : UnityEngine.Input.mouseScrollDelta;

    public static UnityEngine.Vector3 mousePosition
    {
        get => AltInput.UseCustomInput ? AltInput.MousePosition : UnityEngine.Input.mousePosition;
        set => AltInput.MousePosition = value;
    }

    public static float GetAxis(string axisName)
    {
        if (AltInput.UseCustomInput)
        {
            var axis = AltInput.AxisList.FirstOrDefault(axle => axle.name == axisName);
            if (axis == null)
            {
                throw new NotFoundException("No axis with this name was found");
            }
            if (axis.type == InputType.MouseMovement)
            {
                if (axis.axisDirection == 0)
                    return AltInput.MouseDelta.x;
                if (axis.axisDirection == 1)
                    return AltInput.MouseDelta.y;
            }

            foreach (var keyStructure in AltInput.KeyCodesPressed)
            {
                if ((axis.positiveButton != "" && keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.positiveButton)) || (axis.altPositiveButton != "" && keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.altPositiveButton)))
                {
                    return keyStructure.Power;
                }
                if ((axis.negativeButton != "" && keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.negativeButton)) || (axis.altNegativeButton != "" && keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.altNegativeButton)))
                {
                    return -1 * keyStructure.Power;
                }
            }
            return 0;
        }
        else
        {
            return UnityEngine.Input.GetAxis(axisName);
        }
    }

    public static float GetAxisRaw(string axisName)
    {
        if (AltInput.UseCustomInput)
        {
            return GetAxis(axisName);
        }
        else
        {
            return UnityEngine.Input.GetAxisRaw(axisName);
        }
    }

    private static Func<List<KeyStructure>, string, bool> checkButton = (_keyCodes, buttonName) =>
           {
               var axes = AltInput.AxisList.FindAll(axle => axle.name == buttonName);
               if (axes.Count == 0)
               {
                   throw new NotFoundException("No button with this name was found");
               }

               foreach (var keyStructure in _keyCodes)
               {
                   foreach (var axis in axes)
                   {
                       if (keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.positiveButton)
                           || keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.altPositiveButton)
                           || keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.negativeButton)
                           || keyStructure.KeyCode == AltInput.ConvertStringToKeyCode(axis.altNegativeButton))
                       {
                           return true;
                       }
                   }
               }
               return false;
           };

    public static bool GetButton(string buttonName)
    {
        return AltInput.UseCustomInput ? checkButton(AltInput.KeyCodesPressed, buttonName) : UnityEngine.Input.GetButton(buttonName);
    }

    public static bool GetButtonDown(string buttonName)
    {
        return AltInput.UseCustomInput ? checkButton(AltInput.KeyCodesPressedDown, buttonName) : UnityEngine.Input.GetButtonDown(buttonName);
    }

    public static bool GetButtonUp(string buttonName)
    {
        return AltInput.UseCustomInput ? checkButton(AltInput.KeyCodesPressedUp, buttonName) : UnityEngine.Input.GetButtonUp(buttonName);
    }

    public static bool GetKey(string name)
    {
        if (!AltInput.UseCustomInput)
        {
            return UnityEngine.Input.GetKey(name);
        }
        UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
        return 0 != AltInput.KeyCodesPressed.FindAll(key => key.KeyCode == keyCode).Count;
    }

    public static bool GetKey(UnityEngine.KeyCode key) => AltInput.UseCustomInput
            ? 0 != AltInput.KeyCodesPressed.FindAll(keyFromList => keyFromList.KeyCode == key).Count
            : UnityEngine.Input.GetKey(key);

    public static bool GetKeyDown(UnityEngine.KeyCode key) => AltInput.UseCustomInput
            ? 0 != AltInput.KeyCodesPressedDown.FindAll(keyFromList => keyFromList.KeyCode == key).Count
            : UnityEngine.Input.GetKeyDown(key);

    public static bool GetKeyDown(string name)
    {

        if (!AltInput.UseCustomInput)
        {
            return UnityEngine.Input.GetKeyDown(name);
        }
        UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
        return 0 != AltInput.KeyCodesPressedDown.FindAll(key => key.KeyCode == keyCode).Count;
    }

    public static bool GetKeyUp(UnityEngine.KeyCode key) => AltInput.UseCustomInput
            ? 0 != AltInput.KeyCodesPressedUp.FindAll(keyFromList => keyFromList.KeyCode == key).Count
            : UnityEngine.Input.GetKeyUp(key);

    public static bool GetKeyUp(string name)
    {
        if (!AltInput.UseCustomInput)
        {
            return UnityEngine.Input.GetKeyUp(name);
        }
        UnityEngine.KeyCode keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), name);
        return 0 != AltInput.KeyCodesPressedUp.FindAll(key => key.KeyCode == keyCode).Count;
    }

    public static bool GetMouseButton(int button)
    {
        if (!AltInput.UseCustomInput)
        {
            return UnityEngine.Input.GetMouseButton(button);
        }
        var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
        return 0 != AltInput.KeyCodesPressed.FindAll(key => key.KeyCode == keyCode).Count || AltInput.Touches.Length > button;
    }

    public static bool GetMouseButtonDown(int button)
    {
        if (!AltInput.UseCustomInput)
        {
            return UnityEngine.Input.GetMouseButtonDown(button);
        }
        var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
        return 0 != AltInput.KeyCodesPressedDown.FindAll(key => key.KeyCode == keyCode).Count || (AltInput.Touches.Length > button && AltInput.Touches[button].phase == UnityEngine.TouchPhase.Began);
    }

    public static bool GetMouseButtonUp(int button)
    {
        if (!AltInput.UseCustomInput)
        {
            return UnityEngine.Input.GetMouseButtonUp(button);
        }
        var keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), "Mouse" + button);
        return 0 != AltInput.KeyCodesPressedUp.FindAll(key => key.KeyCode == keyCode).Count || AltInput.Touches.Length > button && AltInput.Touches[button].phase == UnityEngine.TouchPhase.Ended;
    }

    public static UnityEngine.Touch GetTouch(int index)
    {
        return AltInput.UseCustomInput ? AltInput.Touches[index] : UnityEngine.Input.GetTouch(index);
    }

    #endregion

}

#pragma warning restore IDE1006 // Naming Styles
