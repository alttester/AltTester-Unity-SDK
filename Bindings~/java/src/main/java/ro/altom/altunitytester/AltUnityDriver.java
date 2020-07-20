package ro.altom.altunitytester;

import org.apache.log4j.BasicConfigurator;
import org.slf4j.LoggerFactory;
import ro.altom.altunitytester.Commands.*;
import ro.altom.altunitytester.Commands.FindObject.*;
import ro.altom.altunitytester.Commands.InputActions.*;
import ro.altom.altunitytester.Commands.OldFindObject.*;
import ro.altom.altunitytester.Commands.UnityCommand.*;
import ro.altom.altunitytester.altUnityTesterExceptions.*;
import ro.altom.altunitytester.position.Vector2;

import java.io.*;
import java.net.Socket;
import java.util.List;

import org.apache.log4j.Logger;
import org.apache.log4j.LogManager;

public class AltUnityDriver {

    private static final Logger log = LogManager.getLogger(AltUnityDriver.class);

    public static class PlayerPrefsKeyType {
        public static int IntType = 1;
        public static int StringType = 2;
        public static int FloatType = 3;
    }

    public static final String VERSION = "1.5.6";
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
        BasicConfigurator.configure();
    }

    public AltUnityDriver(String ip, int port, String requestSeparator, String requestEnd) {
        this(ip, port, requestSeparator, requestEnd, false);
        BasicConfigurator.configure();
    }

    public AltUnityDriver(String ip, int port, String requestSeparator, String requestEnd, Boolean logEnabled) {
        BasicConfigurator.configure();
        if (ip == null || ip.isEmpty()) {
            throw new InvalidParamerException("Provided IP address is null or empty");
        }
        int timeout = 60;
        while (timeout > 0) {
            try {
                try {
                    log.info(String.format("Initializing connection to %s:%d", ip, port));
                    socket = new Socket(ip, port);
                    socket.setSoTimeout(READ_TIMEOUT);
                    out = new PrintWriter(socket.getOutputStream(), true);
                    in = new DataInputStream(socket.getInputStream());
                } catch (IOException e) {
                    throw new ConnectionException("AltUnityServer not running on port " + port
                            + ",retrying (timing out in " + timeout + " secs)...", e);
                }
                altBaseSettings = new AltBaseSettings(socket, requestSeparator, requestEnd, out, in, logEnabled);
                GetServerVersion();
                EnableLogging();
                break;
            } catch (Exception e) {
                if (socket != null)
                    stop();
                System.out.println(e.getMessage());
                System.out.println("AltUnityServer not running on port " + port + ", retrying (timing out in " + timeout
                        + " secs)...");
                timeout -= 5;
                try {
                    Thread.sleep(5000);
                } catch (InterruptedException e1) {
                    e1.printStackTrace();
                }

            }
        }
        if (timeout <= 0) {
            throw new ConnectionException("Could not create connection to " + String.format("%s:%d", ip, port),
                    new Throwable());
        }

    }

    private String GetServerVersion() {
        try {
            new GetServerVersionCommand(altBaseSettings).Execute();
            return "Ok";
        } catch (Exception e) {
            log.warn(e.getMessage());
            return "Version mismatch";
        }
    }

    private void EnableLogging() {
        new EnableLogging(altBaseSettings).Execute();
    }

    public void stop() {
        new AltStop(altBaseSettings).Execute();
    }

    public String callStaticMethods(AltCallStaticMethodsParameters altCallStaticMethodsParameters) {
        return new AltCallStaticMethods(altBaseSettings, altCallStaticMethodsParameters).Execute();
    }

    public String callStaticMethods(String assembly, String typeName, String methodName, String parameters,
            String typeOfParameters) {
        AltCallStaticMethodsParameters altCallStaticMethodsParameters = new AltCallStaticMethodsParameters.Builder(
                typeName, methodName, parameters).withAssembly(assembly).withTypeOfParameters(typeOfParameters).build();
        return callStaticMethods(altCallStaticMethodsParameters);
    }

    public String callStaticMethods(String typeName, String methodName, String parameters) {
        return callStaticMethods("", typeName, methodName, parameters, "");
    }

    public void loadScene(String scene) {
        new AltLoadScene(altBaseSettings, scene).Execute();
    }
	
	/**
	 * Ability to access altBaseSettings.
	 * @return Returns the AltBaseSettings used by the driver.
     */
	public AltBaseSettings GetAltBaseSettings()
	{
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

    @Deprecated
    public AltUnityObject findObject(By by, String value, By cameraBy, String cameraPath, boolean enabled) {
        return findObject(BuildFindObjectsParameters(by, value, cameraBy, cameraPath, enabled));
    }

    @Deprecated
    public AltUnityObject findObject(By by, String value, boolean enabled) {
        return findObject(by, value, By.NAME, "", enabled);
    }

    @Deprecated
    public AltUnityObject findObject(By by, String value, String cameraName) {
        return findObject(by, value, By.NAME, cameraName, true);
    }

    @Deprecated
    public AltUnityObject findObject(By by, String value) {
        return findObject(by, value, By.NAME, "", true);
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

    @Deprecated
    public AltUnityObject findObjectWhichContains(By by, String value, boolean enabled) {
        return findObjectWhichContains(by, value, By.NAME, "", enabled);
    }

    @Deprecated
    public AltUnityObject findObjectWhichContains(By by, String value, String cameraName) {
        return findObjectWhichContains(by, value, By.NAME, cameraName, true);
    }

    @Deprecated
    public AltUnityObject findObjectWhichContains(By by, String value) {
        return findObjectWhichContains(by, value, By.NAME, "", true);
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

    @Deprecated
    public AltUnityObject[] findObjects(By by, String value, String cameraName) {
        return findObjects(by, value, By.NAME, cameraName, true);
    }

    @Deprecated
    public AltUnityObject[] findObjects(By by, String value, boolean enabled) {
        return findObjects(by, value, By.NAME, "", enabled);
    }

    @Deprecated
    public AltUnityObject[] findObjects(By by, String value) {
        return findObjects(by, value, By.NAME, "", true);
    }

    /**
     *
     * @param altFindObjectsParameters
     * @return all objects containing the given criteria
     */
    public AltUnityObject[] findObjectsWhichContains(AltFindObjectsParameters altFindObjectsParameters) {
        return new AltFindObjectsWhichContains(altBaseSettings, altFindObjectsParameters).Execute();
    }

    public AltUnityObject[] findObjectsWhichContains(By by, String value, By cameraBy, String cameraPath,
            boolean enabled) {
        return findObjectsWhichContains(BuildFindObjectsParameters(by, value, cameraBy, cameraPath, enabled));
    }

    @Deprecated
    public AltUnityObject[] findObjectsWhichContains(By by, String value, String cameraName) {
        return findObjectsWhichContains(by, value, By.NAME, cameraName, true);
    }

    @Deprecated
    public AltUnityObject[] findObjectsWhichContains(By by, String value, boolean enabled) {
        return findObjectsWhichContains(by, value, By.NAME, "", enabled);
    }

    @Deprecated
    public AltUnityObject[] findObjectsWhichContains(By by, String value) {
        return findObjectsWhichContains(by, value, By.NAME, "", true);
    }

    @Deprecated
    public AltUnityObject findElementWhereNameContains(AltFindElementsParameters altFindElementsParameters) {
        return new AltFindElementWhereNameContains(altBaseSettings, altFindElementsParameters).Execute();
    }

    @Deprecated
    public AltUnityObject findElementWhereNameContains(String name, String cameraName, boolean enabled) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).isEnabled(enabled).build();
        return findElementWhereNameContains(altFindElementsParameters);
    }

    @Deprecated
    public AltUnityObject findElementWhereNameContains(String name, String cameraName) {
        return findElementWhereNameContains(name, cameraName, true);
    }

    @Deprecated
    public AltUnityObject findElementWhereNameContains(String name, boolean enabled) {
        return findElementWhereNameContains(name, "", enabled);
    }

    @Deprecated
    public AltUnityObject findElementWhereNameContains(String name) {
        return findElementWhereNameContains(name, "");
    }

    /**
     * 
     * @param altGetAllElementsParameters
     * @return information about every object loaded in the currently loaded scenes.
     */
    public AltUnityObject[] getAllElements(AltGetAllElementsParameters altGetAllElementsParameters) {
        return new AltGetAllElements(altBaseSettings, altGetAllElementsParameters).Execute();
    }

    @Deprecated
    public AltUnityObject[] getAllElements(By cameraBy, String cameraName, boolean enabled) {
        AltGetAllElementsParameters altGetAllElementsParameters = new AltGetAllElementsParameters.Builder()
                .withCamera(cameraBy, cameraName).isEnabled(enabled).build();
        return getAllElements(altGetAllElementsParameters);
    }

    @Deprecated
    public AltUnityObject[] getAllElements(String cameraName) {
        return getAllElements(By.NAME, cameraName, true);
    }

    @Deprecated
    public AltUnityObject[] getAllElements(boolean enabled) {
        return getAllElements(By.NAME, "", enabled);
    }

    @Deprecated
    public AltUnityObject[] getAllElements() throws Exception {
        return getAllElements(By.NAME, "", true);
    }

    @Deprecated
    public AltUnityObject findElement(AltFindElementsParameters altFindElementsParameters) {
        return new AltFindElement(altBaseSettings, altFindElementsParameters).Execute();
    }

    @Deprecated
    public AltUnityObject findElement(String name, String cameraName, boolean enabled) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .isEnabled(enabled).withCamera(cameraName).build();
        return findElement(altFindElementsParameters);
    }

    @Deprecated
    public AltUnityObject findElement(String name, boolean enabled) {
        return findElement(name, "", enabled);
    }

    @Deprecated
    public AltUnityObject findElement(String name, String cameraName) {
        return findElement(name, cameraName, true);
    }

    @Deprecated
    public AltUnityObject findElement(String name) {
        return findElement(name, "", true);
    }

    @Deprecated
    public AltUnityObject[] findElements(AltFindElementsParameters altFindElementsParameters) {
        return new AltFindElements(altBaseSettings, altFindElementsParameters).Execute();
    }

    @Deprecated
    public AltUnityObject[] findElements(String name, String cameraName, boolean enabled) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).isEnabled(enabled).build();
        return findElements(altFindElementsParameters);
    }

    @Deprecated
    public AltUnityObject[] findElements(String name) {
        return findElements(name, "", true);
    }

    @Deprecated
    public AltUnityObject[] findElements(String name, String cameraName) {
        return findElements(name, cameraName, true);
    }

    @Deprecated
    public AltUnityObject[] findElements(String name, boolean enabled) {
        return findElements(name, "", enabled);
    }

    @Deprecated
    public AltUnityObject[] findElementsWhereNameContains(AltFindElementsParameters altFindElementsParameters) {
        return new AltFindElementsWhereNameContains(altBaseSettings, altFindElementsParameters).Execute();
    }

    @Deprecated
    public AltUnityObject[] findElementsWhereNameContains(String name, String cameraName, boolean enabled) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).isEnabled(enabled).build();
        return findElementsWhereNameContains(altFindElementsParameters);
    }

    @Deprecated
    public AltUnityObject[] findElementsWhereNameContains(String name, String cameraName) {
        return findElementsWhereNameContains(name, cameraName, true);
    }

    @Deprecated
    public AltUnityObject[] findElementsWhereNameContains(String name, boolean enabled) {
        return findElementsWhereNameContains(name, "", enabled);
    }

    @Deprecated
    public AltUnityObject[] findElementsWhereNameContains(String name) {
        return findElementsWhereNameContains(name, "", true);
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

    @Deprecated
    public String waitForCurrentSceneToBe(String sceneName, double timeout, double interval) {
        AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters = new AltWaitForCurrentSceneToBeParameters.Builder(
                sceneName).withInterval(interval).withTimeout(timeout).build();
        return waitForCurrentSceneToBe(altWaitForCurrentSceneToBeParameters);
    }

    public String waitForCurrentSceneToBe(String sceneName) {
        return waitForCurrentSceneToBe(sceneName, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElementWhereNameContains(AltWaitForElementParameters altWaitForElementParameters) {
        return new AltWaitForElementWhereNameContains(altBaseSettings, altWaitForElementParameters).Execute();
    }

    @Deprecated
    public AltUnityObject waitForElementWhereNameContains(String name, String cameraName, boolean enabled,
            double timeout, double interval) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).isEnabled(enabled).build();
        AltWaitForElementParameters altWaitForElementParameters = new AltWaitForElementParameters.Builder(
                altFindElementsParameters).withInterval(interval).withTimeout(timeout).build();
        return waitForElementWhereNameContains(altWaitForElementParameters);
    }

    @Deprecated
    public AltUnityObject waitForElementWhereNameContains(String name, String cameraName, double timeout,
            double interval) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).build();
        AltWaitForElementParameters altWaitForElementParameters = new AltWaitForElementParameters.Builder(
                altFindElementsParameters).withInterval(interval).withTimeout(timeout).build();
        return waitForElementWhereNameContains(altWaitForElementParameters);
    }

    @Deprecated
    public AltUnityObject waitForElementWhereNameContains(String name) {
        return waitForElementWhereNameContains(name, "", true, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElementWhereNameContains(String name, boolean enabled) {
        return waitForElementWhereNameContains(name, "", enabled, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElementWhereNameContains(String name, String cameraName) {
        return waitForElementWhereNameContains(name, cameraName, true, 20, 0.5);
    }

    @Deprecated
    public void waitForElementToNotBePresent(AltWaitForElementParameters altWaitForElementParameters) {
        new AltWaitForElementToNotBePresent(altBaseSettings, altWaitForElementParameters).Execute();
    }

    @Deprecated
    public void waitForElementToNotBePresent(String name, String cameraName, boolean enabled, double timeout,
            double interval) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).isEnabled(enabled).build();
        AltWaitForElementParameters altWaitForElementParameters = new AltWaitForElementParameters.Builder(
                altFindElementsParameters).withTimeout(timeout).withInterval(interval).build();
        waitForElementToNotBePresent(altWaitForElementParameters);
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
    public AltUnityObject waitForObject(By by, String value, By cameraBy, String cameraPath, boolean enabled,
            double timeout, double interval) {
        return waitForObject(
                BuildWaitForObjectsParameters(by, value, cameraBy, cameraPath, enabled, timeout, interval));
    }

    @Deprecated
    public AltUnityObject waitForObject(By by, String value) {
        return waitForObject(by, value, By.NAME, "", true, 2, 0.5);
    }

    public AltUnityObject waitForObjectWithText(AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters) {
        return new AltWaitForObjectWithText(altBaseSettings, altWaitForObjectWithTextParameters).Execute();
    }

    @Deprecated
    public AltUnityObject waitForObjectWithText(By by, String value, String text, By cameraBy, String cameraPath,
            boolean enabled, double timeout, double interval) {
        AltFindObjectsParameters altFindElementsParameters = BuildFindObjectsParameters(by, value, cameraBy, cameraPath,
                enabled);
        AltWaitForObjectWithTextParameters altWaitForElementWithTextParameters = new AltWaitForObjectWithTextParameters.Builder(
                altFindElementsParameters, text).withInterval(interval).withTimeout(timeout).build();
        return waitForObjectWithText(altWaitForElementWithTextParameters);
    }

    @Deprecated
    public AltUnityObject waitForObjectWithText(By by, String value, String text) {
        return waitForObjectWithText(by, value, text, By.NAME, "", true, 2, 0.5);
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

    @Deprecated
    public void waitForObjectToNotBePresent(By by, String value, By cameraBy, String cameraPath, boolean enabled,
            double timeout, double interval) {
        AltFindObjectsParameters altFindObjectsParameters = BuildFindObjectsParameters(by, value, cameraBy, cameraPath,
                enabled);
        AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                altFindObjectsParameters).withTimeout(timeout).withInterval(interval).build();
        waitForObjectToNotBePresent(altWaitForObjectsParameters);
    }

    @Deprecated
    public void waitForObjectToNotBePresent(By by, String value) {
        waitForObjectToNotBePresent(by, value, By.NAME, "", true, 20, 0.5);
    }

    public AltUnityObject waitForObjectWhichContains(AltWaitForObjectsParameters altWaitForObjectsParameters) {
        return new AltWaitForObjectWhichContains(altBaseSettings, altWaitForObjectsParameters).Execute();
    }

    @Deprecated
    public AltUnityObject waitForObjectWhichContains(By by, String value, By cameraBy, String cameraName,
            boolean enabled, double timeout, double interval) {
        return waitForObjectWhichContains(
                BuildWaitForObjectsParameters(by, value, cameraBy, cameraName, enabled, timeout, interval));
    }

    @Deprecated
    public AltUnityObject waitForObjectWhichContains(By by, String value) {
        return waitForObjectWhichContains(by, value, By.NAME, "", true, 30, 0.5);
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

    private AltWaitForObjectsParameters BuildWaitForObjectsParameters(By by, String value, By cameraBy,
            String cameraName, boolean enabled, double timeout, double interval) {
        return new AltWaitForObjectsParameters.Builder(
                BuildFindObjectsParameters(by, value, cameraBy, cameraName, enabled)).withInterval(interval)
                        .withTimeout(timeout).build();
    }

    @Deprecated
    public void waitForElementToNotBePresent(String name) {
        waitForElementToNotBePresent(name, "", true, 20, 0.5);
    }

    @Deprecated
    public void waitForElementToNotBePresent(String name, String cameraName) {
        waitForElementToNotBePresent(name, cameraName, true, 20, 0.5);
    }

    @Deprecated
    public void waitForElementToNotBePresent(String name, boolean enabled) {
        waitForElementToNotBePresent(name, "", enabled, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElement(AltWaitForElementParameters altWaitForElementParameters) {
        return new AltWaitForElement(altBaseSettings, altWaitForElementParameters).Execute();
    }

    @Deprecated
    public AltUnityObject waitForElement(String name, String cameraName, boolean enabled, double timeout,
            double interval) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).isEnabled(enabled).build();
        AltWaitForElementParameters altWaitForElementParameters = new AltWaitForElementParameters.Builder(
                altFindElementsParameters).withTimeout(timeout).withInterval(interval).build();
        return waitForElement(altWaitForElementParameters);
    }

    @Deprecated
    public AltUnityObject waitForElement(String name) {
        return waitForElement(name, "", true, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElement(String name, String cameraName) {
        return waitForElement(name, cameraName, true, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElement(String name, boolean enabled) {
        return waitForElement(name, "", enabled, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElementWithText(
            AltWaitForElementWithTextParameters altWaitForElementWithTextParameters) {
        return new AltWaitForElementWithText(altBaseSettings, altWaitForElementWithTextParameters).Execute();
    }

    @Deprecated
    public AltUnityObject waitForElementWithText(String name, String text, String cameraName, boolean enabled,
            double timeout, double interval) {
        AltFindElementsParameters altFindElementsParameters = new AltFindElementsParameters.Builder(name)
                .withCamera(cameraName).isEnabled(enabled).build();
        AltWaitForElementWithTextParameters altWaitForElementWithTextParameters = new AltWaitForElementWithTextParameters.Builder(
                altFindElementsParameters, text).withInterval(interval).withTimeout(timeout).build();
        return waitForElementWithText(altWaitForElementWithTextParameters);
    }

    @Deprecated
    public AltUnityObject waitForElementWithText(String name, String text) {
        return waitForElementWithText(name, text, "", true, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElementWithText(String name, String text, String cameraName) {
        return waitForElementWithText(name, text, cameraName, true, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject waitForElementWithText(String name, String text, boolean enabled) {
        return waitForElementWithText(name, text, "", enabled, 20, 0.5);
    }

    @Deprecated
    public AltUnityObject findElementByComponent(
            AltFindElementsByComponentParameters altFindElementsByComponentParameters) {
        return new AltFindElementByComponent(altBaseSettings, altFindElementsByComponentParameters).Execute();
    }

    @Deprecated
    public AltUnityObject findElementByComponent(String componentName, String assemblyName, String cameraName,
            boolean enabled) {
        AltFindElementsByComponentParameters altFindElementsByComponentParameters = new AltFindElementsByComponentParameters.Builder(
                componentName).inAssembly(assemblyName).isEnabled(enabled).withCamera(cameraName).build();
        return findElementByComponent(altFindElementsByComponentParameters);
    }

    @Deprecated
    public AltUnityObject findElementByComponent(String componentName) {
        return findElementByComponent(componentName, "", "", true);
    }

    @Deprecated
    public AltUnityObject findElementByComponent(String componentName, String cameraName) {
        return findElementByComponent(componentName, "", cameraName, true);
    }

    @Deprecated
    public AltUnityObject findElementByComponent(String componentName, String assemblyName, boolean enabled) {
        return findElementByComponent(componentName, assemblyName, "", enabled);
    }

    @Deprecated
    public AltUnityObject[] findElementsByComponent(
            AltFindElementsByComponentParameters altFindElementsByComponentParameters) {
        return new AltFindElementsByComponent(altBaseSettings, altFindElementsByComponentParameters).Execute();
    }

    @Deprecated
    public AltUnityObject[] findElementsByComponent(String componentName, String assemblyName, String cameraName,
            boolean enabled) {
        AltFindElementsByComponentParameters altFindElementsByComponentParameters = new AltFindElementsByComponentParameters.Builder(
                componentName).inAssembly(assemblyName).isEnabled(enabled).withCamera(cameraName).build();
        return findElementsByComponent(altFindElementsByComponentParameters);
    }

    @Deprecated
    public AltUnityObject[] findElementsByComponent(String componentName, String assemblyName) {
        return findElementsByComponent(componentName, assemblyName, "", true);
    }

    @Deprecated
    public AltUnityObject[] findElementsByComponent(String componentName, String assemblyName, boolean enabled) {
        return findElementsByComponent(componentName, assemblyName, "", enabled);
    }

    public void getPNGScreeshot(String path) {
        new GetPNGScreenshotCommand(altBaseSettings, path).Execute();
    }

    // TODO: move those two out of this type and make them compulsory
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
        TAG, LAYER, NAME, COMPONENT, PATH, ID
    }
}
