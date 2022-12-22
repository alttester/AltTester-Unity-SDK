package com.alttester;

public class TestsHelper {
    public static int GetAltDriverPort() {
        String port = System.getenv("ALTDRIVER_PORT");
        if (port != null && port == "") {
            return Integer.parseInt(port);
        }

        return 13000;
    }

    public static String GetAltDriverHost() {
        String host = System.getenv("ALTDRIVER_HOST");
        if (host != null && host == "") {
            return host;
        }

        return "127.0.0.1";
    }

    public static AltDriver getAltDriver() {
        return new AltDriver();
    }

    public static AltDriver getAltDriver(String host, int port) {
        return new AltDriver(host, port);
    }

    public static AltDriver getAltDriver(String host, int port, Boolean enableLogging) {
        return new AltDriver(host, port, enableLogging);
    }

    public static AltDriver getAltDriver(String host, int port, Boolean enableLogging, int connectTimeout) {
        return new AltDriver(host, port, enableLogging, connectTimeout);
    }
}
