package com.alttester;

public class TestsHelper {
    public static int GetAltDriverPort() {
        String port = System.getenv("PROXY_PORT");

        if (port != null && port != "") {
            return Integer.parseInt(port);
        }

        return 13000;
    }

    public static String GetAltDriverHost() {
        String host = System.getenv("PROXY_HOST");

        if (host != null && host != "") {
            return host;
        }

        return "127.0.0.1";
    }
}
