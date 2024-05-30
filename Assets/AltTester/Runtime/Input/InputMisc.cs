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

using System;
using System.Collections;
using System.Collections.Generic;
using AltTester.AltTesterUnitySDK.Driver;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace AltTester.AltTesterUnitySDK.InputModule
{
    public static class InputMisc
    {
        public static void ActivateCustomInput(bool value)
        {

#if ALTTESTER
#if ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = value;
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
    }

}
