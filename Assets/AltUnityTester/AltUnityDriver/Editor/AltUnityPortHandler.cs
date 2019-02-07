using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AltUnityPortHandler {


    public static int idIproxyProcess;
    public static bool iProxyOn = false;

#if UNITY_EDITOR_OSX
    public static void ThreadForwardIos() {

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = AltUnityTesterEditor.EditorConfiguration.IProxyPath,
            Arguments = "13000 13000"
        };
        process.StartInfo = startInfo;
        process.Start();
        idIproxyProcess = process.Id;
        iProxyOn = true;
        process.WaitForExit();
    }

    public static void KillIProxy(int id) {
        var chosenOne = Process.GetProcessesByName("iproxy");
        chosenOne[0].Kill();
        chosenOne[0].WaitForExit();
    }
#endif

    public static void ForwardAndroid() {
        string adbFileName;
#if UNITY_EDITOR_WIN
        adbFileName = "adb.exe";
#elif UNITY_EDITOR_OSX
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = adbFileName,
            Arguments = "forward tcp:13000 tcp:13000 "
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

    }

    public static void RemoveForwardAndroid() {
        string adbFileName;
#if UNITY_EDITOR_WIN
        adbFileName = "adb.exe";
#elif UNITY_EDITOR_OSX
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif
        var process = new Process();
        var startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = adbFileName,
            Arguments = "forward --remove-all"
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }



}
