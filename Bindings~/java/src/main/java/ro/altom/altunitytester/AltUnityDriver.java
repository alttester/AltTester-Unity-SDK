package ro.altom.altunitytester;

import org.apache.logging.log4j.Logger;
import org.apache.logging.log4j.core.appender.ConsoleAppender;
import org.apache.logging.log4j.core.config.ConfigurationFactory;
import org.apache.logging.log4j.core.config.Configurator;
import org.apache.logging.log4j.core.config.builder.api.AppenderComponentBuilder;
import org.apache.logging.log4j.core.config.builder.api.ConfigurationBuilder;
import org.apache.logging.log4j.core.config.builder.api.ConfigurationBuilderFactory;
import org.apache.logging.log4j.core.config.builder.api.LayoutComponentBuilder;
import org.apache.logging.log4j.core.config.builder.api.LoggerComponentBuilder;
import org.apache.logging.log4j.core.config.builder.impl.BuiltConfiguration;
import org.apache.logging.log4j.Level;
import org.apache.logging.log4j.LogManager;

import ro.altom.altunitytester.Commands.*;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltSetServerLoggingParameters;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnitySetServerLogging;
import ro.altom.altunitytester.Commands.FindObject.*;
import ro.altom.altunitytester.Commands.InputActions.*;
import ro.altom.altunitytester.Commands.UnityCommand.*;
import ro.altom.altunitytester.altUnityTesterExceptions.*;
import ro.altom.altunitytester.position.Vector2;

import java.io.*;
import java.net.Socket;
import java.util.List;

public class AltUnityDriver {
    static {
        ConfigurationFactory custom = new AltUnityDriverConfigFactory();
        ConfigurationFactory.setConfigurationFactory(custom);
    }

    private static final Logger log = LogManager.getLogger(AltUnityDriver.class);

    public static class PlayerPrefsKeyType {
        public static int IntType = 1;
        public static int StringType = 2;
        public static int FloatType = 3;
    }

    public static final String VERSION = "1.6.4";
    public static final int READ_TIMEOUT = 5 * 1000;

    private Socket socket = null;
    private PrintWriter out = null;
    private DataInputStream in = null;

    private AltBaseSettings altBaseSettings;

    public AltUnityDriver() {
        this("127.0.0.1", 13000);
    }

    public AltUnityDriver(String ip, int port) {

        this(ip, port, ";", "&", false);
    }

    public AltUnityDriver(String ip, int port, String requestSeparator, String requestEnd) {
        this(ip, port, requestSeparator, requestEnd, false);
    }

    public AltUnityDriver(String ip, int port, String requestSeparator, String requestEnd, Boolean logFlag) {
        this(ip, port, requestSeparator, requestEnd, logFlag, 60);
    }

    public AltUnityDriver(AltUnityDriverParams params) {
        this(params.ip, params.port, params.requestSeparator, params.requestEnd, params.logFlag, params.connectTimeout);
    }

    public AltUnityDriver(String ip, int port, String requestSeparator, String requestEnd, Boolean logFlag,
            int connectTimeout) {
        if (!logFlag)
            AltUnityDriverConfigFactory.DisableLogging();

        if (ip == null || ip.isEmpty()) {
            throw new InvalidParamerException("Provided IP address is null or empty");
        }

        while (connectTimeout > 0) {
            try {
                try {
                    log.info(String.format("Initializing connection to %s:%d", ip, port));
                    socket = new Socket(ip, port);
                    socket.setSoTimeout(READ_TIMEOUT);
                    out = new PrintWriter(socket.getOutputStream(), true);
                    in = new DataInputStream(socket.getInputStream());
                } catch (IOException e) {
                    throw new ConnectionException("AltUnityServer not running on port " + port
                            + ",retrying (timing out in " + connectTimeout + " secs)...", e);
                }

                altBaseSettings = new AltBaseSettings(socket, requestSeparator, requestEnd, out, in);
                checkServerVersion();
                break;
            } catch (Exception e) {
                System.out.println(e.getMessage());

                if (socket != null)
                    stop();

                connectTimeout -= 5;
                try {
                    Thread.sleep(5000);
                } catch (InterruptedException e1) {
                    e1.printStackTrace();
                }
            }
            if (connectTimeout <= 0) {
                throw new ConnectionException("Could not create connection to " + String.format("%s:%d", ip, port),
                        new Throwable());
            }
        }
    }

    private String[] splitVersion(String version) {
        return version.split("\\.");
    }

    private void checkServerVersion() {
        String serverVersion;
        try {
            serverVersion = new GetServerVersionCommand(altBaseSettings).Execute();
        } catch (UnknownErrorException ex) {
            serverVersion = "<=1.5.3";
        } catch (AltUnityRecvallMessageFormatException ex) {
            serverVersion = "<=1.5.7";
        }

        String[] parts = splitVersion(serverVersion);
        String majorServer = parts[0];
        String minorServer = (parts.length > 1) ? parts[1] : "";
        parts = splitVersion(AltUnityDriver.VERSION);
        String majorDriver = parts[0];
        String minorDriver = (parts.length > 1) ? parts[1] : "";

        if (!majorServer.equals(majorDriver) || !minorServer.equals(minorDriver)) {
            String message = "Version mismatch. AltUnity Driver version is " + AltUnityDriver.VERSION
                    + ". AltUnity Server version is " + serverVersion + ".";

            log.warn(message);
            System.out.println(message);
        }
    }

    public void stop() {
        new AltStop(altBaseSettings).Execute();
        try {
            socket.close();
        } catch (IOException ex) {
            log.warn(ex);
        }
    }

    public String callStaticMethod(AltCallStaticMethodParameters altCallStaticMethodParameters) {
        return new AltCallStaticMethod(altBaseSettings, altCallStaticMethodParameters).Execute();
    }

    @Deprecated
    public String callStaticMethods(AltCallStaticMethodParameters altCallStaticMethodParameters) {
        return new AltCallStaticMethod(altBaseSettings, altCallStaticMethodParameters).Execute();
    }

    @Deprecated
    public String callStaticMethods(String assembly, String typeName, String methodName, String parameters,
            String typeOfParameters) {
        AltCallStaticMethodParameters altCallStaticMethodParameters = new AltCallStaticMethodParameters.Builder(
                typeName, methodName, parameters).withAssembly(assembly).withTypeOfParameters(typeOfParameters).build();
        return callStaticMethods(altCallStaticMethodParameters);
    }

    @Deprecated
    public String callStaticMethods(String typeName, String methodName, String parameters) {
        return callStaticMethods("", typeName, methodName, parameters, "");
    }

    public void loadScene(AltLoadSceneParameters altLoadSceneParameters) {
        new AltLoadScene(altBaseSettings, altLoadSceneParameters).Execute();
    }

    public void unloadScene(String sceneName) {
        new AltUnloadScene(altBaseSettings, sceneName).Execute();
    }

    public String[] getAllLoadedScenes() {
        return new AltGetAllLoadedScenes(altBaseSettings).Execute();
    }

    /**
     * Ability to access altBaseSettings.
     *
     * @return Returns the AltBaseSettings used by the driver.
     */
    public AltBaseSettings GetAltBaseSettings() {
        return altBaseSettings;
    }

    /**
     * Delete entire player pref of the game
     */
    public void deletePlayerPref() {
        new AltDeletePlayerPref(altBaseSettings).Execute();
    }

    /**
     * Delete from games player pref a key
     */
    public void deleteKeyPlayerPref(String keyName) {
        new AltDeleteKeyPlayerPref(altBaseSettings, keyName).Execute();
    }

    public void setKeyPlayerPref(String keyName, int valueName) {
        new AltSetKeyPlayerPref(altBaseSettings, keyName, valueName).Execute();
    }

    public void setKeyPlayerPref(String keyName, float valueName) {
        new AltSetKeyPlayerPref(altBaseSettings, keyName, valueName).Execute();
    }

    public void setKeyPlayerPref(String keyName, String valueName) {
        new AltSetKeyPlayerPref(altBaseSettings, keyName, valueName).Execute();
    }

    public int getIntKeyPlayerPref(String keyname) {
        return new AltIntGetKeyPlayerPref(altBaseSettings, keyname).Execute();
    }

    public float getFloatKeyPlayerPref(String keyname) {
        return new AltFloatGetKeyPlayerPref(altBaseSettings, keyname).Execute();
    }

    public String getStringKeyPlayerPref(String keyname) {
        return new AltStringGetKeyPlayerPref(altBaseSettings, keyname).Execute();
    }

    public String getCurrentScene() {
        return new AltGetCurrentScene(altBaseSettings).Execute();
    }

    public float getTimeScale() {
        return new AltGetTimeScale(altBaseSettings).Execute();
    }

    public void setTimeScale(float timeScale) {
        new AltSetTimeScale(altBaseSettings, timeScale).Execute();
    }

    /**
     * Simulate scroll mouse action in your game. This command does not wait for the
     * action to finish.
     *
     * @param xStart         x coordinate of the screen where the swipe begins.
     * @param yStart         y coordinate of the screen where the swipe begins.
     * @param xEnd           x coordinate of the screen where the swipe ends.
     * @param yEnd           y coordinate of the screen where the swipe ends.
     * @param durationInSecs The time measured in seconds to move the mouse from
     *                       current position to the set location.
     */
    public void swipe(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) {
        new AltSwipe(altBaseSettings, xStart, yStart, xEnd, yEnd, durationInSecs).Execute();
    }

    /**
     * Simulate scroll mouse action in your game. This command waits for the action
     * to finish.
     *
     * @param xStart         x coordinate of the screen where the swipe begins.
     * @param yStart         y coordinate of the screen where the swipe begins.
     * @param xEnd           x coordinate of the screen where the swipe ends.
     * @param yEnd           y coordinate of the screen where the swipe ends.
     * @param durationInSecs The time measured in seconds to move the mouse from
     *                       current position to the set location.
     */
    public void swipeAndWait(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) {
        new AltSwipeAndWait(altBaseSettings, xStart, yStart, xEnd, yEnd, durationInSecs).Execute();
    }

    /**
     * Similar command like swipe but instead of swipe from point A to point B you
     * are able to give list a points.
     *
     * @param positions      collection of positions on the screen where the swipe
     *                       be made
     * @param durationInSecs how many seconds the swipe will need to complete
     */
    public void multipointSwipe(List<Vector2> positions, float durationInSecs) {
        new AltMultiPointSwipe(altBaseSettings, positions, durationInSecs).Execute();
    }

    /**
     * Similar command like swipe but instead of swipe from point A to point B you
     * are able to give list a points. Waits for the movement to finish
     *
     * @param positions      collection of positions on the screen where the swipe
     *                       be made
     * @param durationInSecs how many seconds the swipe will need to complete
     */
    public void multipointSwipeAndWait(List<Vector2> positions, float durationInSecs) {
        new AltMultiPointSwipeAndWait(altBaseSettings, positions, durationInSecs).Execute();
    }

    public void holdButton(int xPosition, int yPosition, float durationInSecs) {
        swipe(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    public void holdButtonAndWait(int xPosition, int yPosition, float durationInSecs) {
        swipeAndWait(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    public AltUnityObject clickScreen(float x, float y) {
        return new AltClickScreen(altBaseSettings, x, y).Execute();
    }

    /**
     * Simulates device rotation action in your game.
     */
    public void tilt(AltTiltParameters altTiltParameter) {
        new AltTilt(altBaseSettings, altTiltParameter).Execute();
    }

    /**
     * Simulates device rotation action in your game and waits for the action to
     * finish.
     */
    public void tiltAndWait(AltTiltParameters altTiltParameters) {
        new AltTiltAndWait(altBaseSettings, altTiltParameters).Execute();
    }

    /**
     * Similar command like swipe but instead of swipe from point A to point B you
     * are able to give list a points.
     *
     * @param altPressKeyParameters the builder for the press key commands.
     */
    public void pressKey(AltPressKeyParameters altPressKeyParameters) {
        new AltPressKey(altBaseSettings, altPressKeyParameters).Execute();
    }

    public void pressKey(String keyName, float power, float duration) {
        pressKey(BuildPressKeyParameters(keyName, power, duration));
    }

    /**
     * Similar command like swipe but instead of swipe from point A to point B you
     * are able to give list a points.
     *
     * @param altPressKeyParameters the builder for the press key commands.
     */
    public void pressKeyAndWait(AltPressKeyParameters altPressKeyParameters) {
        new AltPressKeyAndWait(altBaseSettings, altPressKeyParameters).Execute();
    }

    public void pressKeyAndWait(String keyName, float power, float duration) {
        pressKeyAndWait(BuildPressKeyParameters(keyName, power, duration));
    }

    /**
     * Simulate mouse movement in your game. This command does not wait for the
     * movement to finish.
     *
     * @param altMoveMouseParameters the builder for the mouse moves command.
     */
    public void moveMouse(AltMoveMouseParameters altMoveMouseParameters) {
        new AltMoveMouse(altBaseSettings, altMoveMouseParameters).Execute();
    }

    public void moveMouse(int x, int y, float duration) {
        moveMouse(BuildMoveMouseParameters(x, y, duration));
    }

    /**
     * Simulate mouse movement in your game. This command waits for the movement to
     * finish.
     *
     * @param altMoveMouseParameters the builder for the mouse moves command.
     */
    public void moveMouseAndWait(AltMoveMouseParameters altMoveMouseParameters) {
        new AltMoveMouseAndWait(altBaseSettings, altMoveMouseParameters).Execute();
    }

    public void moveMouseAndWait(int x, int y, float duration) {
        moveMouseAndWait(BuildMoveMouseParameters(x, y, duration));
    }

    /**
     * Simulate scroll mouse action in your game. This command does not wait for the
     * action to finish.
     *
     * @param altScrollMouseParameters the builder for the scroll commands.
     */
    public void scrollMouse(AltScrollMouseParameters altScrollMouseParameters) {
        new AltScrollMouse(altBaseSettings, altScrollMouseParameters).Execute();
    }

    public void scrollMouse(float speed, float duration) {
        scrollMouse(BuildScrollMouseParameters(speed, duration));
    }

    /**
     * Simulate scroll mouse action in your game. This command waits for the action
     * to finish.
     *
     * @param altScrollMouseParameters the builder for the scroll commands.
     */
    public void scrollMouseAndWait(AltScrollMouseParameters altScrollMouseParameters) {
        new AltScrollMouseAndWait(altBaseSettings, altScrollMouseParameters).Execute();
    }

    public void scrollMouseAndWait(float speed, float duration) {
        scrollMouseAndWait(BuildScrollMouseParameters(speed, duration));
    }

    /**
     * @param altFindObjectsParameters
     * @return the first object in the scene that respects the given criteria.
     */
    public AltUnityObject findObject(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObject(altBaseSettings, altFindObjectsParameters).Execute();
    }

    /**
     *
     * @param altFindObjectsParameters
     * @return the first object containing the given criteria
     */
    public AltUnityObject findObjectWhichContains(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObjectWhichContains(altBaseSettings, altFindObjectsParameters).Execute();
    }

    public AltUnityObject findObjectWhichContains(By by, String value, By cameraBy, String cameraPath,
            boolean enabled) {
        return findObjectWhichContains(BuildFindObjectsParameters(by, value, cameraBy, cameraPath, enabled));
    }

    /**
     *
     * @param altFindObjectsParameters
     * @return all the objects respecting the given criteria
     */
    public AltUnityObject[] findObjects(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObjects(altBaseSettings, altFindObjectsParameters).Execute();
    }

    public AltUnityObject[] findObjects(By by, String value, By cameraBy, String cameraPath, boolean enabled) {
        return findObjects(BuildFindObjectsParameters(by, value, cameraBy, cameraPath, enabled));
    }

    /**
     *
     * @param altFindObjectsParameters
     * @return all objects containing the given criteria
     */
    public AltUnityObject[] findObjectsWhichContain(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObjectsWhichContain(altBaseSettings, altFindObjectsParameters).Execute();
    }

    /**
     *
     * @param altGetAllElementsParameters
     * @return information about every object loaded in the currently loaded scenes.
     */
    public AltUnityObject[] getAllElements(AltGetAllElementsParameters altGetAllElementsParameters) {
        return new AltGetAllElements(altBaseSettings, altGetAllElementsParameters).Execute();
    }

    /**
     * Simulate a tap action on the screen at the given coordinates.
     *
     * @param x x coordinate of the screen
     * @param y y coordinate of the screen
     */
    public AltUnityObject tapScreen(int x, int y) {
        return new AltTapScreen(altBaseSettings, x, y).Execute();
    }

    public void tapCustom(int x, int y, int count, float interval) {
        new AltTapCustom(altBaseSettings, x, y, count, interval).Execute();
    }

    public void tapCustom(int x, int y, int count) {
        tapCustom(x, y, count, 0.1f);
    }

    public String waitForCurrentSceneToBe(AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters) {
        return new AltWaitForCurrentSceneToBe(altBaseSettings, altWaitForCurrentSceneToBeParameters).Execute();
    }

    /**
     * Wait until there are no longer any objects that respect the given criteria or
     * times run out and will throw an error.
     *
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    public AltUnityObject waitForObject(AltWaitForObjectsParameters altWaitForObjectsParameters) {
        return new AltWaitForObject(altBaseSettings, altWaitForObjectsParameters).Execute();
    }

    @Deprecated
    public AltUnityObject waitForObjectWithText(AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters) {
        return new AltWaitForObjectWithText(altBaseSettings, altWaitForObjectWithTextParameters).Execute();
    }

    /**
     * Wait until the object in the scene that respect the given criteria is no
     * longer in the scene or times run out and will throw an error.
     *
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    public void waitForObjectToNotBePresent(AltWaitForObjectsParameters altWaitForObjectsParameters) {
        new AltWaitForObjectToNotBePresent(altBaseSettings, altWaitForObjectsParameters).Execute();
    }

    public AltUnityObject waitForObjectWhichContains(AltWaitForObjectsParameters altWaitForObjectsParameters) {
        return new AltWaitForObjectWhichContains(altBaseSettings, altWaitForObjectsParameters).Execute();
    }

    private AltPressKeyParameters BuildPressKeyParameters(String keyName, float power, float duration) {
        return new AltPressKeyParameters.Builder(keyName).withPower(power).withDuration(duration).build();
    }

    private AltMoveMouseParameters BuildMoveMouseParameters(int x, int y, float duration) {
        return new AltMoveMouseParameters.Builder(x, y).withDuration(duration).build();
    }

    private AltScrollMouseParameters BuildScrollMouseParameters(float speed, float duration) {
        return new AltScrollMouseParameters.Builder().withDuration(duration).withSpeed(speed).build();
    }

    private AltFindObjectsParameters BuildFindObjectsParameters(By by, String value, By cameraBy, String cameraName,
            boolean enabled) {
        return new AltFindObjectsParameters.Builder(by, value).isEnabled(enabled).withCamera(cameraBy, cameraName)
                .build();
    }

    public void getPNGScreeshot(String path) {
        new GetPNGScreenshotCommand(altBaseSettings, path).Execute();
    }

    public void setServerLogging(AltSetServerLoggingParameters parameters) {
        new AltUnitySetServerLogging(altBaseSettings, parameters).Execute();
    }

    /**
     * @ Deprecated port forwarding methods are moved to AltUnityPortForwarding
     * class. This is going to be removed in the future.
     */

    @Deprecated
    public static void setupPortForwarding(String platform, String deviceID, int local_tcp_port, int remote_tcp_port) {
        log.info("Setting up port forward for " + platform + " on port " + remote_tcp_port);
        removePortForwarding();
        if (platform.toLowerCase().equals("android".toLowerCase())) {
            try {
                String commandToRun;
                if (deviceID.equals(""))
                    commandToRun = "adb forward tcp:" + local_tcp_port + " tcp:" + remote_tcp_port;
                else
                    commandToRun = "adb -s " + deviceID + " forward  tcp:" + local_tcp_port + " tcp:" + remote_tcp_port;
                Runtime.getRuntime().exec(commandToRun);
                Thread.sleep(1000);
                log.info("adb forward enabled.");
            } catch (Exception e) {
                log.warn("AltUnityServer - abd probably not installed\n" + e);
            }

        } else if (platform.toLowerCase().equals("ios".toLowerCase())) {
            try {
                String commandToRun;
                if (deviceID.equals(""))
                    commandToRun = "iproxy " + local_tcp_port + " " + remote_tcp_port + "&";
                else
                    commandToRun = "iproxy " + local_tcp_port + " " + remote_tcp_port + " " + deviceID + "&";
                Runtime.getRuntime().exec(commandToRun);
                Thread.sleep(1000);
                log.info("iproxy forward enabled.");
            } catch (Exception e) {
                log.warn("AltUnityServer - no iproxy process was running/present\n" + e);
            }
        }
    }

    /**
     * @ Deprecated port forwarding methods are moved to AltUnityPortForwarding
     * class. This is going to be removed in the future.
     */

    @Deprecated
    public static void removePortForwarding() {
        try {
            String commandToExecute = "killall iproxy";
            Runtime.getRuntime().exec(commandToExecute);
            Thread.sleep(1000);
            log.info("Killed any iproxy process that may have been running...");
        } catch (Exception e) {
            log.warn("AltUnityServer - no iproxy process was running/present\n" + e);
        }

        try {
            String commandToExecute = "adb forward --remove-all";
            Runtime.getRuntime().exec(commandToExecute);
            Thread.sleep(1000);
            log.info("Removed existing adb forwarding...");
        } catch (Exception e) {
            log.warn("AltUnityServer - adb probably not installed\n" + e);
        }
    }

    public enum By {
        TAG, LAYER, NAME, COMPONENT, PATH, ID, TEXT
    }
}
