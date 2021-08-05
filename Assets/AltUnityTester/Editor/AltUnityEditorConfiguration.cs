using System.Collections.Generic;
using Altom.AltUnity.Instrumentation;
using UnityEngine;

namespace Altom.Editor
{
    public class AltUnityEditorConfiguration : ScriptableObject
    {
        public bool appendToName;
        public string AdbPath = "/usr/local/bin/adb";
        public string IProxyPath = "/usr/local/bin/iproxy";
        public string XcrunPath = "/usr/bin/xcrun";
        public int BuildNumber = 0;
        public List<AltUnityMyTest> MyTests = new List<AltUnityMyTest>();
        public List<AltUnityMyScenes> Scenes = new List<AltUnityMyScenes>();
        public AltUnityPlatform platform = AltUnityPlatform.Editor;
        public UnityEditor.BuildTarget StandaloneTarget = UnityEditor.BuildTarget.StandaloneWindows;
        public bool RanInEditor = false;
        public int ServerPort = 13000;
        public bool ScenePathDisplayed;
        public bool InputVisualizer;
        public bool ShowPopUp = true;
        public string BuildLocationPath = "";
        public string LatestInspectorVersion = "";

        public AltUnityInstrumentationMode InstrumentationMode = AltUnityInstrumentationMode.Server;
        public string ProxyHost = "localhost";
        public int ProxyPort = 13000;

        public AltUnityInstrumentationSettings GetInstrumentationSettings()
        {
            return new AltUnityInstrumentationSettings()
            {
                ServerPort = ServerPort,
                ShowPopUp = ShowPopUp,
                InputVisualizer = InputVisualizer,
                InstrumentationMode = InstrumentationMode,
                ProxyHost = ProxyHost,
                ProxyPort = ProxyPort,
            };
        }
    }
}
