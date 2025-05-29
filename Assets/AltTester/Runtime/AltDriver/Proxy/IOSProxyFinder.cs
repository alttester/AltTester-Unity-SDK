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

#if !UNITY_EDITOR && UNITY_IOS
using System;
using System.Runtime.InteropServices;
using System.Globalization;
using UnityEngine;
using AltTester.AltTesterSDK.Driver.Logging;

namespace AltTester.AltTesterSDK.Driver.Proxy
{
    public class IOSProxyFinder : IProxyFinder
    {
        [DllImport("__Internal")]
        private static extern string _getProxy(string uri, string host);

        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        public string GetProxy(string uri, string host)
        {
            var result = _getProxy(uri, host);

            if (string.IsNullOrEmpty(result))
            {
                NSLog("No proxy found in IOSProxyFinder for uri: {0} and host: {1}", uri, host);
                return null;
            }
            NSLog("Using proxy in IOSProxyFinder: {0} for uri: {1} and host: {2}", result, uri, host);
            return result;
        }
    }
}
#endif
