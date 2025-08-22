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
using System.Collections;
using System.Collections.Generic;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterUnitySDK.Commands;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
#endif

namespace AltTester.AltTesterUnitySDK.InputModule
{
    public static class InputMisc
    {
        private static float threeFingerHoldTimer = 0f;
        public static void ActivateCustomInput(bool value)
        {

#if ALTTESTER
#if ENABLE_LEGACY_INPUT_MANAGER
            AltInput.UseCustomInput = value;
#endif
#if ENABLE_INPUT_SYSTEM
            if (value)
            {
                NewInputSystem.DisableDefaultDevicesAndEnableAltDevices();
            }
            else
            {
                NewInputSystem.EnableDefaultDevicesAndDisableAltDevices();
            }
#endif
#endif
        }

        public static bool IsResetConnectionShortcutPressed()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.L))
#else
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.leftShiftKey.isPressed && Keyboard.current.dKey.isPressed && Keyboard.current.lKey.isPressed)
#else
#endif
#endif
            {
                return true;
            }
            return false;
        }
        public static Vector3 GetMousePosition()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.mousePosition;
#else
            return UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#endif

        }

        public static bool TogglePopup()
        {
#if ENABLE_INPUT_SYSTEM

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                bool ctrlHeld = keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed;
                bool altHeld = keyboard.leftAltKey.isPressed || keyboard.rightAltKey.isPressed;

                if (ctrlHeld && altHeld && keyboard.tKey.wasPressedThisFrame)
                {
                    return true;
                }
            }

            Touchscreen touchscreen = Touchscreen.current;
            if (touchscreen != null)
            {
                int activeTouchCount = 0;
                foreach (var touch in touchscreen.touches)
                {
                    if (touch.phase.ReadValue() == TouchPhase.Began ||
                        touch.phase.ReadValue() == TouchPhase.Moved ||
                        touch.phase.ReadValue() == TouchPhase.Stationary)
                    {
                        activeTouchCount++;
                    }
                }
                if (activeTouchCount == 3)
                {
                    threeFingerHoldTimer += Time.deltaTime;
                    if (threeFingerHoldTimer >= 1.0f)
                    {
                        threeFingerHoldTimer = 0f;
                        return true;
                    }
                }
                else
                {
                    threeFingerHoldTimer = 0f;
                }

            }

#else

bool ctrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        bool altHeld = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

        if (ctrlHeld && altHeld && Input.GetKeyDown(KeyCode.T))
        {
            return true;
        }

        if (Input.touchCount == 3)
        {
            threeFingerHoldTimer += Time.deltaTime;

            if (threeFingerHoldTimer >= 1.0f)
            {
                threeFingerHoldTimer = 0f;
                return true;
            }
        }
        else
        {
            threeFingerHoldTimer = 0f;
        }
#endif
            return false;
        }
    }
}
