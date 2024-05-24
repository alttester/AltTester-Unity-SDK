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

package com.alttester;

public class TestsHelper {
    public static int getAltDriverPort() {
        String port = System.getenv("ALTSERVER_PORT");

        if (port != null && port != "") {
            return Integer.parseInt(port);
        }

        return 13000;
    }

    public static String getAltDriverHost() {
        String host = System.getenv("ALTSERVER_HOST");

        if (host != null && host != "") {
            return host;
        }

        return "127.0.0.1";
    }

    public static AltDriver getAltDriver() {
        return new AltDriver(TestsHelper.getAltDriverHost(), TestsHelper.getAltDriverPort(), true, 60);
    }
}
