/*
    Copyright(C) 2026 Altom Consulting
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
        public bool UsingLocation = false;

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
                hideGreenPopup = HideGreenPopup,
            };
        }
        public bool KeepAUTSymbolDefined = true;
        public bool KeepAltTesterPrefabInScene = false;
        public bool CreatedPrefab { get; internal set; }
    }
}
