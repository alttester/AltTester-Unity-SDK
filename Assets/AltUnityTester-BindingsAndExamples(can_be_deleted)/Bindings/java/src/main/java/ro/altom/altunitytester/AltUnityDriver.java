package ro.altom.altunitytester;

import com.google.gson.Gson;
import lombok.extern.slf4j.Slf4j;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

import java.io.DataInputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;

@Slf4j
public class AltUnityDriver {

    public static final int READ_TIMEOUT = 30 * 1000;

    public static class PlayerPrefsKeyType {
        public static int IntType = 1;
        public static int StringType = 2;
        public static int FloatType = 3;
    }

    private final Socket socket;
    private final static int BUFFER_SIZE = 1024;
    private final PrintWriter out;
    private final DataInputStream in;

    public static String RequestSeparator=";";
    public static String RequestEnd="&";

    public AltUnityDriver(String ip, int port) {
        if (ip == null || ip.isEmpty()) {
            throw new InvalidParamerException("Provided IP address is null or empty");
        }
        try {
            //log.info("Initializing connection to {}:{}", ip, port);
            socket = new Socket(ip, port);
            socket.setSoTimeout(READ_TIMEOUT);
            out = new PrintWriter(socket.getOutputStream(), true);
            in = new DataInputStream(socket.getInputStream());
        } catch (IOException e) {
            throw new ConnectionException("Could not create connection to " + String.format("%s:%d", ip, port), e);
        }
        // TODO: make this unnecessary
        AltUnityObject.altUnityDriver = this;
    }

    public AltUnityDriver(String ip, int port,String requestSeparator,String requestEnd) {
        if (ip == null || ip.isEmpty()) {
            throw new InvalidParamerException("Provided IP address is null or empty");
        }
        try {
            //log.info("Initializing connection to {}:{}", ip, port);
            socket = new Socket(ip, port);
            socket.setSoTimeout(READ_TIMEOUT);
            out = new PrintWriter(socket.getOutputStream(), true);
            in = new DataInputStream(socket.getInputStream());
        } catch (IOException e) {
            throw new ConnectionException("Could not create connection to " + String.format("%s:%d", ip, port), e);
        }
        // TODO: make this uneccesary
        RequestEnd=requestEnd;
        RequestSeparator=requestSeparator;
        AltUnityObject.altUnityDriver = this;
    }

    public String CreateCommand(String... arguments){
        String command="";
        for (String argument:arguments) {
            command+=argument+RequestSeparator;
        }
        return command+RequestEnd;

    }
    public void send(String message) {
        log.info("Sending rpc message [{}]", message);
        out.print(message);
        out.flush();
    }

    public void stop() {
        log.info("Closing connection with server.");
        send(CreateCommand("closeConnection"));
        try {
            in.close();
            out.close();
            socket.close();
        } catch (IOException e) {
            throw new ConnectionException("Could not close the socket.", e);
        }
    }

    // TODO: move this out from the driver
    public String recvall() {
        String receivedData = "";
        boolean streamIsFinished = false;

        while (!streamIsFinished) {
            byte[] messageByte = new byte[BUFFER_SIZE];
            int bytesRead = 0;
            try {
                bytesRead = in.read(messageByte);
            } catch (IOException e) {
                throw new ConnectionException(e);
            }
            if (bytesRead > 0)
                receivedData += new String(messageByte, 0, bytesRead);
            if (receivedData.contains("::altend")) {
                streamIsFinished = true;
            }
        }

        receivedData = receivedData.split("altstart::")[1].split("::altend")[0];
        log.debug("Data received: " + receivedData);
        return receivedData;
    }

    public String callStaticMethods(String assembly, String typeName, String methodName,
                                    String parameters, String typeOfParameters) {
        String actionInfo = new Gson().toJson(new AltUnityObjectAction(typeName, methodName, parameters, typeOfParameters, assembly));
        send(CreateCommand("callComponentMethodForObject", "" , actionInfo ));
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }

    public String callStaticMethods(String typeName, String methodName, String parameters) {
        return callStaticMethods("", typeName, methodName, parameters, "");
    }

    public void loadScene(String scene) {
        log.debug("Load scene: " + scene + "...");
        send(CreateCommand("loadScene",scene ));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void deletePlayerPref() {
        send(CreateCommand("deletePlayerPref"));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void deleteKeyPlayerPref(String keyName) {
        send(CreateCommand("deleteKeyPlayerPref", keyName));
        String data = recvall();
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, int valueName) {
        send(CreateCommand("setKeyPlayerPref", keyName, String.valueOf(valueName), String.valueOf(PlayerPrefsKeyType.IntType)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, float valueName) {
        send(CreateCommand("setKeyPlayerPref", keyName, String.valueOf(valueName), String.valueOf(PlayerPrefsKeyType.FloatType)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, String valueName) {
        send(CreateCommand("setKeyPlayerPref", keyName, valueName, String.valueOf(PlayerPrefsKeyType.StringType)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public int getIntKeyPlayerPref(String keyname) {
        send(CreateCommand("getKeyPlayerPref", keyname, String.valueOf(PlayerPrefsKeyType.IntType)));
        String data = recvall();
        if (!data.contains("error:")) {
            return Integer.parseInt(data);
        }
        handleErrors(data);
        return 0;
    }

    public float getFloatKeyPlayerPref(String keyname) {
        send(CreateCommand("getKeyPlayerPref", keyname, String.valueOf(PlayerPrefsKeyType.FloatType)));
        String data = recvall();
        if (!data.contains("error:")) {
            return Float.parseFloat(data);
        }
        handleErrors(data);
        return 0;

    }

    public String getStringKeyPlayerPref(String keyname) {
        send(CreateCommand("getKeyPlayerPref", keyname, String.valueOf(PlayerPrefsKeyType.StringType)));
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }

    public String getCurrentScene() {
        //log.debug("Get current scene...");
        send(CreateCommand("getCurrentScene"));
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject.class)).name;
        }
        handleErrors(data);
        return "";
    }

    public float getTimeScale() {
        send(CreateCommand("getTimeScale"));
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, float.class));
        }
        handleErrors(data);
        return 0;
    }

    public void setTimeScale(float timeScale) {
        send(CreateCommand("setTimeScale", String.valueOf(timeScale)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void swipe(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) {
        String vectorStartJson = vectorToJsonString(xStart, yStart);
        String vectorEndJson = vectorToJsonString(xEnd, yEnd);
        send(CreateCommand("movingTouch", vectorStartJson, vectorEndJson, String.valueOf(durationInSecs)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void swipeAndWait(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) {
        swipe(xStart, yStart, xEnd, yEnd, durationInSecs);
        sleepFor(durationInSecs );
        String data;
        do {
            send(CreateCommand("swipeFinished"));
            data = recvall();
        } while (data.equals("No"));

        if (data.equals("Yes")) {
            return;
        }
        handleErrors(data);
    }

    public void holdButton(int xPosition, int yPosition, float durationInSecs) {
        swipe(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    public void holdButtonAndWait(int xPosition, int yPosition, float durationInSecs) {
        swipeAndWait(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    public AltUnityObject clickScreen(float x, float y) {
        send(CreateCommand("clickScreenOnXY", String.valueOf(x), String.valueOf(y)));
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject.class));
        }
        handleErrors(data);
        return null;
    }

    public void tilt(int x, int y, int z) {
        String accelerationString = vectorToJsonString(x, y, z);
        send(CreateCommand("tilt", accelerationString));
        String data = recvall();
        if (data.equals("OK")) {
            return;
        }
        handleErrors(data);
    }

    public AltUnityObject findElementWhereNameContains(String name, String cameraName,boolean enabled) {
        send(CreateCommand("findObjectWhereNameContains", name, cameraName, String.valueOf(enabled)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElementWhereNameContains(String name, String cameraName) {
        return findElementWhereNameContains(name, cameraName, true);
    }
    public AltUnityObject findElementWhereNameContains(String name, boolean enabled) {
        return findElementWhereNameContains(name, "", enabled);
    }

    public AltUnityObject findElementWhereNameContains(String name) {
        return findElementWhereNameContains(name, "");
    }

    public AltUnityObject[] getAllElements(String cameraName,boolean enabled) {
        send(CreateCommand("findAllObjects", cameraName, String.valueOf(enabled)));
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject[].class));
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject[] getAllElements(String cameraName) {
        return getAllElements(cameraName,true);
    }

    public AltUnityObject[] getAllElements(boolean enabled)  {
        return getAllElements("",enabled);
    }

    public AltUnityObject[] getAllElements() throws Exception {
        return getAllElements("",true);
    }

    public AltUnityObject findElement(String name, String cameraName, boolean enabled) {
        send(CreateCommand("findObjectByName", name, cameraName, String.valueOf(enabled)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElement(String name,boolean enabled) {
        return findElement(name, "",enabled);
    }
    public AltUnityObject findElement(String name,String cameraName) {
        return findElement(name, cameraName,true);
    }
    public AltUnityObject findElement(String name) {
        return findElement(name, "",true);
    }

    public AltUnityObject[] findElements(String name, String cameraName, boolean enabled) {
        send(CreateCommand("findObjectsByName", name, cameraName, String.valueOf(enabled)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject[].class);
        }
        handleErrors(data);
        return new AltUnityObject[]{};
    }

    public AltUnityObject[] findElements(String name) {
        return findElements(name, "",true);
    }
    public AltUnityObject[] findElements(String name, String cameraName) {
        return findElements(name, cameraName,true);
    }
    public AltUnityObject[] findElements(String name, boolean enabled) {
        return findElements(name, "",enabled);
    }

    public AltUnityObject[] findElementsWhereNameContains(String name, String cameraName, boolean enabled) {
        send(CreateCommand("findObjectsWhereNameContains", name, cameraName, String.valueOf(enabled)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject[].class);
        }
        handleErrors(data);
        return new AltUnityObject[]{};
    }
    public AltUnityObject[] findElementsWhereNameContains(String name, String cameraName) {
        return findElementsWhereNameContains(name,cameraName,true);
    }
    public AltUnityObject[] findElementsWhereNameContains(String name,boolean enabled) {
        return findElementsWhereNameContains(name,"",enabled);

    }
    public AltUnityObject[] findElementsWhereNameContains(String name) {
        return findElementsWhereNameContains(name,"",true);

    }

    public AltUnityObject tapScreen(int x, int y) {
        send(CreateCommand("tapScreen", String.valueOf(x), String.valueOf(y)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        if(data.contains("error:notFound"))
            return null;
        handleErrors(data);
        return null;
    }

    public String waitForCurrentSceneToBe(String sceneName, double timeout, double interval) {
        double time = 0;
        String currentScene = "";
        while (time < timeout) {
            //log.debug("Waiting for scene to be " + sceneName + "...");
            currentScene = getCurrentScene();
            if (currentScene != null && currentScene.equals(sceneName)) {
                return currentScene;
            }
            sleepFor(interval);
            time += interval;
        }
        throw new WaitTimeOutException("Scene [" + sceneName + "] not loaded after " + timeout + " seconds");
    }

    public String waitForCurrentSceneToBe(String sceneName) {
        return waitForCurrentSceneToBe(sceneName, 20, 0.5);
    }

    public AltUnityObject waitForElementWhereNameContains(String name, String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            //log.debug("Waiting for element where name contains " + name + "....");
            try {
                altElement = findElementWhereNameContains(name, cameraName);
                if (altElement != null) {
                    return altElement;
                }
            } catch (Exception e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            sleepFor(interval);
            time += interval;
        }
        throw new WaitTimeOutException("Element " + name + " still not found after " + timeout + " seconds");
    }

    public AltUnityObject waitForElementWhereNameContains(String name) {
        return waitForElementWhereNameContains(name, "", 20, 0.5);
    }

    public void waitForElementToNotBePresent(String name, String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time <= timeout) {
            //log.debug("Waiting for element " + name + " not to be present");
            try {
                altElement = findElement(name, cameraName);
                if (altElement == null) {
                    return;
                }
            } catch (Exception e) {
                log.warn(e.getLocalizedMessage());
                break;
            }
            sleepFor(interval);
            time += interval;
        }

        if (altElement != null) {
            throw new AltUnityException("Element " + name + " still found after " + timeout + " seconds");
        }
    }

    /**
     * Sleeps for certain amount of seconds.
     *
     * @param interval Seconds to sleep for.
     */
    private void sleepFor(double interval) {
        long timeToSleep = (long) (interval * 1000);
        try {
            Thread.sleep(timeToSleep);
        } catch (InterruptedException e) {
            log.warn("Could not sleep for " + timeToSleep + " ms");
        }
    }

    public void waitForElementToNotBePresent(String name) {
        waitForElementToNotBePresent(name, "", 20, 0.5);
    }

    public AltUnityObject waitForElement(String name, String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            //log.debug("Waiting for element " + name + "...");
            try {
                altElement = findElement(name, cameraName);
            } catch (Exception e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }

            if (altElement != null) {
                return altElement;
            }
            sleepFor(interval);
            time += interval;
        }

        throw new WaitTimeOutException("Element " + name + " not loaded after " + timeout + " seconds");
    }

    public AltUnityObject waitForElement(String name) {
        return waitForElement(name, "", 20, 0.5);
    }

    public AltUnityObject waitForElementWithText(String name, String text, String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            //log.debug("Waiting for element " + name + " to have text [" + text + "]");
            try {
                altElement = findElement(name, cameraName);
                if (altElement != null && altElement.getText().equals(text)) {
                    return altElement;
                }
            } catch (AltUnityException e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            time += interval;
            sleepFor(interval);
        }
        throw new WaitTimeOutException("Element with text: " + text + " not loaded after " + timeout + " seconds");
    }

    public AltUnityObject waitForElementWithText(String name, String text) {
        return waitForElementWithText(name, text, "", 20, 0.5);
    }

    public AltUnityObject findElementByComponent(String componentName,String assemblyName, String cameraName,boolean enabled) {
        send(CreateCommand("findObjectByComponent",assemblyName, componentName, cameraName, String.valueOf(enabled)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElementByComponent(String componentName) {
        return findElementByComponent(componentName, "","",true);
    }

    public AltUnityObject findElementByComponent(String componentName, String cameraName) {
        return findElementByComponent(componentName,"", cameraName,true);
    }

    public AltUnityObject findElementByComponent(String componentName,String assemblyName, boolean enabled) {
        return findElementByComponent(componentName, assemblyName,"", enabled);
    }

    public AltUnityObject[] findElementsByComponent(String componentName, String assemblyName, String cameraName, boolean enabled) {
        send(CreateCommand("findObjectsByComponent",assemblyName, componentName, cameraName, String.valueOf(enabled)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject[].class);
        }
        handleErrors(data);
        return new AltUnityObject[]{};
    }

    public AltUnityObject[] findElementsByComponent(String componentName,String assemblyName ) {
        return findElementsByComponent(componentName, assemblyName,"",true);
    }

    public AltUnityObject[] findElementsByComponent(String componentName,String assemblyName, boolean enabled) {
        return findElementsByComponent(componentName, assemblyName,"", enabled);
    }



    public String vectorToJsonString(int x, int y) {
        return "{\"x\":" + x + ", \"y\":" + y + "}";
    }

    public String vectorToJsonString(int x, int y, int z) {
        return "{\"x\":" + x + ", \"y\":" + y + ", \"z\":" + z + "}";
    }

    public void handleErrors(String data) {
        String typeOfException = data.split(";")[0];
        if ("error:notFound".equals(typeOfException)) {
            throw new NotFoundException(data);
        } else if ("error:propertyNotFound".equals(typeOfException)) {
            throw new PropertyNotFoundException(data);
        } else if ("error:methodNotFound".equals(typeOfException)) {
            throw new MethodNotFoundException(data);
        } else if ("error:componentNotFound".equals(typeOfException)) {
            throw new ComponentNotFoundException(data);
        } else if ("error:couldNotPerformOperation".equals(typeOfException)) {
            throw new CouldNotPerformOperationException(data);
        } else if ("error:couldNotParseJsonString".equals(typeOfException)) {
            throw new CouldNotParseJsonStringException(data);
        } else if ("error:incorrectNumberOfParameters".equals(typeOfException)) {
            throw new IncorrectNumberOfParametersException(data);
        } else if ("error:failedToParseMethodArguments".equals(typeOfException)) {
            throw new FailedToParseArgumentsException(data);
        } else if ("error:objectNotFound".equals(typeOfException)) {
            throw new ObjectWasNotFoundException(data);
        } else if ("error:propertyCannotBeSet".equals(typeOfException)) {
            throw new PropertyNotFoundException(data);
        } else if ("error:nullRefferenceException".equals(typeOfException)) {
            throw new NullRefferenceException(data);
        } else if ("error:unknownError".equals(typeOfException)) {
            throw new UnknownErrorException(data);
        } else if ("error:formatException".equals(typeOfException)) {
            throw new FormatException(data);
        }
    }

    // TODO: move those two out of this type and make them compulsory
    public static void setupPortForwarding(String platform,String deviceID, int local_tcp_port, int remote_tcp_port) {
        log.info("Setting up port forward for " + platform + " on port " + remote_tcp_port);
        removePortForwarding();
        if (platform.toLowerCase().equals("android".toLowerCase())) {
            try {
                String commandToRun;
                if(deviceID.equals(""))
                    commandToRun = "adb forward tcp:" + local_tcp_port + " tcp:" + remote_tcp_port;
                else
                    commandToRun = "adb forward -s "+deviceID+" tcp:" + local_tcp_port + " tcp:" + remote_tcp_port;
                Runtime.getRuntime().exec(commandToRun);
                Thread.sleep(1000);
                log.info("adb forward enabled.");
            } catch (Exception e) {
                log.warn("AltUnityServer - abd probably not installed\n" + e);
            }

        } else if (platform.toLowerCase().equals("ios".toLowerCase())) {
            try {
                String commandToRun;
                if(deviceID.equals(""))
                    commandToRun = "iproxy " + local_tcp_port + " " + remote_tcp_port + "&";
                else
                    commandToRun = "iproxy " + local_tcp_port + " " + remote_tcp_port+" "+deviceID  + "&";
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
}
