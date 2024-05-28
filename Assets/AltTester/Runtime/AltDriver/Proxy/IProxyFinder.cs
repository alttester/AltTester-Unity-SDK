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

namespace AltTester.AltTesterUnitySDK.Driver.Proxy
{
    public interface IProxyFinder
    {
        /// <summary>
        /// Finds the appropriate proxy server for the specified URL and host.
        /// </summary>
        /// <param name="uri">The URL for which the proxy server needs to be determined.</param>
        /// <param name="host">The host associated with the URL. This is only for convenience; it is the same string as between :// and the first : or / after that.</param>
        /// <returns>The proxy server uri. If the string is null, no proxies should be used.</returns>
        string GetProxy(string uri, string host);
    }
}
