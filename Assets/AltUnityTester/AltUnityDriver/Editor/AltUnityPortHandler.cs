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

    public static void ForwardAndroid(string deviceId="",int localPort=13000,int remotePort=13000) {
        string adbFileName;
        string argument;
        if (!deviceId.Equals(""))
            argument = "forward tcp:" + localPort + " tcp:" + remotePort;
        else
            argument = "forward -s " + deviceId + " tcp:" + localPort + " tcp:" + remotePort;


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
            Arguments = argument
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

    }

    public static void RemoveForwardAndroid(int localPort=-1) {
        string argument;
        if (localPort == -1)
        {
            argument = "forward --remove-all";
        }
        else
        {
            argument = "forward --remove tcp:" + localPort;
        }
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
            Arguments = argument
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }


    public static List<MyDevices> GetDevicesAndroid()
    {
       
        string adbFileName;
#if UNITY_EDITOR_WIN
        adbFileName = "adb.exe";
#elif UNITY_EDITOR_OSX
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = adbFileName,
            Arguments = "devices"
        };
        process.StartInfo = startInfo;
        process.Start();
        List<MyDevices> devices = new List<MyDevices>();
        while (!process.StandardOutput.EndOfStream)
        {
            string line = process.StandardOutput.ReadLine();
            if (line.Length > 0 && !line.StartsWith("List "))
            {
                var parts = line.Split('\t');
                string deviceId = parts[0];                
                devices.Add(new MyDevices(deviceId));
            }
        }
        process.WaitForExit();
        return devices;
    }
    public static List<MyDevices> GetForwardedDevicesAndroid()
    {
        string adbFileName;
#if UNITY_EDITOR_WIN
        adbFileName = "adb.exe";
#elif UNITY_EDITOR_OSX
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            CreateNoWindow=true,
            WindowStyle = ProcessWindowStyle.Minimized,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            FileName = adbFileName,
            Arguments = "forward --list"
        };
        process.StartInfo = startInfo;
        process.Start();
        List<MyDevices> devices = new List<MyDevices>();
        while (!process.StandardOutput.EndOfStream)
        {
            string line = process.StandardOutput.ReadLine();
            if (line.Length > 0)
            {
                var parts = line.Split(' ');
                string deviceId = parts[0];
                int localPort = int.Parse(parts[1].Split(':')[1]);
                int remotePort = int.Parse(parts[2].Split(':')[1]);
                devices.Add(new MyDevices(deviceId, localPort, remotePort,true));
            }
        }
        process.WaitForExit();
        return devices;
    }



}
