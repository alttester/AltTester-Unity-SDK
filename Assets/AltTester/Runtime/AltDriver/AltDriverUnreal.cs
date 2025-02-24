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
    public class AltDriverUnreal : AltDriverBase
    {
        public AltDriverUnreal(string host = "127.0.0.1", int port = 13000, string appName = "__default__", bool enableLogging = false, int connectTimeout = 60, string platform = "unknown", string platformVersion = "unknown", string deviceInstanceId = "unknown", string appId = "unknown", string driverType = "SDK")
            : base(host, port, appName, enableLogging, connectTimeout, platform, platformVersion, deviceInstanceId, appId, driverType)
        {
        }

        public AltObjectUnreal FindObject(AltBy altBy, bool enabled = true)
        {
            AltObject altObject = base.FindObject(altBy, enabled: enabled);
            return AltObjectUnreal.Create(altObject);
        }

        public AltObjectUnreal FindObjectWhichContains(AltBy altBy, bool enabled = true)
        {
            return AltObjectUnreal.Create(base.FindObjectWhichContains(altBy, enabled: enabled));
        }


        public List<AltObjectUnreal> FindObjects(AltBy altBy, bool enabled = true)
        {
            return base.FindObjects(altBy, enabled: enabled).ConvertAll(obj => AltObjectUnreal.Create(obj));
        }


        public List<AltObjectUnreal> FindObjectsWhichContain(AltBy altBy, bool enabled = true)
        {
            
            return base.FindObjectsWhichContain(altBy, enabled: enabled).ConvertAll(obj => AltObjectUnreal.Create(obj));
        }


        public List<AltObjectUnreal> GetAllElements(bool enabled = true)
        {
            return base.GetAllElements(enabled: enabled).ConvertAll(obj => AltObjectUnreal.Create(obj));
        }

        public List<AltObjectLight> GetAllElementsLight(bool enabled = true)
        {
            return base.GetAllElementsLight(enabled: enabled);
        }


        public AltObjectUnreal WaitForObject(AltBy altBy, bool enabled = true, int timeout = 20, double interval = 0.5)
        {
            return AltObjectUnreal.Create(base.WaitForObject(altBy, enabled: enabled, timeout: timeout, interval: interval));
        }
        
        public void WaitForObjectNotBePresent(AltBy altBy, bool enabled = true, int timeout = 20, double interval = 0.5)
        {
            base.WaitForObjectNotBePresent(altBy, enabled: enabled, timeout: timeout, interval: interval);
        }

        public AltObjectUnreal WaitForObjectWhichContains(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true, int timeout = 20, double interval = 0.5)
        {
            return AltObjectUnreal.Create(base.WaitForObjectWhichContains(altBy, enabled: enabled, timeout: timeout, interval: interval));
        }

        public void KeyDown(AltKeyCode key)
        {
            base.KeyDown(key, power: 1);
        }

        public void KeysDown(AltKeyCode[] keyCodes)
        {
            base.KeysDown(keyCodes, power: 1);
        }

        public void PressKey(AltKeyCode key)
        {
            base.PressKey(key, power: 1);
        }

        public void PressKeys(AltKeyCode[] keyCodes)
        {
            base.PressKeys(keyCodes, power: 1);
        }

        public void LoadLevel(string levelName)
        {
            base.LoadScene(levelName);
        }

        public string GetCurrentLevel()
        {
            return base.GetCurrentScene();
        }

        public void WaitForCurrentLevelToBe(string levelName, double timeout = 10, double interval = 1)
        {
            base.WaitForCurrentSceneToBe(levelName, timeout, interval);
        }

        public float GetGlobalTimeDilation()
        {
            return base.GetTimeScale();
        }

        public void SetGlobalTimeDilation(float timeDilation)
        {
            base.SetTimeScale(timeDilation);
        }

        public T CallStaticMethod<T>(string typeName, string methodName, object[] parameters, string[] typeOfParameters = null)
        {
            return base.CallStaticMethod<T>(typeName, methodName, "", parameters, typeOfParameters);
        }

        public new AltObjectUnreal FindObjectAtCoordinates(AltVector2 screenPosition)
        {
            return AltObjectUnreal.Create(base.FindObjectAtCoordinates(screenPosition));
        }
    }
}