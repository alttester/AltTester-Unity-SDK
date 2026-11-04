/*
    Copyright(C) 2026 Altom Consulting
*/

#if !UNITY_EDITOR && UNITY_ANDROID
using System;
using UnityEngine;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class AndroidProxyFinder : IProxyFinder
    {
        public string GetProxy(string uri, string host)
        {
            Debug.Log("[AltTester][AndroidProxyFinder] GetProxy called");
            var result = CallJavaGetProxy(uri);
            Debug.Log("[AltTester][AndroidProxyFinder] GetProxy result: " + (result != null ? "proxy found" : "no proxy"));
            return result;
        }

        private string CallJavaGetProxy(string uri)
        {
            using (var JavaClass = new AndroidJavaClass("com.alttester.utils.AltProxyFinder")) {
                return JavaClass.CallStatic<string>("getProxy", uri);
            }
        }
    }
}
#endif
