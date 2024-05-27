/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver.Logging;

namespace AltTester.AltTesterUnitySDK.Driver
{
    /// <summary>
    /// API to interact with adb programmatically
    /// </summary>
    public class AltReversePortForwarding
    {

#if UNITY_EDITOR
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
#endif
        /// <summary>
        /// Calls adb reverse [-s {deviceId}] tcp:{remotePort} tcp:{localPort}
        /// </summary>
        /// <param name="remotePort">The device's port to reverse from</param>
        /// <param name="localPort">The local port to reverse to</param>
        /// <param name="deviceId">The id of the device</param>
        /// <param name="adbPath">
        /// The adb path.
        /// If no adb path is provided, it tries to use adb from  ${ANDROID_SDK_ROOT}/platform-tools/adb
        /// if ANDROID_SDK_ROOT env variable is not set, it tries to execute adb from path.
        /// </param>
        public static string ReversePortForwardingAndroid(int remotePort = 13000, int localPort = 13000, string deviceId = "", string adbPath = "")
        {
            adbPath = GetAdbPath(adbPath);
            string arguments;
            if (deviceId.Equals(""))
                arguments = "reverse tcp:" + remotePort + " tcp:" + localPort;
            else
            {
                arguments = "-s " + deviceId + " reverse" + " tcp:" + remotePort + " tcp:" + localPort;
            }
            try
            {

                var process = startProcess(adbPath, arguments);

                string stdout = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (stdout.Length > 0)
                {
                    return stdout;
                }
            }
            catch (Exception ex)
            {
                throw new ReversePortForwardingException("Error while running command: " + adbPath + " " + arguments, ex);

            }
            return "Ok";
        }

        /// <summary>
        /// Calls `adb reverse --remove-all` 
        /// </summary>
        /// <param name="adbPath">
        /// The adb path.
        /// If no adb path is provided, it tries to use adb from  ${ANDROID_SDK_ROOT}/platform-tools/adb
        /// if ANDROID_SDK_ROOT env variable is not set, it tries to execute adb from path.
        /// </param>
        public static void RemoveAllReversePortForwardingsAndroid(string adbPath = "")
        {
            adbPath = GetAdbPath(adbPath);
            string arguments = "reverse --remove-all";
            try
            {
                var process = startProcess(adbPath, arguments);
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new ReversePortForwardingException("Error while running command: " + adbPath + " " + arguments, ex);
            }
        }

        /// <summary>
        /// Calls `adb reverse --remove [-s {deviceId}] tcp:{remotePort}`
        /// </summary>
        /// <param name="remotePort">The device's port to be removed </param>
        /// <param name="deviceId">The id of the device to be removed</param>
        /// <param name="adbPath">
        /// The adb path.
        /// If no adb path is provided, it tries to use adb from  ${ANDROID_SDK_ROOT}/platform-tools/adb
        /// if ANDROID_SDK_ROOT env variable is not set, it tries to execute adb from path.
        /// </param>
        public static void RemoveReversePortForwardingAndroid(int remotePort = 13000, string deviceId = "", string adbPath = "")
        {
            adbPath = GetAdbPath(adbPath);
            string arguments = "reverse --remove tcp:" + remotePort;

            if (!string.IsNullOrEmpty(deviceId))
            {
                arguments = "-s " + deviceId + " " + arguments;
            }
            try
            {
                var process = startProcess(adbPath, arguments);
                process.WaitForExit();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                throw new ReversePortForwardingException("Error while running command: " + adbPath + " " + arguments, ex);
            }
        }

        /// <summary>
        /// Runs `adb devices`
        /// </summary>
        /// <param name="adbPath">
        /// The adb path.
        /// If no adb path is provided, it tries to use adb from  ${ANDROID_SDK_ROOT}/platform-tools/adb
        /// if ANDROID_SDK_ROOT env varibale is not set, it tries to execute adb from path.
        /// </param>
        public static List<AltDevice> GetDevicesAndroid(string adbPath = "")
        {
            adbPath = GetAdbPath(adbPath);
            var arguments = "devices";
            try
            {
                var process = startProcess(adbPath, arguments);
                var devices = new List<AltDevice>();

                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line.Length > 0 && !line.StartsWith("List "))
                    {
                        var parts = line.Split('\t');
                        string deviceId = parts[0];
                        devices.Add(new AltDevice(deviceId, "Android"));
                    }
                }
                process.WaitForExit();
                process.StandardError.ReadToEnd();
                return devices;
            }
            catch (Exception ex)
            {
                throw new ReversePortForwardingException("Error while running command: " + adbPath + " " + arguments, ex);
            }
        }
        public static List<AltDevice> GetReversedDevicesAndroid(string adbPath = "")
        {
            adbPath = GetAdbPath(adbPath);
            var arguments = "reverse --list";
            try
            {
                var process = startProcess(adbPath, arguments);
                var devices = new List<AltDevice>();

                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line.Length > 0)
                    {
                        try
                        {
                            var parts = line.Split(' ');
                            string deviceId = parts[0];
                            int remotePort = int.Parse(parts[1].Split(':')[1]);
                            int localPort = int.Parse(parts[2].Split(':')[1]);
                            devices.Add(new AltDevice(deviceId, "Android", remotePort, localPort, true));
                        }
                        catch (System.FormatException)
                        {
#if UNITY_EDITOR

                            logger.Warn("adb reverse also has: " + line + "; which was not included in the list of devices");
#endif
                        }
                    }
                }
                process.WaitForExit();
                return devices;
            }
            catch (Exception ex)
            {
                throw new ReversePortForwardingException("Error while running command: " + adbPath + " " + arguments, ex);
            }
        }

        public static string GetAdbPath(string adbPath)
        {
            if (!string.IsNullOrEmpty(adbPath)) return adbPath;
            var androidSdkRoot = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
            if (!string.IsNullOrEmpty(androidSdkRoot))
            {
                return Path.Combine(androidSdkRoot, "platform-tools", "adb");
            }
            return "adb";
        }

        private static Process startProcess(string processPath, string arguments)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Minimized,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = processPath,
                Arguments = arguments
            };
            process.StartInfo = startInfo;
            process.Start();

            return process;
        }
    }
}

