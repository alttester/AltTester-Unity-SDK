package ro.altom.altunitytester;

import org.apache.logging.log4j.Logger;
import org.apache.logging.log4j.core.config.ConfigurationFactory;
import org.apache.logging.log4j.LogManager;

import ro.altom.altunitytester.Commands.*;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltSetServerLoggingParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityAddNotificationListener;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityAddNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityRemoveNotificationListener;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityRemoveNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnitySetServerLogging;
import ro.altom.altunitytester.Commands.FindObject.*;
import ro.altom.altunitytester.Commands.InputActions.*;
import ro.altom.altunitytester.Commands.UnityCommand.*;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.altunitytester.altUnityTesterExceptions.*;
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

    public static final String VERSION = "1.7.0";
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

    /**
     * Closes the connection to the running instrumented app
     */
    public void stop() throws IOException {
        this.connection.close();
    }

    /**
     * Gets the AltUnity Tester version, used to instrument the app
     */
    public String getServerVersion() {
        return new GetServerVersionCommand(this.connection.messageHandler).Execute();
    }

    /**
     * Loads a scene.
     */
    public void loadScene(AltLoadSceneParams altLoadSceneParameters) {
        new AltLoadScene(this.connection.messageHandler, altLoadSceneParameters).Execute();
    }

    /**
     * Unloads a scene.
     * 
     * @param sceneName - the scene name
     */
    public void unloadScene(AltUnloadSceneParams unloadSceneParams) {
        new AltUnloadScene(this.connection.messageHandler, unloadSceneParams).Execute();
    }

    /**
     * Returns all the scenes that have been loaded.
     */
    public String[] getAllLoadedScenes() {
        return new AltGetAllLoadedScenes(this.connection.messageHandler).Execute();
    }

    /**
     * Sets the value for the command response timeout.
     */
    public void setCommandResponseTimeout(int timeout) {
        this.connection.messageHandler.setCommandTimeout(timeout);
    }

    /**
     * Removes all keys and values from PlayerPref.
     */
    public void deletePlayerPref() {
        new AltDeletePlayerPref(this.connection.messageHandler).Execute();
    }

    /**
     * Removes key and its corresponding value from PlayerPrefs.
     */
    public void deleteKeyPlayerPref(String keyName) {
        new AltDeleteKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
    }

    /**
     * Sets the value for a given key in PlayerPrefs.
     */
    public void setKeyPlayerPref(String keyName, int valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
    }

    /**
     * Sets the value for a given key in PlayerPrefs.
     */
    public void setKeyPlayerPref(String keyName, float valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
    }

    /**
     * Sets the value for a given key in PlayerPrefs.
     */
    public void setKeyPlayerPref(String keyName, String valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
    }

    /**
     * Returns the value for a given key from PlayerPrefs.
     */
    public int getIntKeyPlayerPref(String keyName) {
        return new AltIntGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
    }

    /**
     * Returns the value for a given key from PlayerPrefs.
     */
    public float getFloatKeyPlayerPref(String keyName) {
        return new AltFloatGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
    }

    /**
     * Returns the value for a given key from PlayerPrefs.
     */
    public String getStringKeyPlayerPref(String keyName) {
        return new AltStringGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
    }

    /**
     * Returns the current active scene.
     */
    public String getCurrentScene() {
        return new AltGetCurrentScene(this.connection.messageHandler).Execute();
    }

    /**
     * Returns the value of the time scale.
     */
    public float getTimeScale() {
        return new AltGetTimeScale(this.connection.messageHandler).Execute();
    }

    /**
     * Sets the value of the time scale.
     */
    public void setTimeScale(AltSetTimeScaleParams setTimescaleParams) {
        new AltSetTimeScale(this.connection.messageHandler, setTimescaleParams).Execute();
    }

    /**
     * Invokes static methods from your game.
     */
    public <T> T callStaticMethod(AltCallStaticMethodParams altCallStaticMethodParams, Class<T> returnType) {
        return new AltCallStaticMethod(this.connection.messageHandler, altCallStaticMethodParams)
                .Execute(returnType);
    }

    /**
     * Simulates a swipe action between two points.
     */
    public void swipe(AltSwipeParams swipeParams) {
        new AltSwipe(this.connection.messageHandler, swipeParams).Execute();
    }

    /**
     * Simulates a multipoint swipe action.
     */
    public void multipointSwipe(AltMultiPointSwipeParams parameters) {
        new AltMultiPointSwipe(this.connection.messageHandler, parameters).Execute();
    }

    /**
     * Simulates holding left click button down for a specified amount of time at
     * given coordinates.
     */
    public void holdButton(AltHoldParams holdParams) {
        swipe(holdParams);
    }

    /**
     * Simulates device rotation action in your game.
     */
    public void tilt(AltTiltParams altTiltParameter) {
        new AltTilt(this.connection.messageHandler, altTiltParameter).Execute();
    }

    /**
     * Simulates key press action in your game.
     */
    public void pressKey(AltPressKeyParams altPressKeyParameters) {
        new AltPressKey(this.connection.messageHandler, altPressKeyParameters).Execute();
    }

    /**
     * Simulates a key down.
     */
    public void keyDown(AltKeyDownParams keyDownParams) {
        new AltKeyDown(this.connection.messageHandler, keyDownParams).Execute();
    }

    /**
     * Simulates a key up.
     */
    public void keyUp(AltKeyUpParams keyUpParams) {
        new AltKeyUp(this.connection.messageHandler, keyUpParams).Execute();
    }

    /**
     * Simulate mouse movement in your game.
     *
     * @param altMoveMouseParameters the builder for the move mouse command.
     */
    public void moveMouse(AltMoveMouseParams altMoveMouseParams) {
        new AltMoveMouse(this.connection.messageHandler, altMoveMouseParams).Execute();
    }

    /**
     * Simulate scroll action in your game.
     *
     * @param altScrollParams the builder for the scroll command.
     */
    public void scroll(AltScrollParams altScrollParams) {
        new AltScroll(this.connection.messageHandler, altScrollParams).Execute();
    }

    /**
     * @param altFindObjectsParams the builder for the find objects command.
     * @return the first object in the scene that respects the given criteria.
     */
    public AltUnityObject findObject(AltFindObjectsParams altFindObjectsParams) {
        return new AltFindObject(this.connection.messageHandler, altFindObjectsParams).Execute();
    }

    /**
     *
     * @param altFindObjectsParams
     * @return the first object containing the given criteria
     */
    public AltUnityObject findObjectWhichContains(AltFindObjectsParams altFindObjectsParams) {
        return new AltFindObjectWhichContains(this.connection.messageHandler, altFindObjectsParams).Execute();
    }

    /**
     *
     * @param altFindObjectsParams
     * @return all the objects respecting the given criteria
     */
    public AltUnityObject[] findObjects(AltFindObjectsParams altFindObjectsParams) {
        return new AltFindObjects(this.connection.messageHandler, altFindObjectsParams).Execute();
    }

    /**
     * Finds all objects in the scene that respects the given criteria.
     * 
     * @param altFindObjectsParams
     * @return all objects containing the given criteria
     */
    public AltUnityObject[] findObjectsWhichContain(AltFindObjectsParams altFindObjectsParams) {
        return new AltFindObjectsWhichContain(this.connection.messageHandler, altFindObjectsParams).Execute();
    }

    /**
     * Returns information about every objects loaded in the currently loaded
     * scenes. This also means objects that are set as DontDestroyOnLoad.
     * 
     * @param altGetAllElementsParams - get all elements parameters
     * @return information about every object loaded in the currently loaded scenes.
     */
    public AltUnityObject[] getAllElements(AltGetAllElementsParams altGetAllElementsParams) {
        return new AltGetAllElements(this.connection.messageHandler, altGetAllElementsParams).Execute();
    }

    /**
     * Waits for the scene to be loaded for a specified amount of time. It returns
     * the name of the current scene.
     * 
     * @param altWaitForCurrentSceneToBeParameters - Wait for current scene
     *                                             parameters
     * @return {String} -
     */
    public void waitForCurrentSceneToBe(AltWaitForCurrentSceneToBeParams altWaitForCurrentSceneToBeParameters) {
        new AltWaitForCurrentSceneToBe(this.connection.messageHandler, altWaitForCurrentSceneToBeParameters)
                .Execute();
    }

    /**
     * Wait until there are no longer any objects that respect the given criteria or
     * times run out and will throw an error.
     *
     * @param altWaitForObjectsParams the properties parameter for finding the
     *                                objects in a scene.
     */
    public AltUnityObject waitForObject(AltWaitForObjectsParams altWaitForObjectsParams) {
        return new AltWaitForObject(this.connection.messageHandler, altWaitForObjectsParams).Execute();
    }

    /**
     * Wait until the object in the scene that respect the given criteria is no
     * longer in the scene or times run out and will throw an error.
     *
     * @param altWaitForObjectsParams the properties parameter for finding the
     *                                objects in a scene.
     */
    public void waitForObjectToNotBePresent(AltWaitForObjectsParams altWaitForObjectsParams) {
        new AltWaitForObjectToNotBePresent(this.connection.messageHandler, altWaitForObjectsParams).Execute();
    }

    /**
     * Waits until it finds an object that respects the given criteria or time runs
     * out and will throw an error.
     */
    public AltUnityObject waitForObjectWhichContains(AltWaitForObjectsParams altWaitForObjectsParams) {
        return new AltWaitForObjectWhichContains(this.connection.messageHandler, altWaitForObjectsParams).Execute();
    }

    /**
     * Creates a screenshot of the current screen in png format.
     */
    public void getPNGScreenshot(String path) {
        new GetPNGScreenshotCommand(this.connection.messageHandler, path).Execute();
    }

    /**
     * Sets the level of logging on AltUnity Tester
     * 
     * @param parameters
     */
    public void setServerLogging(AltSetServerLoggingParams parameters) {
        new AltUnitySetServerLogging(this.connection.messageHandler, parameters).Execute();
    }

    /**
     * Simulates starting of a touch on the screen.
     */
    public int beginTouch(AltBeginTouchParams beginTouchParams) {
        return new AltBeginTouch(this.connection.messageHandler, beginTouchParams).Execute();
    }

    /**
     * Simulates a touch movement on the screen.
     */
    public void moveTouch(AltMoveTouchParams moveTouchParams) {
        new AltMoveTouch(this.connection.messageHandler, moveTouchParams).Execute();
    }

    /**
     * Simulates ending of a touch on the screen.
     */
    public void endTouch(AltEndTouchParams endTouchParams) {
        new AltEndTouch(this.connection.messageHandler, endTouchParams).Execute();
    }

    /**
     * Tap at screen coordinates.
     *
     * @param parameters Tap parameters
     */
    public void tap(AltTapClickCoordinatesParams parameters) {
        new AltTapCoordinates(this.connection.messageHandler, parameters).Execute();
    }

    /**
     * Click at screen coordinates
     *
     * @param parameters Click parameters
     */
    public void click(AltTapClickCoordinatesParams parameters) {
        new AltClickCoordinates(this.connection.messageHandler, parameters).Execute();
    }

    /**
     * Gets the value of the static field or property
     */
    public <T> T getStaticProperty(AltGetComponentPropertyParams parameters, Class<T> returnType) {
        return new AltGetStaticProperty(this.connection.messageHandler, parameters).Execute(returnType);
    }

    /**
     * Retrieves the Unity object at given coordinates
     * Uses EventSystem.RaycastAll to find object. If no object is found then it
     * uses UnityEngine.Physics.Raycast and UnityEngine.Physics2D.Raycast and
     * returns the one closer to the camera.
     * 
     * @param coordinates The screen coordinates
     * @return The UI object hit by event system Raycast, null otherwise
     */

    public AltUnityObject findObjectAtCoordinates(AltFindObjectAtCoordinatesParams parameters) {
        return new AltFindObjectAtCoordinates(this.connection.messageHandler, parameters).Execute();
    }

    public void addNotification(AltUnityAddNotificationListenerParams parameters) {
        new AltUnityAddNotificationListener(this.connection.messageHandler, parameters).Execute();
    }

    public void removeNotificationListener(AltUnityRemoveNotificationListenerParams notificationType) {

        new AltUnityRemoveNotificationListener(this.connection.messageHandler, notificationType).Execute();

    }

    public enum By {
        TAG, LAYER, NAME, COMPONENT, PATH, ID, TEXT
    }
}
