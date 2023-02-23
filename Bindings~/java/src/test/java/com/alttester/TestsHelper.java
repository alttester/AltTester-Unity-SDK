package com.alttester;

import java.util.Collection;
import com.alttester.Commands.AltCommands.AltAddNotificationListenerParams;
import com.alttester.Commands.AltCommands.NotificationType;

public class TestsHelper {
    public static int GetAltDriverPort() {
        String port = System.getenv("ALTSERVER_PORT");

        if (port != null && port != "") {
            return Integer.parseInt(port);
        }

        return 13000;
    }

    public static String GetAltDriverHost() {
        String host = System.getenv("ALTSERVER_HOST");

        if (host != null && host != "") {
            return host;
        }

        return "127.0.0.1";
    }

    public static AltDriver GetAltDriver() {
        AltDriver altDriver = new AltDriver(TestsHelper.GetAltDriverHost(), TestsHelper.GetAltDriverPort(), true);
        return altDriver;
    }
}
