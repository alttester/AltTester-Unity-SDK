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
            using (var JavaClass = new AndroidJavaClass("com.alttester.utils.AltProxyFinder")) {
                return JavaClass.CallStatic<string>("getProxy", uri);
            }
        }
    }
}
#endif
