using System;
using System.Threading;

namespace AltTester.AltTesterUnitySDK.Driver.Communication
{
    public class Utils
    {
        public static Uri CreateURI(string host, int port, string path, string appName)
        {
            Uri uri;
            if (!Uri.TryCreate(string.Format("ws://{0}:{1}{2}?appName={3}", host, port, path, appName), UriKind.Absolute, out uri))
            {
                throw new Exception(String.Format("Invalid host or port {0}:{1}", host, port));
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
