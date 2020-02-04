using System.Collections.Generic;
using UnityEngine;


public class AltUnityEditorConfiguration : ScriptableObject
{

    public bool appendToName;
    public  string OutputPathName = "";
    public  string AdbPath = "/usr/local/bin/adb";
    public string IProxyPath = "/usr/local/bin/iproxy";
    public string XcrunPath="/usr/bin/xcrun";
    public bool AutomaticallySign;
    public string SigningTeamId;
    public int BuildNumber = 0;
    public List<AltUnityMyTest> MyTests = new List<AltUnityMyTest>();
    public List<AltUnityMyScenes> Scenes = new List<AltUnityMyScenes>();
    public AltUnityPlatform platform = AltUnityPlatform.Editor;
    public UnityEditor.BuildTarget standaloneTarget = UnityEditor.BuildTarget.StandaloneWindows;
    public bool ranInEditor = false;
    public string requestSeparator = ";";
    public string requestEnding = "&";
    public int serverPort = 13000;
    public bool scenePathDisplayed;
    public bool inputVisualizer;
    public bool showPopUp = true;
}
