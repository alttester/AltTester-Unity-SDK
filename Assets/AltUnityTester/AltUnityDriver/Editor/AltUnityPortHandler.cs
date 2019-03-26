using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class AltUnityPortHandler {


    public static int idIproxyProcess=0;

#if UNITY_EDITOR_OSX
    public static string ForwardIos(string id="",int localPort=13000,int remotePort=13000) {
        var argument=localPort+" "+remotePort+" "+id;
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError=true,
            FileName = AltUnityTesterEditor.EditorConfiguration.IProxyPath,
            Arguments = argument
        };
        process.StartInfo = startInfo;
        process.Start();
        idIproxyProcess = process.Id;

        Thread.Sleep(1000);
        if(process.HasExited){
            return process.StandardError.ReadToEnd();
        }
        
        return "Ok "+process.Id;
               
    }

    public static void KillIProxy(int id) {
        var process=Process.GetProcessById(id);
        if(process!=null){
        process.Kill();
        process.WaitForExit();
        }
    }
#endif

    public static string ForwardAndroid(string deviceId="",int localPort=13000,int remotePort=13000) {
        string adbFileName;
        string argument;
        if (!deviceId.Equals(""))
            argument = "forward tcp:" + localPort + " tcp:" + remotePort;
        else
            argument = "forward -s " + deviceId + " tcp:" + localPort + " tcp:" + remotePort;


#if UNITY_EDITOR_WIN
        adbFileName = "adb.exe";
#elif UNITY_EDITOR
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError=true,
            FileName = adbFileName,
            Arguments = argument
        };
        process.StartInfo = startInfo;
        process.Start();
        string stdout=process.StandardError.ReadToEnd();
        process.WaitForExit();
        if(stdout.Length>0){
            return stdout;
        }
        return "Ok";

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
#elif UNITY_EDITOR
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif
        var process = new Process();
        var startInfo = new ProcessStartInfo {
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError=true,
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
#elif UNITY_EDITOR
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError=true,
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
#elif UNITY_EDITOR
        adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
#endif
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            CreateNoWindow=true,
            WindowStyle = ProcessWindowStyle.Minimized,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError=true,
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
                try{
                var parts = line.Split(' ');
                string deviceId = parts[0];
                int localPort = int.Parse(parts[1].Split(':')[1]);
                int remotePort = int.Parse(parts[2].Split(':')[1]);
                devices.Add(new MyDevices(deviceId, localPort, remotePort,true));
                }catch(FormatException){
                    UnityEngine.Debug.Log("adb forward also has: "+line+" but we did not included in the list");
                }
            }
        }
        process.WaitForExit();
        return devices;
    }
#if UNITY_EDITOR_OSX
     public static List<MyDevices> GetConnectediOSDevices()
    {
    
        var xcrun = "/usr/bin/xcrun";
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            CreateNoWindow=true,
            WindowStyle = ProcessWindowStyle.Minimized,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError=true,
            FileName = xcrun,
            Arguments = "instruments -s devices"
        };
        process.StartInfo = startInfo;
        process.Start();
        List<MyDevices> devices = new List<MyDevices>();
        string line = process.StandardOutput.ReadLine();//Known devices: line
        line = process.StandardOutput.ReadLine();//mac's id
        while (!process.StandardOutput.EndOfStream)
        {
            line = process.StandardOutput.ReadLine();
            if (line.Length > 0 && !line.Contains("(Simulator)"))
            {
                var parts = line.Split('[');
                string deviceId = parts[1].Split(']')[0];
                devices.Add(new MyDevices(deviceId,13000,13000,false,Platform.iOS));
            }
        }
        process.WaitForExit();
        return devices;
    }
#endif



}
