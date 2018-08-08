using System.Collections.Generic;
using UnityEngine;


public class EditorConfiguration : ScriptableObject
    {
        public string BundleIdentifier = "fi.altom.altunitytester";
        public string ProductName = "sampleGame";
        public string CompanyName = "Altom";
        public static string OutputFolder = "";
        public  string AdbPathIos = "/usr/local/bin/adb";
    public string IProxyPath = "/usr/local/bin/iproxy";
        public bool AutomaticallySign;
        public string SigningTeamId;
        public int BuildNumber = 0;
        public List<MyTest> MyTests = new List<MyTest>();
        public List<MyScenes> Scenes = new List<MyScenes>();
    public bool TestAndroid = true;



    public string OutPutFileNameAndroidDefault()
        {
            return OutputFolder + ProductName + ".apk";
        }
        public string OutputFilenameiOSdDefault()
        {
            return OutputFolder + ProductName;
        }
    }

