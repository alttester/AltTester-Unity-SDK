package ro.altom.altunitytester;

public class TestsHelper {
    public static int GetAltUnityDriverPort() {
        String port = System.getenv("ALTUNITYDRIVER_PORT");
        if (port != null && !port.equals("")) {
            return Integer.parseInt(port);
        }

        return 13000;
    }

    public static String GetAltUnityDriverHost() {
        String host = System.getenv("ALTUNITYDRIVER_HOST");
        if (host != null && !host.equals("")) {
            return host;
        }

        return "127.0.0.1";
    }
}
