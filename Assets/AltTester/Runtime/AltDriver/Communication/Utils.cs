/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Threading;

namespace AltTester.AltTesterSDK.Driver.Communication
{
    public class Utils
    {
        public static Uri CreateURI(string host, int port, string path, string appName, string platform, string platformVersion, string deviceInstanceId, string appId = null, string driverType = "SDK", bool secureMode = false)
        {
            Uri uri;

            string scheme = secureMode ? "wss" : "ws";
            string capabilityTags = "";
            if (string.IsNullOrEmpty(appId))
            {
                if (!Uri.TryCreate(string.Format("{0}://{1}:{2}{3}?appName={4}&platform={5}&platformVersion={6}&deviceInstanceId={7}&driverType={8}{9}", scheme, host, port, path, Uri.EscapeDataString(appName), Uri.EscapeDataString(platform), Uri.EscapeDataString(platformVersion), Uri.EscapeDataString(deviceInstanceId), Uri.EscapeDataString(driverType), capabilityTags), UriKind.Absolute, out uri))
                {
                    throw new Exception(String.Format("Invalid host or port {0}:{1}{2}", host, port, path));
                }
            }
            else
            {
                if (!Uri.TryCreate(string.Format("{0}://{1}:{2}{3}?appName={4}&platform={5}&platformVersion={6}&deviceInstanceId={7}&appId={8}&driverType={9}{10}", scheme, host, port, path, Uri.EscapeDataString(appName), Uri.EscapeDataString(platform), Uri.EscapeDataString(platformVersion), Uri.EscapeDataString(deviceInstanceId), Uri.EscapeDataString(appId), Uri.EscapeDataString(driverType), capabilityTags), UriKind.Absolute, out uri))
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
