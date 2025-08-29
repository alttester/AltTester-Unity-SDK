/*
    Copyright(C) 2025 Altom Consulting

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
using AltTester.AltTesterSDK.Driver.Logging;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class AndroidProxyFinder : IProxyFinder
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        public string GetProxy(string uri, string host)
        {
            logger.Info("AndroidProxyFinder.GetProxy called with uri: {0}, host: {1}", uri, host);
            return CallJavaGetProxy(uri);
        }

        private string CallJavaGetProxy(string uri)
        {
            logger.Info("Calling Java method to get proxy for uri in AndroidProxyFinder: {0}", uri);
            using (var JavaClass = new AndroidJavaClass("com.alttester.utils.AltProxyFinder")) {
                return JavaClass.CallStatic<string>("getProxy", uri);
            }
        }
    }
}
#endif
