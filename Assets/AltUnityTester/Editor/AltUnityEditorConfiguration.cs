using System.Collections.Generic;
using Altom.AltUnity.Instrumentation;
using UnityEngine;

namespace Altom.Editor
{
    public class AltUnityEditorConfiguration : ScriptableObject
    {
        public bool appendToName;
        public string AdbPath = "/usr/local/bin/adb";
        public List<AltUnityMyTest> MyTests = new List<AltUnityMyTest>();
        public List<AltUnityMyScenes> Scenes = new List<AltUnityMyScenes>();
        public AltUnityPlatform platform = AltUnityPlatform.Editor;
        public UnityEditor.BuildTarget StandaloneTarget = UnityEditor.BuildTarget.StandaloneWindows;
        public bool RanInEditor = false;
        public bool ScenePathDisplayed;
        public bool InputVisualizer;
        public bool ShowPopUp = true;
        public string BuildLocationPath = "";

        public string ProxyHost = "127.0.0.1";
        public int ProxyPort = 13000;

        public AltUnityInstrumentationSettings GetInstrumentationSettings()
        {
            return new AltUnityInstrumentationSettings()
            {
                ShowPopUp = ShowPopUp,
                InputVisualizer = InputVisualizer,
                ProxyHost = ProxyHost,
                ProxyPort = ProxyPort,
            };
        }
        public bool KeepAUTSymbolDefined = false;
    }
}
