using System.Collections.Generic;
using UnityEngine;


public class EditorConfiguration : ScriptableObject
{

    public bool appendToName;
    public  string OutputPathName = "";
    public  string AdbPath = "/usr/local/bin/adb";
    public string IProxyPath = "/usr/local/bin/iproxy";
    public string XcrunPath="/usr/bin/xcrun";
    public bool AutomaticallySign;
    public string SigningTeamId;
    public int BuildNumber = 0;
    public List<MyTest> MyTests = new List<MyTest>();
    public List<MyScenes> Scenes = new List<MyScenes>();
    public Platform platform = Platform.Editor;
    public bool ranInEditor = false;
    public string requestSeparator = ";";
    public string requestEnding = "&";
    public int serverPort = 13000;

}
