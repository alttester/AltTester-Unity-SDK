using System.Collections.Generic;
using Altom.AltTester;
using UnityEngine;

namespace Altom.AltTesterEditor
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
        public bool InputVisualizer;
        public bool ShowPopUp = true;
        public string BuildLocationPath = "";
        public string LatestDesktopVersion = "";
        public bool ShowDesktopPopUpInEditor = false;
        public bool createXMLReport = false;
        public string xMLFilePath = "";

        public int AltTesterPort = 13000;


        public AltInstrumentationSettings GetInstrumentationSettings()
        {
            return new AltInstrumentationSettings()
            {
                ShowPopUp = ShowPopUp,
                InputVisualizer = InputVisualizer,
                AltTesterPort = AltTesterPort,
            };
        }
        public bool KeepAUTSymbolDefined = false;
    }
}
