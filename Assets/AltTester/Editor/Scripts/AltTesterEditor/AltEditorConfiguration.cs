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
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterUnitySDK;
using AltTester.AltTesterUnitySDK.Editor.Platform;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Editor
{
    public class AltEditorConfiguration : ScriptableObject
    {
        public bool appendToName;
        public string AdbPath = "/usr/local/bin/adb";
        public string IProxyPath = "/usr/local/bin/iproxy";
        public string XcrunPath = "/usr/bin/xcrun";
        public List<AltMyTest> MyTests = new List<AltMyTest>();
        public List<AltMyScenes> Scenes = new List<AltMyScenes>();
        public AltPlatform platform = AltPlatform.Standalone;
        public UnityEditor.BuildTarget StandaloneTarget = UnityEditor.BuildTarget.NoTarget;
        public bool RanInEditor = false;
        public bool ScenePathDisplayed;
        public string BuildLocationPath = "";
        public bool createXMLReport = false;
        public string xMLFilePath = "";

        public int AltServerPort = 13000;
        public string AltServerHost = "127.0.0.1";

        public string AppName = "__default__";
        public int assemblyTestDisplayedIndex;
        public bool ResetConnectionData = false;
        public string UID = "";

        public bool HideGreenPopup = false;

        public AltInstrumentationSettings GetInstrumentationSettings()
        {
            return new AltInstrumentationSettings()
            {
                AltServerPort = AltServerPort,
                AltServerHost = AltServerHost,
                AppName = AppName,
                ResetConnectionData = ResetConnectionData,
                UID = SystemInfo.deviceUniqueIdentifier.ToString() + DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                hideGreenPopup = HideGreenPopup

            };
        }
        public bool KeepAUTSymbolDefined = true;
    }
}
