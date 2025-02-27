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
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Driver.Communication;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.Notifications;

namespace AltTester.AltTesterUnitySDK.Driver
{
    public class AltDriver: AltDriverUnity
    {
        
        public AltDriver(string host = "127.0.0.1", int port = 13000, string appName = "__default__", bool enableLogging = false, int connectTimeout = 60, string platform = "unknown", string platformVersion = "unknown", string deviceInstanceId = "unknown", string appId = "unknown", string driverType = "SDK")
            : base(host, port, appName, enableLogging, connectTimeout, platform, platformVersion, deviceInstanceId, appId, driverType)
        {            
        }

        public List<AltObject> FindObjects(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return base.FindObjects(new AltBy(by, value), new AltBy(cameraBy, cameraValue), enabled);
        }

        public List<AltObject> FindObjectsWhichContain(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return base.FindObjectsWhichContain(new AltBy(by, value), new AltBy(cameraBy, cameraValue), enabled);
        }

        public AltObject FindObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return base.FindObject(new AltBy(by, value), new AltBy(cameraBy, cameraValue), enabled);
        }

        public AltObject FindObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return base.FindObjectWhichContains(new AltBy(by, value), new AltBy(cameraBy, cameraValue), enabled);
        }

        public List<AltObject> GetAllElements(By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return base.GetAllElements(new AltBy(cameraBy, cameraValue), enabled);
        }

        public List<AltObjectLight> GetAllElementsLight(By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return base.GetAllElementsLight(new AltBy(cameraBy, cameraValue), enabled);
        }

        public AltObject WaitForObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return base.WaitForObject(new AltBy(by, value), new AltBy(cameraBy, cameraValue), enabled, timeout, interval);
        }

        public void WaitForObjectNotBePresent(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            base.WaitForObjectNotBePresent(new AltBy(by, value), new AltBy(cameraBy, cameraValue), enabled, timeout, interval);
        }

        public AltObject WaitForObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return base.WaitForObjectWhichContains(new AltBy(by, value), new AltBy(cameraBy, cameraValue), enabled, timeout, interval);
        }
    }
}
