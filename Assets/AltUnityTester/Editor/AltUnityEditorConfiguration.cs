using System.Collections.Generic;
using UnityEngine;

namespace Altom.Editor
{
    public class AltUnityEditorConfiguration : ScriptableObject
    {
        public bool appendToName;
        public string MaxLogLength = "100";
        public string AdbPath = "/usr/local/bin/adb";
        public string IProxyPath = "/usr/local/bin/iproxy";
        public string XcrunPath = "/usr/bin/xcrun";
        public bool AutomaticallySign;
        public string SigningTeamId;
        public int BuildNumber = 0;
        public List<AltUnityMyTest> MyTests = new List<AltUnityMyTest>();
        public List<AltUnityMyScenes> Scenes = new List<AltUnityMyScenes>();
        public AltUnityPlatform platform = AltUnityPlatform.Editor;
        public UnityEditor.BuildTarget StandaloneTarget = UnityEditor.BuildTarget.StandaloneWindows;
        public bool RanInEditor = false;
        public string RequestSeparator = ";";
        public string RequestEnding = "&";
        public int ServerPort = 13000;
        public bool ScenePathDisplayed;
        public bool InputVisualizer;
        public bool ShowPopUp = true;
        public string BuildLocationPath = "";
        public string LatestInspectorVersion = "";

        public bool ShowInsectorPopUpInEditor = false;
    }
}
