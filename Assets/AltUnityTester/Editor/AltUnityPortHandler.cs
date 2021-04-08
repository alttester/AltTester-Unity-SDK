using System;
using System.Linq;
using Altom.Editor.Logging;
using NLog;

namespace Altom.Editor
{
    public class AltUnityPortHandler
    {
        private static readonly Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        public static int idIproxyProcess = 0;

#if UNITY_EDITOR_OSX
        [Obsolete("Use Altom.AltUnityDriver.AltUnityPortForwarding.ForwardIos")]
        public static string ForwardIos(string id = "", int localPort = 13000, int remotePort = 13000)
        {
            var argument = localPort + " " + remotePort + " " + id;
            try
            {
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = AltUnityTesterEditor.EditorConfiguration.IProxyPath,
                    Arguments = argument
                };
                process.StartInfo = startInfo;
                process.Start();
                idIproxyProcess = process.Id;

                System.Threading.Thread.Sleep(1000);
                if (process.HasExited)
                {
                    return process.StandardError.ReadToEnd();
                }
                return "Ok " + process.Id;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return "The path to Iproxy is not correct or Iproxy is not installed";
            }
        }

        public static void KillIProxy(int id)
        {
            var process = System.Diagnostics.Process.GetProcessById(id);
            if (process != null)
            {
                process.Kill();
                process.WaitForExit();
            }
        }
#endif

        [Obsolete("Use Altom.AltUnityDriver.AltUnityPortForwarding.ForwardAndroid")]
        public static string ForwardAndroid(string deviceId = "", int localPort = 13000, int remotePort = 13000)
        {
            string adbFileName;
            string argument;
            if (deviceId.Equals(""))
                argument = "forward tcp:" + localPort + " tcp:" + remotePort;
            else
            {
                argument = "-s " + deviceId + " forward" + " tcp:" + localPort + " tcp:" + remotePort;
            }

            adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = adbFileName,
                Arguments = argument
            };
            process.StartInfo = startInfo;
            process.Start();
            string stdout = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (stdout.Length > 0)
            {
                return stdout;
            }
            return "Ok";

        }

        [Obsolete("Use Altom.AltUnityDriver.AltUnityPortForwarding.RemoveForwardAndroid")]
        public static void RemoveForwardAndroid(int localPort = -1, string deviceId = "")
        {
            string argument;
            if (localPort == -1)
            {
                argument = "forward --remove-all";
            }
            else
            {
                argument = "-s " + deviceId + " forward --remove tcp:" + localPort;
            }
            string adbFileName;
            adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = adbFileName,
                Arguments = argument
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }


        [Obsolete("Use Altom.AltUnityDriver.AltUnityPortForwarding.GetDevicesAndroid")]
        public static System.Collections.Generic.List<AltUnityMyDevices> GetDevicesAndroid()
        {
            var devices = new System.Collections.Generic.List<AltUnityMyDevices>();
            try
            {
                string adbFileName;
                adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    CreateNoWindow = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = adbFileName,
                    Arguments = "devices"
                };
                process.StartInfo = startInfo;
                process.Start();
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line.Length > 0 && !line.StartsWith("List "))
                    {
                        var parts = line.Split('\t');
                        string deviceId = parts[0];
                        devices.Add(new AltUnityMyDevices(deviceId));
                    }
                }
                process.WaitForExit();
                string stdout = process.StandardError.ReadToEnd();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                logger.Warn("The path to adb is not correct or Adb is not installed. If you don't need Android device just ignore this warning");
            }
            return devices;
        }
        [Obsolete("Use Altom.AltUnityDriver.AltUnityPortForwarding.GetForwardedDevicesAndroid")]
        public static System.Collections.Generic.List<AltUnityMyDevices> GetForwardedDevicesAndroid()
        {
            var devices = new System.Collections.Generic.List<AltUnityMyDevices>();
            try
            {

                string adbFileName;
                adbFileName = AltUnityTesterEditor.EditorConfiguration.AdbPath;
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    CreateNoWindow = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = adbFileName,
                    Arguments = "forward --list"
                };
                process.StartInfo = startInfo;
                process.Start();
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line.Length > 0)
                    {
                        try
                        {
                            var parts = line.Split(' ');
                            string deviceId = parts[0];
                            int localPort = int.Parse(parts[1].Split(':')[1]);
                            int remotePort = int.Parse(parts[2].Split(':')[1]);
                            devices.Add(new AltUnityMyDevices(deviceId, localPort, remotePort, true));
                        }
                        catch (FormatException)
                        {
                            logger.Info("adb forward also has: " + line + " but we did not included in the list");
                        }
                    }
                }
                process.WaitForExit();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                logger.Warn("The path to adb is not correct or Adb is not installed. If you don't need Android device just ignore this warning");
            }
            return devices;
        }
#if UNITY_EDITOR_OSX
        [Obsolete("Use Altom.AltUnityDriver.AltUnityPortForwarding.GetConnectediOSDevices")]
        static System.Collections.Generic.List<AltUnityMyDevices> GetConnectediOSDevices()
        {

            System.Collections.Generic.List<AltUnityMyDevices> devices = new System.Collections.Generic.List<AltUnityMyDevices>();
            try
            {

                var xcrun = AltUnityTesterEditor.EditorConfiguration.XcrunPath;
                var process = new System.Diagnostics.Process();
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    CreateNoWindow = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = xcrun,
                    Arguments = "instruments -s devices"
                };
                process.StartInfo = startInfo;
                process.Start();
                string line = process.StandardOutput.ReadLine();//Known devices: line
                line = process.StandardOutput.ReadLine();//mac's id
                while (!process.StandardOutput.EndOfStream)
                {
                    line = process.StandardOutput.ReadLine();
                    if (line.Length > 0 && !line.Contains("(Simulator)"))
                    {
                        var parts = line.Split('[');
                        string deviceId = parts[1].Split(']')[0];
                        devices.Add(new AltUnityMyDevices(deviceId, 13000, 13000, false, AltUnityPlatform.iOS));
                    }
                }
                process.WaitForExit();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                logger.Warn("The path to Xcrun is not correct or Xcrun is not installed. If you don't need iOS device just ignore this warning");
            }
            return devices;
        }

        [Obsolete("Use Altom.AltUnityDriver.AltUnityPortForwarding.GetForwardediOSDevices")]
        public static System.Collections.Generic.List<AltUnityMyDevices> GetForwardediOSDevices()
        {
            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = "ps",
                Arguments = "aux"
            };
            process.StartInfo = startInfo;
            process.Start();
            System.Collections.Generic.List<AltUnityMyDevices> devices = new System.Collections.Generic.List<AltUnityMyDevices>();
            while (!process.StandardOutput.EndOfStream)
            {
                var line2 = process.StandardOutput.ReadLine();//mac's id    
                if (line2.Contains("/iproxy"))
                {
                    var splitedString = line2.Split(' ');
                    splitedString = splitedString.Where(a => !string.IsNullOrEmpty(a)).ToArray();
                    var id = splitedString[splitedString.Length - 1];
                    var localPort = System.Int32.Parse(splitedString[splitedString.Length - 3]);
                    var remotePort = System.Int32.Parse(splitedString[splitedString.Length - 2]);
                    var pid = System.Int32.Parse(splitedString[1]);
                    devices.Add(new AltUnityMyDevices(id, localPort, remotePort, true, AltUnityPlatform.iOS, pid));
                }
            }
            return devices;
        }
#endif
    }
}
