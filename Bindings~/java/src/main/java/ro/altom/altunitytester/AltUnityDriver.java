package ro.altom.altunitytester;

import org.apache.logging.log4j.Logger;
import org.apache.logging.log4j.core.config.ConfigurationFactory;
import org.apache.logging.log4j.LogManager;

import ro.altom.altunitytester.Commands.*;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltSetServerLoggingParameters;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnitySetServerLogging;
import ro.altom.altunitytester.Commands.FindObject.*;
import ro.altom.altunitytester.Commands.InputActions.*;
import ro.altom.altunitytester.Commands.UnityCommand.*;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;
import ro.altom.altunitytester.altUnityTesterExceptions.*;
import ro.altom.altunitytester.position.Vector2;

import java.io.IOException;
import java.util.List;

import javax.websocket.CloseReason;

public class AltUnityDriver {
    static {
        ConfigurationFactory custom = new AltUnityDriverConfigFactory();
        ConfigurationFactory.setConfigurationFactory(custom);
    }

    private static final Logger log = LogManager.getLogger(AltUnityDriver.class);

    public static enum PlayerPrefsKeyType {
        Int(1), String(2), Float(3);

        private int val;

        PlayerPrefsKeyType(int val) {
            this.val = val;
        }

        public int getVal() {
            return val;
        }
    }

    public static final String VERSION = "1.6.6";
    public static final int READ_TIMEOUT = 5 * 1000;

    private WebsocketConnection connection = null;

    public AltUnityDriver() {
        this("127.0.0.1", 13000);
    }

    public AltUnityDriver(String host, int port) {

        this(host, port, false);
    }

    public AltUnityDriver(String host, int port, Boolean enableLogging) {
        this(host, port, enableLogging, 60);
    }

    public AltUnityDriver(String host, int port, Boolean enableLogging, int connectTimeout) {
        if (!enableLogging)
            AltUnityDriverConfigFactory.DisableLogging();

        if (host == null || host.isEmpty()) {
            throw new InvalidParamerException("Provided IP address is null or empty");
        }

        this.connection = new WebsocketConnection(host, port, connectTimeout);
        checkServerVersion();

    }

    private String[] splitVersion(String version) {
        return version.split("\\.");
    }

    private void checkServerVersion() {
        String serverVersion;
        try {
            serverVersion = GetServerVersion();
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

    public void stop(CloseReason closeReason) throws IOException {
        this.connection.session.close(closeReason);
    }

    public String GetServerVersion() {
        return new GetServerVersionCommand(this.connection.messageHandler).Execute();
    }

    public void loadScene(AltLoadSceneParameters altLoadSceneParameters) {
        new AltLoadScene(this.connection.messageHandler, altLoadSceneParameters).Execute();
    }

    public void unloadScene(String sceneName) {
        new AltUnloadScene(this.connection.messageHandler, sceneName).Execute();
    }

    public String[] getAllLoadedScenes() {
        return new AltGetAllLoadedScenes(this.connection.messageHandler).Execute();
    }

    /**
     * Delete entire player pref of the game
     */
    public void deletePlayerPref() {
        new AltDeletePlayerPref(this.connection.messageHandler).Execute();
    }

    /**
     * Delete from games player pref a key
     */
    public void deleteKeyPlayerPref(String keyName) {
        new AltDeleteKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
    }

    public void setKeyPlayerPref(String keyName, int valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
    }

    public void setKeyPlayerPref(String keyName, float valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
    }

    public void setKeyPlayerPref(String keyName, String valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
    }

    public int getIntKeyPlayerPref(String keyname) {
        return new AltIntGetKeyPlayerPref(this.connection.messageHandler, keyname).Execute();
    }

    public float getFloatKeyPlayerPref(String keyname) {
        return new AltFloatGetKeyPlayerPref(this.connection.messageHandler, keyname).Execute();
    }

    public String getStringKeyPlayerPref(String keyname) {
        return new AltStringGetKeyPlayerPref(this.connection.messageHandler, keyname).Execute();
    }

    public String getCurrentScene() {
        return new AltGetCurrentScene(this.connection.messageHandler).Execute();
    }

    public float getTimeScale() {
        return new AltGetTimeScale(this.connection.messageHandler).Execute();
    }

    public void setTimeScale(float timeScale) {
        new AltSetTimeScale(this.connection.messageHandler, timeScale).Execute();
    }

    public <T> T callStaticMethod(AltCallStaticMethodParameters altCallStaticMethodParameters, Class<T> returnType) {
        return new AltCallStaticMethod(this.connection.messageHandler, altCallStaticMethodParameters)
                .Execute(returnType);
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
        new AltSwipe(this.connection.messageHandler, xStart, yStart, xEnd, yEnd, durationInSecs).Execute();
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
        new AltSwipeAndWait(this.connection.messageHandler, xStart, yStart, xEnd, yEnd, durationInSecs).Execute();
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
        new AltMultiPointSwipe(this.connection.messageHandler, positions, durationInSecs).Execute();
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
        new AltMultiPointSwipeAndWait(this.connection.messageHandler, positions, durationInSecs).Execute();
    }

    public void holdButton(int xPosition, int yPosition, float durationInSecs) {
        swipe(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    public void holdButtonAndWait(int xPosition, int yPosition, float durationInSecs) {
        swipeAndWait(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    /**
     * Simulates device rotation action in your game.
     */
    public void tilt(AltTiltParameters altTiltParameter) {
        new AltTilt(this.connection.messageHandler, altTiltParameter).Execute();
    }

    /**
     * Simulates device rotation action in your game and waits for the action to
     * finish.
     */
    public void tiltAndWait(AltTiltParameters altTiltParameters) {
        new AltTiltAndWait(this.connection.messageHandler, altTiltParameters).Execute();
    }

    /**
     * Similar command like swipe but instead of swipe from point A to point B you
     * are able to give list a points.
     *
     * @param altPressKeyParameters the builder for the press key commands.
     */
    public void pressKey(AltPressKeyParameters altPressKeyParameters) {
        new AltPressKey(this.connection.messageHandler, altPressKeyParameters).Execute();
    }

    public void pressKey(AltUnityKeyCode keyCode, float power, float duration) {
        pressKey(BuildPressKeyParameters(keyCode, power, duration));
    }

    /**
     * Similar command like swipe but instead of swipe from point A to point B you
     * are able to give list a points.
     *
     * @param altPressKeyParameters the builder for the press key commands.
     */
    public void pressKeyAndWait(AltPressKeyParameters altPressKeyParameters) {
        new AltPressKeyAndWait(this.connection.messageHandler, altPressKeyParameters).Execute();
    }

    public void pressKeyAndWait(AltUnityKeyCode keyCode, float power, float duration) {
        pressKeyAndWait(BuildPressKeyParameters(keyCode, power, duration));
    }

    public void KeyDown(AltKeyParameters altKeyParameters) {
        new AltKeyDown(this.connection.messageHandler, altKeyParameters).Execute();
    }

    public void KeyUp(AltUnityKeyCode keyCode) {
        new AltKeyUp(this.connection.messageHandler, keyCode).Execute();
    }

    /**
     * Simulate mouse movement in your game. This command does not wait for the
     * movement to finish.
     *
     * @param altMoveMouseParameters the builder for the mouse moves command.
     */
    public void moveMouse(AltMoveMouseParameters altMoveMouseParameters) {
        new AltMoveMouse(this.connection.messageHandler, altMoveMouseParameters).Execute();
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
        new AltMoveMouseAndWait(this.connection.messageHandler, altMoveMouseParameters).Execute();
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
        new AltScrollMouse(this.connection.messageHandler, altScrollMouseParameters).Execute();
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
        new AltScrollMouseAndWait(this.connection.messageHandler, altScrollMouseParameters).Execute();
    }

    public void scrollMouseAndWait(float speed, float duration) {
        scrollMouseAndWait(BuildScrollMouseParameters(speed, duration));
    }

    /**
     * @param altFindObjectsParameters
     * @return the first object in the scene that respects the given criteria.
     */
    public AltUnityObject findObject(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObject(this.connection.messageHandler, altFindObjectsParameters).Execute();
    }

    /**
     *
     * @param altFindObjectsParameters
     * @return the first object containing the given criteria
     */
    public AltUnityObject findObjectWhichContains(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObjectWhichContains(this.connection.messageHandler, altFindObjectsParameters).Execute();
    }

    public AltUnityObject findObjectWhichContains(By by, String value, By cameraBy, String cameraValue,
            boolean enabled) {
        return findObjectWhichContains(BuildFindObjectsParameters(by, value, cameraBy, cameraValue, enabled));
    }

    /**
     *
     * @param altFindObjectsParameters
     * @return all the objects respecting the given criteria
     */
    public AltUnityObject[] findObjects(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObjects(this.connection.messageHandler, altFindObjectsParameters).Execute();
    }

    public AltUnityObject[] findObjects(By by, String value, By cameraBy, String cameraValue, boolean enabled) {
        return findObjects(BuildFindObjectsParameters(by, value, cameraBy, cameraValue, enabled));
    }

    /**
     *
     * @param altFindObjectsParameters
     * @return all objects containing the given criteria
     */
    public AltUnityObject[] findObjectsWhichContain(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObjectsWhichContain(this.connection.messageHandler, altFindObjectsParameters).Execute();
    }

    /**
     *
     * @param altGetAllElementsParameters
     * @return information about every object loaded in the currently loaded scenes.
     */
    public AltUnityObject[] getAllElements(AltGetAllElementsParameters altGetAllElementsParameters) {
        return new AltGetAllElements(this.connection.messageHandler, altGetAllElementsParameters).Execute();
    }

    public String waitForCurrentSceneToBe(AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters) {
        return new AltWaitForCurrentSceneToBe(this.connection.messageHandler, altWaitForCurrentSceneToBeParameters)
                .Execute();
    }

    /**
     * Wait until there are no longer any objects that respect the given criteria or
     * times run out and will throw an error.
     *
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    public AltUnityObject waitForObject(AltWaitForObjectsParameters altWaitForObjectsParameters) {
        return new AltWaitForObject(this.connection.messageHandler, altWaitForObjectsParameters).Execute();
    }

    /**
     * Wait until the object in the scene that respect the given criteria is no
     * longer in the scene or times run out and will throw an error.
     *
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    public void waitForObjectToNotBePresent(AltWaitForObjectsParameters altWaitForObjectsParameters) {
        new AltWaitForObjectToNotBePresent(this.connection.messageHandler, altWaitForObjectsParameters).Execute();
    }

    public AltUnityObject waitForObjectWhichContains(AltWaitForObjectsParameters altWaitForObjectsParameters) {
        return new AltWaitForObjectWhichContains(this.connection.messageHandler, altWaitForObjectsParameters).Execute();
    }

    private AltPressKeyParameters BuildPressKeyParameters(AltUnityKeyCode keyCode, float power, float duration) {
        return new AltPressKeyParameters.Builder(keyCode).withPower(power).withDuration(duration).build();
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
        new GetPNGScreenshotCommand(this.connection.messageHandler, path).Execute();
    }

    public void setServerLogging(AltSetServerLoggingParameters parameters) {
        new AltUnitySetServerLogging(this.connection.messageHandler, parameters).Execute();
    }

    public int beginTouch(Vector2 screenCoordinates) {
        return new AltBeginTouch(this.connection.messageHandler, screenCoordinates).Execute();
    }

    public void moveTouch(int fingerId, Vector2 screenCoordinates) {
        new AltMoveTouch(this.connection.messageHandler, fingerId, screenCoordinates).Execute();
    }

    public void endTouch(int fingerId) {
        new AltEndTouch(this.connection.messageHandler, fingerId).Execute();
    }

    /**
     * Tap at screen coordinates
     * 
     * @param parameters Tap parameters
     */
    public void tap(AltTapClickCoordinatesParameters parameters) {
        new AltTapCoordinates(this.connection.messageHandler, parameters).Execute();
    }

    /**
     * Click at screen coordinates
     * 
     * @param parameters Click parameters
     */
    public void click(AltTapClickCoordinatesParameters parameters) {
        new AltClickCoordinates(this.connection.messageHandler, parameters).Execute();
    }

    /**
     * Deprecated port forwarding methods are moved to AltUnityPortForwarding class.
     * This is going to be removed in the future.
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
     * Deprecated port forwarding methods are moved to AltUnityPortForwarding class.
     * This is going to be removed in the future.
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
