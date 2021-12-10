package ro.altom.altunitytester;

import java.nio.file.Paths;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import ro.altom.altunitytester.altUnityTesterExceptions.PortForwardingException;

public class AltUnityPortForwarding {
    private static final Logger log = LogManager.getLogger(AltUnityPortForwarding.class);

    public static String getAdbPath(String adbPath) {
        if (adbPath != null && !adbPath.isEmpty())
            return adbPath;

        String androidSdkRoot = System.getenv("ANDROID_SDK_ROOT");

        if (androidSdkRoot != null && !androidSdkRoot.isEmpty()) {
            return Paths.get(androidSdkRoot, "platform-tools", "adb").toString();
        }
        return "adb";
    }

    public static String getIproxyPath(String iproxyPath) {
        if (iproxyPath != null && !iproxyPath.isEmpty())
            return iproxyPath;

        return "iproxy";
    }

    public static void forwardAndroid() {
        forwardAndroid(13000, 13000, "", "");
    }

    public static void forwardAndroid(int localPort) {
        forwardAndroid(localPort, 13000, "", "");
    }

    public static void forwardAndroid(int localPort, int remotePort) {
        forwardAndroid(localPort, remotePort, "", "");
    }

    public static void forwardAndroid(int localPort, int remotePort, String deviceId) {
        forwardAndroid(localPort, remotePort, deviceId, "");
    }

    public static void forwardAndroid(int localPort, int remotePort, String deviceId, String adbPath) {
        adbPath = getAdbPath(adbPath);
        log.debug("Setting up port forward for android local port " + localPort + " remote port: " + remotePort);

        String command;
        if (deviceId.equals(""))
            command = adbPath + " forward tcp:" + localPort + " tcp:" + remotePort;
        else
            command = adbPath + " -s " + deviceId + " forward  tcp:" + localPort + " tcp:" + remotePort;

        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new PortForwardingException("An exception occured while running command: " + command, ex);
        }

        log.debug("adb forward enabled.");
    }

    public static void removeForwardAndroid() {
        removeForwardAndroid(13000, "", "");
    }

    public static void removeForwardAndroid(int localPort) {
        removeForwardAndroid(localPort, "", "");
    }

    public static void removeForwardAndroid(int localPort, String deviceId) {
        removeForwardAndroid(localPort, deviceId, "");
    }

    public static void removeForwardAndroid(int localPort, String deviceId, String adbPath) {
        adbPath = getAdbPath(adbPath);
        log.debug("Removing android forward localPort: " + localPort + " deviceId: " + deviceId);

        String arguments = "forward --remove tcp:" + localPort;
        if (deviceId != null && !deviceId.isEmpty()) {
            arguments = "-s " + deviceId + " " + arguments;
        }
        String command = adbPath + " " + arguments;

        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new PortForwardingException("An exception occured while running command: " + command, ex);
        }
        log.debug("Android forward removed...");
    }

    public static void removeAllForwardAndroid() {
        removeAllForwardAndroid("");
    }

    public static void removeAllForwardAndroid(String adbPath) {
        adbPath = getAdbPath(adbPath);

        String command = adbPath + " forward --remove-all";
        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new PortForwardingException("An exception occured while running command: " + command, ex);
        }
        log.debug("Removed all existing adb forwarding...");
    }

    public static void forwardIos() {
        forwardIos(13000, 13000, "", "");
    }

    public static void forwardIos(int localPort) {
        forwardIos(localPort, 13000, "", "");
    }

    public static void forwardIos(int localPort, int devicePort) {
        forwardIos(localPort, devicePort, "", "");
    }

    public static void forwardIos(int localPort, int devicePort, String deviceId) {
        forwardIos(localPort, devicePort, deviceId, "");
    }

    public static void forwardIos(int localPort, int devicePort, String deviceId, String iproxyPath) {
        iproxyPath = getIproxyPath(iproxyPath);
        String arguments;
        if (deviceId == null || deviceId.isEmpty())
            arguments = localPort + " " + devicePort + "&";
        else
            arguments = localPort + " " + devicePort + " -u " + deviceId + "&";
        String command = iproxyPath + " " + arguments;

        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new PortForwardingException("An exception occured while running command: " + command, ex);
        }
        log.debug("iproxy forward enabled.");
    }

    public static void killAllIproxyProcess() {
        String command = "killall iproxy";
        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new PortForwardingException("An exception occured while running command: " + command, ex);
        }
        log.debug("Killed any iproxy process that may have been running...");
    }
}
