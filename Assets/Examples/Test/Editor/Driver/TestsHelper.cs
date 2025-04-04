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

namespace AltTester.AltTesterSDK.Driver.Tests
{
    public class TestsHelper
    {
        public static int GetAltDriverPort()
        {
            string port = System.Environment.GetEnvironmentVariable("ALTSERVER_PORT");
            if (!string.IsNullOrEmpty(port))
            {
                return int.Parse(port);
            }

            return 13000;
        }

        public static string GetAltDriverHost()
        {
            string host = System.Environment.GetEnvironmentVariable("ALTSERVER_HOST");
            if (!string.IsNullOrEmpty(host))
            {
                return host;
            }

            return "127.0.0.1";
        }

        public static AltDriver GetAltDriver()
        {
            return new AltDriver(host: GetAltDriverHost(), port: GetAltDriverPort(), enableLogging: true);
        }

    }
}
