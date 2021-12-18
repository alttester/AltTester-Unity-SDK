package ro.altom.altunitytester;

import org.apache.logging.log4j.Logger;
import org.apache.logging.log4j.core.config.ConfigurationFactory;
import org.apache.logging.log4j.LogManager;

import ro.altom.altunitytester.Commands.*;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltSetServerLoggingParameters;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityAddNotificationListener;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityAddNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityRemoveNotificationListener;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityRemoveNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnitySetServerLogging;
import ro.altom.altunitytester.Commands.FindObject.*;
import ro.altom.altunitytester.Commands.InputActions.*;
import ro.altom.altunitytester.Commands.UnityCommand.*;
import ro.altom.altunitytester.Notifications.INotificationCallbacks;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParameters;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;
import ro.altom.altunitytester.altUnityTesterExceptions.*;
import ro.altom.altunitytester.position.Vector2;
import java.io.IOException;

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

    public static final String VERSION = "1.7.0-alpha";
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
        if (!enableLogging) {
            AltUnityDriverConfigFactory.DisableLogging();
        }

        if (host == null || host.isEmpty()) {
            throw new InvalidParameterException("Provided host address is null or empty");
        }

        this.connection = new WebsocketConnection(host, port, connectTimeout);
        this.connection.connect();
        checkServerVersion();
    }

    private String[] splitVersion(String version) {
        String[] parts = version.split("\\.");
        return new String[] { parts[0], (parts.length > 1) ? parts[1] : "" };
    }

    private void checkServerVersion() {
        String serverVersion = getServerVersion();

        String[] parts = splitVersion(serverVersion);
        String majorServer = parts[0];
        String minorServer = parts[1];

        parts = splitVersion(AltUnityDriver.VERSION);
        String majorDriver = parts[0];
        String minorDriver = parts[1];

        if (!majorServer.equals(majorDriver) || !minorServer.equals(minorDriver)) {
            String message = String.format(
                    "Version mismatch. AltUnity Driver version is %s. AltUnity Tester version is %s.",
                    AltUnityDriver.VERSION, serverVersion);
            log.warn(message);
            System.out.println(message);
        }
    }

    public void stop() throws IOException {
        this.connection.close();
    }

    public String getServerVersion() {
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

    public void setCommandResponseTimeout(int timeout) {
        this.connection.messageHandler.setCommandTimeout(timeout);
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

    public int getIntKeyPlayerPref(String keyName) {
        return new AltIntGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
    }

    public float getFloatKeyPlayerPref(String keyName) {
        return new AltFloatGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
    }

    public String getStringKeyPlayerPref(String keyName) {
        return new AltStringGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
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
     * Simulates a swipe action between two points.
     */
    public void swipe(AltSwipeParameters swipeParams) {
        new AltSwipe(this.connection.messageHandler, swipeParams).Execute();
    }

    /**
     * Simulates a multipoint swipe action.
     */
    public void multipointSwipe(AltMultiPointSwipeParameters paramers) {
        new AltMultiPointSwipe(this.connection.messageHandler, paramers).Execute();
    }

    /**
     * Simulates holding left click button down for a specified amount of time at
     * given coordinates.
     */
    public void holdButton(AltHoldParameters holdParams) {
        swipe(holdParams);
    }

    /**
     * Simulates device rotation action in your game.
     */
    public void tilt(AltTiltParameters altTiltParameter) {
        new AltTilt(this.connection.messageHandler, altTiltParameter).Execute();
    }

    /**
     * Simulates key press action in your game.
     */
    public void pressKey(AltPressKeyParameters altPressKeyParameters) {
        new AltPressKey(this.connection.messageHandler, altPressKeyParameters).Execute();
    }

    public void keyDown(AltKeyParameters altKeyParameters) {
        new AltKeyDown(this.connection.messageHandler, altKeyParameters).Execute();
    }

    public void keyUp(AltUnityKeyCode keyCode) {
        new AltKeyUp(this.connection.messageHandler, keyCode).Execute();
    }

    /**
     * Simulate mouse movement in your game.
     *
     * @param altMoveMouseParameters the builder for the move mouse command.
     */
    public void moveMouse(AltMoveMouseParameters altMoveMouseParameters) {
        new AltMoveMouse(this.connection.messageHandler, altMoveMouseParameters).Execute();
    }

    /**
     * Simulate scroll action in your game.
     *
     * @param altScrollParameters the builder for the scroll command.
     */
    public void scroll(AltScrollParameters altScrollParameters) {
        new AltScroll(this.connection.messageHandler, altScrollParameters).Execute();
    }

    /**
     * @param altFindObjectsParameters the builder for the find objects command.
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

    private AltFindObjectsParameters BuildFindObjectsParameters(By by, String value, By cameraBy, String cameraName,
            boolean enabled) {
        return new AltFindObjectsParameters.Builder(by, value).isEnabled(enabled).withCamera(cameraBy, cameraName)
                .build();
    }

    public void getPNGScreenshot(String path) {
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

    public <T> T GetStaticProperty(AltGetComponentPropertyParameters parameters, Class<T> returnType) {
        return new AltGetStaticProperty(this.connection.messageHandler, parameters).Execute(returnType);
    }

    public void AddNotification(AltUnityAddNotificationListenerParams params) {
        new AltUnityAddNotificationListener(this.connection.messageHandler, params).Execute();
    }

    public void RemoveNotificationListener(AltUnityRemoveNotificationListenerParams notificationType) {

        new AltUnityRemoveNotificationListener(this.connection.messageHandler, notificationType).Execute();

    }

    public enum By {
        TAG, LAYER, NAME, COMPONENT, PATH, ID, TEXT
    }
}
