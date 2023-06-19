#if !UNITY_EDITOR && UNITY_ANDROID
using System;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public class AndroidProxyFinder : IProxyFinder
    {
        public string GetProxy(string uri, string host)
        {
            return CallJavaGetProxy(uri);
        }

        private string CallJavaGetProxy(string uri)
        {
            using (var JavaClass = new AndroidJavaClass("com.alttester.utils.ProxyFinder")) {
                return JavaClass.CallStatic<string>("getProxy", uri);
            }
        }
    }
}
#endif
