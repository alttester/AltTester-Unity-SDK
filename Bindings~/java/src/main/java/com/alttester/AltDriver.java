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

import java.io.IOException;

import org.apache.logging.log4j.Logger;
import org.apache.logging.log4j.core.config.ConfigurationFactory;
import org.apache.logging.log4j.LogManager;

import com.alttester.Commands.*;
import com.alttester.Commands.AltCommands.AltSetServerLoggingParams;
import com.alttester.Commands.AltCommands.AltAddNotificationListener;
import com.alttester.Commands.AltCommands.AltAddNotificationListenerParams;
import com.alttester.Commands.AltCommands.AltRemoveNotificationListener;
import com.alttester.Commands.AltCommands.AltRemoveNotificationListenerParams;
import com.alttester.Commands.AltCommands.AltResetInput;
import com.alttester.Commands.AltCommands.AltSetServerLogging;
import com.alttester.Commands.FindObject.*;
import com.alttester.Commands.InputActions.*;
import com.alttester.Commands.UnityCommand.*;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.Commands.ObjectCommand.AltSetComponentPropertyParams;
import com.alttester.UnityStruct.AltKeyCode;
import com.alttester.altTesterExceptions.*;

public class AltDriver {
    private static final Logger logger = LogManager.getLogger(AltDriver.class);
    public static final String VERSION = "2.2.2";

    static {
        ConfigurationFactory custom = new AltDriverConfigFactory();
        ConfigurationFactory.setConfigurationFactory(custom);
    }

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

    private WebsocketConnection connection = null;

    public AltDriver() {
        this("127.0.0.1", 13000, false, 60, "__default__", "unknown", "unknown", "unknown", "unknown");
    }

    public AltDriver(String host, int port) {
        this(host, port, false, 60, "__default__", "unknown", "unknown", "unknown", "unknown");
    }

    public AltDriver(String host, int port, Boolean enableLogging) {
        this(host, port, enableLogging, 60, "__default__", "unknown", "unknown", "unknown", "unknown");
    }

    public AltDriver(String host, int port, Boolean enableLogging, int connectTimeout) {
        this(host, port, enableLogging, connectTimeout, "__default__", "unknown", "unknown", "unknown", "unknown");
    }

    public AltDriver(String host, int port, Boolean enableLogging, int connectTimeout, String appName) {
        this(host, port, enableLogging, connectTimeout, appName, "unknown", "unknown", "unknown", "unknown");
    }

    public AltDriver(String host, int port, Boolean enableLogging, int connectTimeout, String appName, String platform,
            String platformVersion, String deviceInstanceId, String appId) {
        if (!enableLogging) {
            AltDriverConfigFactory.DisableLogging();
        }

        if (host == null || host.isEmpty()) {
            throw new InvalidParameterException("Provided host address is null or empty.");
        }

        logger.debug("Connecting to AltTester(R) on host: '{}', port: '{}' and appName: '{}'.", host, port, appName);
        this.connection = new WebsocketConnection(host, port, appName, connectTimeout, platform, platformVersion,
                deviceInstanceId, appId);
        this.connection.connect();

        checkServerVersion();
    }

    public int[] getApplicationScreenSize() {

        AltCallStaticMethodParams altCallStaticMethodParamsWidth = new AltCallStaticMethodParams.Builder(
                "UnityEngine.Screen", "get_width",
                "UnityEngine.CoreModule", new Object[] {})
                .build();
        int screenWidth = callStaticMethod(altCallStaticMethodParamsWidth,
                Integer.class);
        AltCallStaticMethodParams altCallStaticMethodParamsHeight = new AltCallStaticMethodParams.Builder(
                "UnityEngine.Screen", "get_height",
                "UnityEngine.CoreModule", new Object[] {})
                .build();
        int screenHeight = callStaticMethod(altCallStaticMethodParamsHeight,
                Integer.class);

        return new int[] { screenWidth, screenHeight };
    }

    private String[] splitVersion(String version) {
        String[] parts = version.split("\\.");
        return new String[] { parts[0], (parts.length > 1) ? parts[1] : "" };
    }

    private void checkServerVersion() {
        String serverVersion = getServerVersion();

        String[] serverParts = splitVersion(serverVersion);
        int serverMajor = Integer.parseInt(serverParts[0]);
        int serverMinor = Integer.parseInt(serverParts[1]);

        boolean isSupported = (serverMajor == 2 && serverMinor == 2) || (serverMajor == 1 && serverMinor == 0);

        if (!isSupported) {
            String message = String.format(
                    "Version mismatch. AltDriver version is %s. AltTester(R) version is %s.",
                    AltDriver.VERSION, serverVersion);
            logger.warn(message);
            System.out.println(message);
        }
    }

    /**
     * Closes the connection to the running instrumented app.
     *
     * @throws IOException
     */
    public void stop() throws IOException {
        this.connection.close();
    }

    /**
     * Gets the AltTester® version, used to instrument the app.
     *
     * @return AltTester® version
     */
    public String getServerVersion() {
        return new GetServerVersionCommand(this.connection.messageHandler).Execute();
    }

    /**
     * Gets the delay after a command.
     *
     * @return The delay after a command
     */
    public double getDelayAfterCommand() {
        return this.connection.messageHandler.getDelayAfterCommand();
    }

    /**
     * Sets the delay after a command.
     *
     * @param delay - Double
     */
    public void setDelayAfterCommand(double delay) {
        this.connection.messageHandler.setDelayAfterCommand(delay);
    }

    /**
     * Loads a scene.
     *
     * @param altLoadSceneParameters - scene name
     */
    public void loadScene(AltLoadSceneParams altLoadSceneParameters) {
        new AltLoadScene(this.connection.messageHandler, altLoadSceneParameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Unloads a scene.
     *
     * @param unloadSceneParams - scene name
     */
    public void unloadScene(AltUnloadSceneParams unloadSceneParams) {
        new AltUnloadScene(this.connection.messageHandler, unloadSceneParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Returns all the scenes that have been loaded.
     *
     * @return All the scenes that have been loaded
     */
    public String[] getAllLoadedScenes() {
        String[] response = new AltGetAllLoadedScenes(this.connection.messageHandler).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets the value for the command response timeout.
     *
     * @param timeout - int
     */
    public void setCommandResponseTimeout(int timeout) {
        this.connection.messageHandler.setCommandTimeout(timeout);
    }

    /**
     * Removes all keys and values from PlayerPref.
     */
    public void deletePlayerPref() {
        new AltDeletePlayerPref(this.connection.messageHandler).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Removes key and its corresponding value from PlayerPrefs.
     *
     * @param keyName - String
     */
    public void deleteKeyPlayerPref(String keyName) {
        new AltDeleteKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Sets the value for a given key in PlayerPrefs.
     *
     * @param keyName   - String
     * @param valueName - int
     */
    public void setKeyPlayerPref(String keyName, int valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Sets the value for a given key in PlayerPrefs.
     *
     * @param keyName   - String
     * @param valueName - float
     */
    public void setKeyPlayerPref(String keyName, float valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Sets the value for a given key in PlayerPrefs.
     *
     * @param keyName   - String
     * @param valueName - String
     */
    public void setKeyPlayerPref(String keyName, String valueName) {
        new AltSetKeyPlayerPref(this.connection.messageHandler, keyName, valueName).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Returns the value for a given key from PlayerPrefs.
     *
     * @param keyName -String
     * @return The value for a given key from PlayerPrefs
     */
    public int getIntKeyPlayerPref(String keyName) {
        int response = new AltIntGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns the value for a given key from PlayerPrefs.
     *
     * @param keyName - String
     * @return The value for a given key from PlayerPrefs
     */
    public float getFloatKeyPlayerPref(String keyName) {
        float response = new AltFloatGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns the value for a given key from PlayerPrefs.
     *
     * @param keyName - String
     * @return The value for a given key from PlayerPrefs
     */
    public String getStringKeyPlayerPref(String keyName) {
        String response = new AltStringGetKeyPlayerPref(this.connection.messageHandler, keyName).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns the current active scene.
     *
     * @return Current active scene
     */
    public String getCurrentScene() {
        String response = new AltGetCurrentScene(this.connection.messageHandler).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns the value of the time scale.
     *
     * @return Timescale
     */
    public float getTimeScale() {
        float response = new AltGetTimeScale(this.connection.messageHandler).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets the value of the time scale.
     *
     * @param setTimescaleParams - timescale
     */
    public void setTimeScale(AltSetTimeScaleParams setTimescaleParams) {
        new AltSetTimeScale(this.connection.messageHandler, setTimescaleParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Invokes static methods from your application.
     *
     * @param altCallStaticMethodParams - String component* , String method* ,
     *                                  Object[] parameters* , String[]
     *                                  typeOfParameters , String assembly
     * @param returnType
     * @return Static methods from your application
     */
    public <T> T callStaticMethod(AltCallStaticMethodParams altCallStaticMethodParams, Class<T> returnType) {
        T response = new AltCallStaticMethod(this.connection.messageHandler, altCallStaticMethodParams)
                .Execute(returnType);
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Simulates a swipe action between two points.
     *
     * @param swipeParams - Vector2 start* , Vector2 end* , float duration , boolean
     *                    wait = true
     */
    public void swipe(AltSwipeParams swipeParams) {
        new AltSwipe(this.connection.messageHandler, swipeParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulates a multipoint swipe action.
     *
     * @param parameters - positions[]* , float duration , boolean wait
     */
    public void multipointSwipe(AltMultiPointSwipeParams parameters) {
        new AltMultiPointSwipe(this.connection.messageHandler, parameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulates holding left click button down for a specified amount of time at
     * given coordinates.
     *
     * @param holdParams - Vector2 coordinates* , float duration , boolean wait
     */
    public void holdButton(AltHoldParams holdParams) {
        swipe(holdParams);
    }

    /**
     * Simulates device rotation action in your application.
     *
     * @param altTiltParameter - Vector3 acceleration* , float duration , boolean
     *                         wait
     */
    public void tilt(AltTiltParams altTiltParameter) {
        new AltTilt(this.connection.messageHandler, altTiltParameter).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulates key press action in your application.
     *
     * @param altPressKeyParameters - AltKeyCode keyCode* , float power , float
     *                              duration , boolean wait
     */
    public void pressKey(AltPressKeyParams altPressKeyParameters) {
        AltKeyCode[] keyCodes = { altPressKeyParameters.getKeyCode() };
        AltPressKeysParams params = new AltPressKeysParams.Builder(keyCodes).withPower(altPressKeyParameters.getPower())
                .withDuration(altPressKeyParameters.getDuration()).withWait(altPressKeyParameters.getWait()).build();
        this.pressKeys(params);
    }

    /**
     * Simulates multiple keys pressed action in your application.
     *
     * @param altPressKeysParameters - AltKeyCode[] keyCodes* , float power ,
     *                               float duration , boolean wait
     */
    public void pressKeys(AltPressKeysParams altPressKeysParameters) {
        new AltPressKeys(this.connection.messageHandler, altPressKeysParameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulates a key down.
     *
     * @param keyDownParams - AltKeyCode keyCode* , float power
     * @throws InterruptedException
     */
    public void keyDown(AltKeyDownParams keyDownParams) throws InterruptedException {
        AltKeyCode[] keys = { keyDownParams.getKeyCode() };
        AltKeysDownParams params = new AltKeysDownParams.Builder(keys).withPower(keyDownParams.getPower()).build();
        this.keysDown(params);
    }

    /**
     * Simulates multiple keys down.
     *
     * @param keysDownParams - AltKeyCode keyCode* , float power
     */
    public void keysDown(AltKeysDownParams keysDownParams) {
        new AltKeysDown(this.connection.messageHandler, keysDownParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulates a key up.
     *
     * @param keyUpParams - keyCode
     */
    public void keyUp(AltKeyUpParams keyUpParams) {
        AltKeyCode[] keyCodes = { keyUpParams.getKeyCode() };
        AltKeysUpParams params = new AltKeysUpParams.Builder(keyCodes).build();
        this.keysUp(params);
    }

    /**
     * Simulates multiple keys up.
     *
     * @param keysUpParams - AltKeyCode[] keyCodes
     */
    public void keysUp(AltKeysUpParams keysUpParams) {
        new AltKeysUp(this.connection.messageHandler, keysUpParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulate mouse movement in your application.
     *
     * @param altMoveMouseParams - Vector2 coordinates* , float duration , boolean
     *                           wait
     */
    public void moveMouse(AltMoveMouseParams altMoveMouseParams) {
        new AltMoveMouse(this.connection.messageHandler, altMoveMouseParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulate scroll action in your application.
     *
     * @param altScrollParams - float speed , float speedHorizontal , float duration
     *                        , boolean wait
     */
    public void scroll(AltScrollParams altScrollParams) {
        new AltScroll(this.connection.messageHandler, altScrollParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * @param altFindObjectsParams - By by* , String value* , By cameraBy , String
     *                             cameraValue , boolean enabled
     * @return The first object in the scene that respects the given criteria.
     */
    public AltObject findObject(AltFindObjectsParams altFindObjectsParams) {
        AltObject response = new AltFindObject(this.connection.messageHandler, altFindObjectsParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     *
     * @param altFindObjectsParams - By by* , String value* , By cameraBy , String
     *                             cameraValue , boolean enabled
     * @return The first object containing the given criteria
     */
    public AltObject findObjectWhichContains(AltFindObjectsParams altFindObjectsParams) {
        AltObject response = new AltFindObjectWhichContains(this.connection.messageHandler, altFindObjectsParams)
                .Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     *
     * @param altFindObjectsParams - By by* , String value* , By cameraBy , String
     *                             cameraValue , boolean enabled
     * @return All the objects respecting the given criteria
     */
    public AltObject[] findObjects(AltFindObjectsParams altFindObjectsParams) {
        AltObject[] response = new AltFindObjects(this.connection.messageHandler, altFindObjectsParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Finds all objects in the scene that respects the given criteria.
     *
     * @param altFindObjectsParams - By by* , String value* , By cameraBy , String
     *                             cameraValue , boolean enabled
     * @return All objects containing the given criteria
     */
    public AltObject[] findObjectsWhichContain(AltFindObjectsParams altFindObjectsParams) {
        AltObject[] response = new AltFindObjectsWhichContain(this.connection.messageHandler, altFindObjectsParams)
                .Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns information about every objects loaded in the currently loaded
     * scenes. This also means objects that are set as DontDestroyOnLoad.
     *
     * @param altGetAllElementsParams - By cameraBy , String cameraValue , boolean
     *                                enabled
     * @return Information about every object loaded in the currently loaded scenes.
     */
    public AltObject[] getAllElements(AltGetAllElementsParams altGetAllElementsParams) {
        AltObject[] response = new AltGetAllElements(this.connection.messageHandler, altGetAllElementsParams)
                .Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Waits for the scene to be loaded for a specified amount of time. It returns
     * the name of the current scene.
     *
     * @param altWaitForCurrentSceneToBeParameters - String sceneName* , double
     *                                             timeout , double interval
     */
    public void waitForCurrentSceneToBe(AltWaitForCurrentSceneToBeParams altWaitForCurrentSceneToBeParameters) {
        new AltWaitForCurrentSceneToBe(this.connection.messageHandler, altWaitForCurrentSceneToBeParameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Wait until there are no longer any objects that respect the given criteria or
     * times run out and will throw an error.
     *
     * @param altWaitForObjectsParams - AltFindObjectsParams
     *                                altFindObjectsParameters* , double timeout ,
     *                                double interval
     * @return Error if time runs out
     */
    public AltObject waitForObject(AltWaitForObjectsParams altWaitForObjectsParams) {
        AltObject response = new AltWaitForObject(this.connection.messageHandler, altWaitForObjectsParams)
                .Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Wait until the object in the scene that respect the given criteria is no
     * longer in the scene or times run out and will throw an error.
     *
     * @param altWaitForObjectsParams - AltFindObjectsParams
     *                                altFindObjectsParameters* , double timeout ,
     *                                double interval
     */
    public void waitForObjectToNotBePresent(AltWaitForObjectsParams altWaitForObjectsParams) {
        new AltWaitForObjectToNotBePresent(this.connection.messageHandler, altWaitForObjectsParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Waits until it finds an object that respects the given criteria or time runs
     * out and will throw an error.
     *
     * @param altWaitForObjectsParams - AltFindObjectsParams
     *                                altFindObjectsParameters* , double timeout ,
     *                                double interval
     * @return The object that respects the given criteria/Error if time runs out
     */
    public AltObject waitForObjectWhichContains(AltWaitForObjectsParams altWaitForObjectsParams) {
        AltObject response = new AltWaitForObjectWhichContains(this.connection.messageHandler,
                altWaitForObjectsParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Creates a screenshot of the current screen in png format.
     *
     * @param path - String
     */
    public void getPNGScreenshot(String path) {
        new GetPNGScreenshotCommand(this.connection.messageHandler, path).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Sets the level of logging on AltTester.
     *
     * @param parameters - AltLogger logger* , AltLogLevel logLevel*
     */
    public void setServerLogging(AltSetServerLoggingParams parameters) {
        new AltSetServerLogging(this.connection.messageHandler, parameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Simulates starting of a touch on the screen.
     *
     * @param beginTouchParams - Vector2 coordinates*
     * @return The starting of a touch on the screen
     */
    public int beginTouch(AltBeginTouchParams beginTouchParams) {
        int response = new AltBeginTouch(this.connection.messageHandler, beginTouchParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Simulates a touch movement on the screen.
     *
     * @param moveTouchParams - int fingerId* , Vector2 coordinates*
     */
    public void moveTouch(AltMoveTouchParams moveTouchParams) {
        new AltMoveTouch(this.connection.messageHandler, moveTouchParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());

    }

    /**
     * Simulates ending of a touch on the screen.
     *
     * @param endTouchParams - int fingerId*
     */
    public void endTouch(AltEndTouchParams endTouchParams) {
        new AltEndTouch(this.connection.messageHandler, endTouchParams).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Tap at screen coordinates.
     *
     * @param parameters - Vector2 coordinates* , int count , float interval ,
     *                   boolean wait
     */
    public void tap(AltTapClickCoordinatesParams parameters) {
        new AltTapCoordinates(this.connection.messageHandler, parameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Click at screen coordinates.
     *
     * @param parameters - Vector2 coordinates* , int count , float interval ,
     *                   boolean wait
     */
    public void click(AltTapClickCoordinatesParams parameters) {
        new AltClickCoordinates(this.connection.messageHandler, parameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Gets the value of the static field or property.
     *
     * @param parameters - String componentName* , String propertyName* , String
     *                   assembly , int maxDept
     * @param returnType
     * @return value of the static field or property
     */
    public <T> T getStaticProperty(AltGetComponentPropertyParams parameters, Class<T> returnType) {
        T response = new AltGetStaticProperty(this.connection.messageHandler, parameters).Execute(returnType);
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets the value of the static field or property.
     *
     * @param parameters - String componentName* , String propertyName* , String
     *                   assembly
     */
    public void setStaticProperty(AltSetComponentPropertyParams parameters) {
        new AltSetStaticProperty(this.connection.messageHandler, parameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
    }

    /**
     * Retrieves the Unity object at given coordinates.
     * Uses EventSystem.RaycastAll to find object. If no object is found then it
     * uses UnityEngine.Physics.Raycast and UnityEngine.Physics2D.Raycast and
     * returns the one closer to the camera.
     *
     * @param parameters - Vector2 coordinates
     * @return The UI object hit by event system Raycast, null otherwise
     */
    public AltObject findObjectAtCoordinates(AltFindObjectAtCoordinatesParams parameters) {
        AltObject response = new AltFindObjectAtCoordinates(this.connection.messageHandler, parameters).Execute();
        Utils.sleepFor(this.connection.messageHandler.getDelayAfterCommand());
        return response;
    }

    public void addNotification(AltAddNotificationListenerParams parameters) {
        new AltAddNotificationListener(this.connection.messageHandler, parameters).Execute();
    }

    public void removeNotificationListener(AltRemoveNotificationListenerParams notificationType) {
        new AltRemoveNotificationListener(this.connection.messageHandler, notificationType).Execute();
    }

    /**
     * Clears all active input simulated by AltTester.
     */
    public void resetInput() {
        new AltResetInput(this.connection.messageHandler).Execute();
    }

    public enum By {
        TAG, LAYER, NAME, COMPONENT, PATH, ID, TEXT
    }
}
