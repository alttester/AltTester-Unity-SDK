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

package com.alttester;

import java.nio.file.Paths;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import com.alttester.altTesterExceptions.ReversePortForwardingException;

public class AltReversePortForwarding {
    private static final Logger log = LogManager.getLogger(AltReversePortForwarding.class);

    public static String getAdbPath(String adbPath) {
        if (adbPath != null && !adbPath.isEmpty())
            return adbPath;

        String androidSdkRoot = System.getenv("ANDROID_SDK_ROOT");

        if (androidSdkRoot != null && !androidSdkRoot.isEmpty()) {
            return Paths.get(androidSdkRoot, "platform-tools", "adb").toString();
        }
        return "adb";
    }

    public static void reversePortForwardingAndroid() {
        reversePortForwardingAndroid(13000, 13000, "", "");
    }

    public static void reversePortForwardingAndroid(int localPort) {
        reversePortForwardingAndroid(localPort, 13000, "", "");
    }

    public static void reversePortForwardingAndroid(int localPort, int remotePort) {
        reversePortForwardingAndroid(localPort, remotePort, "", "");
    }

    public static void reversePortForwardingAndroid(int localPort, int remotePort, String deviceId) {
        reversePortForwardingAndroid(localPort, remotePort, deviceId, "");
    }

    public static void reversePortForwardingAndroid(int remotePort, int localPort, String deviceId, String adbPath) {
        adbPath = getAdbPath(adbPath);
        log.debug(
                "Setting up reverse port forward for android remote port " + remotePort + " local port: " + localPort);

        String command;
        if (deviceId.equals(""))
            command = adbPath + " reverse tcp:" + remotePort + " tcp:" + localPort;
        else
            command = adbPath + " -s " + deviceId + " reverse  tcp:" + remotePort + " tcp:" + localPort;

        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new ReversePortForwardingException("An exception occurred while running command: " + command, ex);
        }

        log.debug("adb reverse port forwarding enabled.");
    }

    public static void removeReversePortForwardingAndroid() {
        removeReversePortForwardingAndroid(13000, "", "");
    }

    public static void removeReversePortForwardingAndroid(int localPort) {
        removeReversePortForwardingAndroid(localPort, "", "");
    }

    public static void removeReversePortForwardingAndroid(int localPort, String deviceId) {
        removeReversePortForwardingAndroid(localPort, deviceId, "");
    }

    public static void removeReversePortForwardingAndroid(int remotePort, String deviceId, String adbPath) {
        adbPath = getAdbPath(adbPath);
        log.debug("Removing android reverse port forwarding remotePort: " + remotePort + " deviceId: " + deviceId);

        String arguments = "reverse --remove tcp:" + remotePort;
        if (deviceId != null && !deviceId.isEmpty()) {
            arguments = "-s " + deviceId + " " + arguments;
        }
        String command = adbPath + " " + arguments;

        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new ReversePortForwardingException("An exception occurred while running command: " + command, ex);
        }
        log.debug("Android reverse port forwarding removed...");
    }

    public static void removeAllReversePortForwardingsAndroid() {
        removeAllReversePortForwardingsAndroid("");
    }

    public static void removeAllReversePortForwardingsAndroid(String adbPath) {
        adbPath = getAdbPath(adbPath);

        String command = adbPath + " reverse --remove-all";
        try {
            Runtime.getRuntime().exec(command);
            Thread.sleep(1000);
        } catch (Exception ex) {
            throw new ReversePortForwardingException("An exception occurred while running command: " + command, ex);
        }
        log.debug("Removed all existing adb reverse port forwardings...");
    }
}
