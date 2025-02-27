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
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Driver
{
    public class AltDriverUnity : AltDriverBase
    {
        public AltDriverUnity(string host = "127.0.0.1", int port = 13000, string appName = "__default__", bool enableLogging = false, int connectTimeout = 60, string platform = "unknown", string platformVersion = "unknown", string deviceInstanceId = "unknown", string appId = "unknown", string driverType = "SDK")
            : base(host, port, appName, enableLogging, connectTimeout, platform, platformVersion, deviceInstanceId, appId, driverType)
        {
        }

        public new void ResetInput()
        {
            base.ResetInput();
        }
        public new void LoadScene(string scene, bool loadSingle = true)
        {
            base.LoadScene(scene, loadSingle);
        }

        public new void UnloadScene(string scene)
        {
            base.UnloadScene(scene);
        }

        public new List<string> GetAllLoadedScenes()
        {
            return base.GetAllLoadedScenes();
        }

        public new string GetCurrentScene()
        {
            return base.GetCurrentScene();
        }

        public new void WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
        {
            base.WaitForCurrentSceneToBe(sceneName, timeout, interval);
        }

        public new void SetTimeScale(float timeScale)
        {
           base.SetTimeScale(timeScale);
        }

        public new float GetTimeScale()
        {
            return base.GetTimeScale();
        }


        public new void DeletePlayerPref()
        {
            base.DeletePlayerPref();
        }

        public new void DeleteKeyPlayerPref(string keyName)
        {
            base.DeleteKeyPlayerPref(keyName);
        }

        public new void SetKeyPlayerPref(string keyName, int valueName)
        {
            base.SetKeyPlayerPref(keyName, valueName);
        }

        public new void SetKeyPlayerPref(string keyName, float valueName)
        {
            base.SetKeyPlayerPref(keyName, valueName);
        }

        public new void SetKeyPlayerPref(string keyName, string valueName)
        {
            base.SetKeyPlayerPref(keyName, valueName);
        }

        public new int GetIntKeyPlayerPref(string keyName)
        {
            return base.GetIntKeyPlayerPref(keyName);
        }

        public new float GetFloatKeyPlayerPref(string keyName)
        {
            return base.GetFloatKeyPlayerPref(keyName);
        }

        public new string GetStringKeyPlayerPref(string keyName)
        {
           return base.GetStringKeyPlayerPref(keyName);
        }

        public new List<AltObject> FindObjects(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            return base.FindObjects(altBy, cameraAltBy, enabled);
        }

        public new List<AltObject> FindObjectsWhichContain(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            return base.FindObjectsWhichContain(altBy, cameraAltBy, enabled);
        }

        public new AltObject FindObject(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            return base.FindObject(altBy, cameraAltBy, enabled);
        }

        public new AltObject FindObjectWhichContains(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            return base.FindObjectWhichContains(altBy, cameraAltBy, enabled);
        }

        public new T CallStaticMethod<T>(string typeName, string methodName, string assemblyName,
                    object[] parameters, string[] typeOfParameters = null)
        {
            return base.CallStaticMethod<T>(typeName, methodName, assemblyName, parameters, typeOfParameters);
        }

        public new T GetStaticProperty<T>(string componentName, string propertyName, string assemblyName, int maxDepth = 2)
        {
            return base.GetStaticProperty<T>(componentName, propertyName, assemblyName, maxDepth);
        }

        public new void SetStaticProperty(string componentName, string propertyName, string assemblyName, object updatedProperty)
        {
            base.SetStaticProperty(componentName, propertyName, assemblyName, updatedProperty);
        }

        /// <summary>
        /// Simulates a swipe action between two points.
        /// </summary>
        /// <param name="start">Coordinates of the screen where the swipe begins</param>
        /// <param name="end">Coordinates of the screen where the swipe ends</param>
        /// <param name="duration">The time measured in seconds to move the mouse from start to end location. Defaults to <c>0.1</c>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void Swipe(AltVector2 start, AltVector2 end, float duration = 0.1f, bool wait = true)
        {
            base.Swipe(start, end, duration, wait);
        }

        /// <summary>
        /// Simulates a multipoint swipe action.
        /// </summary>
        /// <param name="positions">A list of positions on the screen where the swipe be made.</param>
        /// <param name="duration">The time measured in seconds to swipe from first position to the last position. Defaults to <code>0.1</code>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void MultipointSwipe(AltVector2[] positions, float duration = 0.1f, bool wait = true)
        {
           base.MultipointSwipe(positions, duration, wait);
        }

        /// <summary>
        /// Simulates holding left click button down for a specified amount of time at given coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates where the button is held down.</param>
        /// <param name="duration">The time measured in seconds to keep the button down.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void HoldButton(AltVector2 coordinates, float duration, bool wait = true)
        {
            base.HoldButton(coordinates, duration, wait);
        }

        /// <summary>
        /// Simulates key press action in your app.
        /// </summary>
        /// <param name="keyCode">The key code of the key simulated to be pressed.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void PressKey(AltKeyCode keyCode, float power = 1, float duration = 0.1f, bool wait = true)
        {
            base.PressKey(keyCode, power, duration, wait);
        }

        /// <summary>
        /// Simulates multiple keys pressed action in your app.
        /// </summary>
        /// <param name="keyCodes">The list of key codes of the keys simulated to be pressed.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void PressKeys(AltKeyCode[] keyCodes, float power = 1, float duration = 0.1f, bool wait = true)
        {
            base.PressKeys(keyCodes, power, duration, wait);
        }

        public new void KeyDown(AltKeyCode keyCode, float power = 1)
        {
            base.KeyDown(keyCode, power);
        }

        /// <summary>
        /// Simulates multiple keys down action in your app.
        /// </summary>
        /// <param name="keyCodes">The key codes of the keys simulated to be down.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        public new void KeysDown(AltKeyCode[] keyCodes, float power = 1)
        {
            base.KeysDown(keyCodes, power);
        }


        /// <summary>
        /// Simulate mouse movement in your app.
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="duration">The time measured in seconds to move the mouse from the current mouse position to the set coordinates. Defaults to <c>0.1f</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void MoveMouse(AltVector2 coordinates, float duration = 0.1f, bool wait = true)
        {
            base.MoveMouse(coordinates, duration, wait);
        }

        /// <summary>
        /// Simulate scroll action in your app.
        /// </summary>
        /// <param name="speed">Set how fast to scroll. Positive values will scroll up and negative values will scroll down. Defaults to <code> 1 </code></param>
        /// <param name="duration">The duration of the scroll in seconds. Defaults to <code> 0.1 </code></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void Scroll(float speed = 1, float duration = 0.1f, bool wait = true)
        {
            base.Scroll(speed, duration, wait);
        }

        /// <summary>
        /// Simulate scroll action in your app.
        /// </summary>
        /// <param name="scrollValue">Set how fast to scroll. X is horizontal and Y is vertical. Defaults to <code> 1 </code></param>
        /// <param name="duration">The duration of the scroll in seconds. Defaults to <code> 0.1 </code></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void Scroll(AltVector2 scrollValue, float duration = 0.1f, bool wait = true)
        {
            base.Scroll(scrollValue, duration, wait);
        }

        /// <summary>
        /// Tap at screen coordinates
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval between taps in seconds</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void Tap(AltVector2 coordinates, int count = 1, float interval = 0.1f, bool wait = true)
        {
            base.Tap(coordinates, count, interval, wait);
        }

        /// <summary>
        /// Simulates device rotation action in your app.
        /// </summary>
        /// <param name="acceleration">The linear acceleration of a device.</param>
        /// <param name="duration">How long the rotation will take in seconds. Defaults to <code>0.1<code>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public new void Tilt(AltVector3 acceleration, float duration = 0.1f, bool wait = true)
        {
            base.Tilt(acceleration, duration, wait);
        }

        public new List<AltObject> GetAllElements(AltBy cameraAltBy = null, bool enabled = true)
        {
            return base.GetAllElements(cameraAltBy, enabled);
        }

        public new List<AltObjectLight> GetAllElementsLight(AltBy cameraAltBy = null, bool enabled = true)
        {
            return base.GetAllElementsLight(cameraAltBy, enabled);
        }

        public new AltObject WaitForObject(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return base.WaitForObject(altBy, cameraAltBy, enabled, timeout, interval);
        }

        public new void WaitForObjectNotBePresent(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            base.WaitForObjectNotBePresent(altBy, cameraAltBy, enabled, timeout, interval);
        }

        public new AltObject WaitForObjectWhichContains(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return base.WaitForObjectWhichContains(altBy, cameraAltBy, enabled, timeout, interval);
        }

        public new List<string> GetAllScenes()
        {
            return base.GetAllScenes();
        }

        public new List<AltObject> GetAllCameras()
        {
            return base.GetAllCameras();
        }

        public new List<AltObject> GetAllActiveCameras()
        {
            return base.GetAllActiveCameras();
        }
        public new AltVector2 GetApplicationScreenSize()
        {
            return base.GetApplicationScreenSize();
        }


        public new int BeginTouch(AltVector2 screenPosition)
        {
           return base.BeginTouch(screenPosition);
        }

        public new void MoveTouch(int fingerId, AltVector2 screenPosition)
        {
            base.MoveTouch(fingerId, screenPosition);
        }

        public new void EndTouch(int fingerId)
        {
            base.EndTouch(fingerId);
        }

        public new AltObject FindObjectAtCoordinates(AltVector2 screenPosition)
        {
            return base.FindObjectAtCoordinates(screenPosition);
        }
    }
}