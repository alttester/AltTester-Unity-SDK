package ro.altom.alttester;

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
}
