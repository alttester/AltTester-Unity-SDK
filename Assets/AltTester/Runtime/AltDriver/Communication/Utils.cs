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
using System.Threading;

namespace AltTester.AltTesterUnitySDK.Driver.Communication
{
    public class Utils
    {
        public static Uri CreateURI(string host, int port, string path, string appName, string platform, string platformVersion, string deviceInstanceId, string appId = null, string driverType = "SDK")
        {
            Uri uri;
            if (string.IsNullOrEmpty(appId))
            {
                if (!Uri.TryCreate(string.Format("ws://{0}:{1}{2}?appName={3}&platform={4}&platformVersion={5}&deviceInstanceId={6}&driverType={7}", host, port, path, Uri.EscapeDataString(appName), Uri.EscapeDataString(platform), Uri.EscapeDataString(platformVersion), Uri.EscapeDataString(deviceInstanceId), Uri.EscapeDataString(driverType)), UriKind.Absolute, out uri))
                {
                    throw new Exception(String.Format("Invalid host or port {0}:{1}{2}", host, port, path));
                }
            }
            else
            {
                if (!Uri.TryCreate(string.Format("ws://{0}:{1}{2}?appName={3}&platform={4}&platformVersion={5}&deviceInstanceId={6}&appId={7}&driverType={8}", host, port, path, Uri.EscapeDataString(appName), Uri.EscapeDataString(platform), Uri.EscapeDataString(platformVersion), Uri.EscapeDataString(deviceInstanceId), Uri.EscapeDataString(appId), Uri.EscapeDataString(driverType)), UriKind.Absolute, out uri))
                {
                    throw new Exception(String.Format("Invalid host or port {0}:{1}{2}", host, port, path));
                }
            }


            return uri;
        }

        public static void SleepFor(float time)
        {
            Thread.Sleep(System.Convert.ToInt32(time * 1000));
        }

        public static string TrimLog(string log, int maxLogLength = 1000)
        {
            if (string.IsNullOrEmpty(log))
            {
                return log;
            }

            if (log.Length <= maxLogLength)
            {
                return log;
            }

            return log.Substring(0, maxLogLength) + "[...]";
        }
    }
}
