package ro.altom.altunitytester;

import com.google.gson.Gson;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

import java.io.DataInputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.time.Duration;
import java.time.Instant;
import java.util.Date;

public class AltUnityDriver {

    private static final int MAX_LISTENING_TIME = 3000;

    public static class PlayerPrefsKeyType {
        public static int IntType = 1;
        public static int StringType = 2;
        public static int FloatType = 3;
    }

    private final Socket socket;
    private final static int BUFFER_SIZE = 1024;
    private final PrintWriter out;
    private final DataInputStream in;


    public AltUnityDriver(String ip, int port) throws IOException {
        if (ip == null || ip.isEmpty()) {
            throw new AltUnityException("Provided IP address is null or empty");
        }
        socket = new Socket(ip, port);
        out = new PrintWriter(socket.getOutputStream(), true);
        in = new DataInputStream(socket.getInputStream());
        // TODO: make this uneccesary
        AltUnityObject.altUnityDriver = this;
    }

    public void send(String message) {
        out.print(message);
        out.flush();
    }

    public void stop() throws IOException {
        send("closeConnection;&");
        String data = recvall();
        if (!data.equals("Ok")){
            System.err.println("Could not close connection from the client");
        }
        socket.close();
    }

    // TODO: move this out from the driver
    public String recvall() {
        String receivedData = "";
        boolean streamIsFinished = false;
        Instant start = Instant.now();

        while (!streamIsFinished) {
            byte[] messageByte = new byte[BUFFER_SIZE];
            int bytesRead = 0;
            try {
                bytesRead = in.read(messageByte);
            } catch (IOException e) {
                throw new AltUnityException(e);
            }
            if (bytesRead > 0)
                receivedData += new String(messageByte, 0, bytesRead);
            if (receivedData.contains("::altend")) {
                streamIsFinished = true;
            }
            Instant now = Instant.now();
            long timeSpendOnListening = Duration.between(start, now).toMillis();
            if (timeSpendOnListening > MAX_LISTENING_TIME){
                // TODO: rethink this part - possibly throw exception or check the split which happens later
                System.err.println("Couldn't get and answer from the client for specified time [ms] " + MAX_LISTENING_TIME);
                return "";
            }
        }

        receivedData = receivedData.split("altstart::")[1].split("::altend")[0];
        System.out.println("Data received: " + receivedData);
        return receivedData;
    }

    public String callStaticMethods(String assembly,String typeName, String methodName,
                                    String parameters,String typeOfParameters) throws Exception {
        String actionInfo =new Gson().toJson(new AltUnityObjectAction(typeName, methodName, parameters,typeOfParameters,assembly));
        send("callComponentMethodForObject;" + "" + "; " + actionInfo + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }

    public String callStaticMethods(String typeName, String methodName,String parameters ) throws Exception {
        return callStaticMethods("",typeName,methodName,parameters,"");
    }

    public void loadScene(String scene) throws Exception {
        System.out.println("Load scene: " + scene + "...");
        send("loadScene;" + scene + ";&");
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void deletePlayerPref() throws Exception {
        send("deletePlayerPref;&");
        String data = recvall();
        if (data.equals("Ok")){
            return;
        }
        handleErrors(data);
    }

    public void deleteKeyPlayerPref(String keyName) throws Exception {
        send("deleteKeyPlayerPref;" + keyName + ";&");
        String data = recvall();
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, int valueName) throws Exception {
        send("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PlayerPrefsKeyType.IntType + ";&");
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, float valueName) throws Exception {
        send("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PlayerPrefsKeyType.FloatType + ";&");
        String data = recvall();
        if (data.equals("Ok")){
            return;
        }
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, String valueName) throws Exception {
        send("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PlayerPrefsKeyType.StringType + ";&");
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

    public int getIntKeyPlayerPref(String keyname) throws Exception {
        send("getKeyPlayerPref;" + keyname + ";" + PlayerPrefsKeyType.IntType + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return Integer.parseInt(data);
        }
        handleErrors(data);
        return 0;
    }

    public float getFloatKeyPlayerPref(String keyname) throws Exception {
        send("getKeyPlayerPref;" + keyname + ";" + PlayerPrefsKeyType.FloatType + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return Float.parseFloat(data);
        }
        handleErrors(data);
        return 0;

    }

    public String getStringKeyPlayerPref(String keyname) throws Exception {
        send("getKeyPlayerPref;" + keyname + ";" + PlayerPrefsKeyType.StringType + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }

    public String getCurrentScene() {
        System.out.println("Get current scene...");
        send("getCurrentScene;&");
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject.class)).name;
        }
        handleErrors(data);
        return "";
    }

    public void swipe(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) {
        String vectorStartJson = vectorToJsonString(xStart, yStart);
        String vectorEndJson = vectorToJsonString(xEnd, yEnd);
        send("movingTouch;" + vectorStartJson + ";" + vectorEndJson + ";" + durationInSecs + ";&");
        String data = recvall();
        if (data.equals("Ok")){
            return;
        }
        handleErrors(data);
    }

    public void swipeAndWait(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) {
        swipe(xStart, yStart, xEnd, yEnd, durationInSecs);
        sleepFor(durationInSecs * 1000);
        String data;
        do {
            send("swipeFinished;&");
            data = recvall();
        } while (data.equals("No"));

        if (data.equals("Yes")){
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
        send("clickScreenOnXY;" + x + ";" + y + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject.class));
        }
        handleErrors(data);
        return null;
    }

    public void tilt(int x, int y, int z)  {
        String accelerationString = vectorToJsonString(x, y, z);
        send("tilt;" + accelerationString + ";&");
        String data = recvall();
        if (data.equals("OK")) {
            return;
        }
        handleErrors(data);
    }

    public AltUnityObject findElementWhereNameContains(String name, String cameraName) {
        send("findObjectWhereNameContains;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElementWhereNameContains(String name) {
        return findElementWhereNameContains(name, "");
    }

    public AltUnityObject[] getAllElements(String cameraName) {
        send("findAllObjects;" + ";" + cameraName + "&");
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject[].class));
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject[] getAllElements() {
        return getAllElements("");
    }

    public AltUnityObject findElement(String name, String cameraName) {
        send("findObjectByName;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElement(String name) {
        return findElement(name, "");
    }

    public AltUnityObject[] findElements(String name, String cameraName) {
        send("findObjectsByName;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject[].class);
        }
        handleErrors(data);
        return new AltUnityObject[]{};
    }

    public AltUnityObject[] findElements(String name) {
        return findElements(name, "");
    }

    public AltUnityObject[] findElementsWhereNameContains(String name, String cameraName) {
        send("findObjectsWhereNameContains;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")){
            return new Gson().fromJson(data, AltUnityObject[].class);
        }
        handleErrors(data);
        return new AltUnityObject[]{};
    }

    public AltUnityObject tapScreen(int x, int y) {
        send("tapScreen;" + x + ";" + y + ";&");
        String data = recvall();
        if (!data.contains("error:")){
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject[] findElementsWhereNameContains(String name) {
        return findElementsWhereNameContains(name, "");
    }

    public String waitForCurrentSceneToBe(String sceneName, double timeout, double interval) {
        double time = 0;
        String currentScene = "";
        while (time < timeout) {
            currentScene = getCurrentScene();
            if (!currentScene.equals(sceneName)) {
                System.out.println("Waiting for scene to be " + sceneName + "...");
                sleepFor(interval);
                time += interval;
            } else {
                break;
            }
        }

        if (sceneName.equals(currentScene))
            return currentScene;
        throw new WaitTimeOutException("Scene " + sceneName + " not loaded after " + timeout + " seconds");
    }

    public String waitForCurrentSceneToBe(String sceneName) throws Exception {
        return waitForCurrentSceneToBe(sceneName, 20, 0.5);
    }

    public AltUnityObject waitForElementWhereNameContains(String name,String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            try {
                altElement = findElementWhereNameContains(name,cameraName);
                break;
            } catch (Exception e) {
                System.out.println("Waiting for element where name contains " + name + "....");
                sleepFor(interval);
                time += interval;

            }
        }
        if (altElement != null)
            return altElement;
        throw new WaitTimeOutException("Element " + name + " still not found after " + timeout + " seconds");
    }

    public AltUnityObject waitForElementWhereNameContains(String name) throws Exception {
        return waitForElementWhereNameContains(name,"", 20, 0.5);
    }


    public void waitForElementToNotBePresent(String name,String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time <= timeout) {
            try {
                altElement = findElement(name,cameraName);
                break;
            } catch (Exception e) {
                sleepFor(interval);
                time += interval;
                System.out.println("Waiting for element " + name + " to not be present");
            }
        }

        if (!altElement.equals(null))
            throw new AltUnityException("Element " + name + " not found after " + timeout + " seconds");
    }

    private void sleepFor(double interval) {
            long timeToSleep = (long) (interval * 1000);
        try {
            Thread.sleep(timeToSleep);
        } catch (InterruptedException e) {
            System.err.println("Could not sleep for " + timeToSleep + " ms");
        }
    }

    public void waitForElementToNotBePresent(String name) throws Exception {
        waitForElementToNotBePresent(name,"", 20, 0.5);
    }


    public AltUnityObject waitForElement(String name,String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            try {
                altElement = findElement(name,cameraName);
                break;
            } catch (Exception e) {
                sleepFor(interval);
                time += interval;
                System.out.println("Waiting for element " + name + "...");
            }
        }

        if (altElement != null) {
            return altElement;
        }
        throw new WaitTimeOutException("Element " + name + " loaded after " + timeout + " seconds");
    }

    public AltUnityObject waitForElement(String name) throws Exception {
        return waitForElement(name,"", 20, 0.5);
    }

    public AltUnityObject waitForElementWithText(String name, String text,String cameraName, double timeout, double interval) {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            try {
                altElement = findElement(name, cameraName);
                if (altElement.getText().equals(text)){
                    return altElement;
                }
                throw new AltUnityException("This is peculiar way of making time counting work - HAS TO BE CHANGED");
            } catch (AltUnityException e) {
                sleepFor(interval);
                time += interval;
                System.out.println("Waiting for element " + name + " to have text " + text);
            }
        }
        throw new WaitTimeOutException("Element with text: " + text + " loaded after " + timeout + " seconds");
    }

    public AltUnityObject waitForElementWithText(String name, String text) {
        return waitForElementWithText(name, text,"", 20, 0.5);
    }

    public AltUnityObject findElementByComponent(String componentName,String assemblyName, String cameraName) {
        send("findObjectByComponent;"+assemblyName+";" + componentName + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElementByComponent(String componentName) {
        return findElementByComponent(componentName, "","");
    }
    public AltUnityObject findElementByComponent(String componentName,String assemblyName) {
        return findElementByComponent(componentName,assemblyName,"");
    }

    public AltUnityObject[] findElementsByComponent(String componentName,String assemblyName, String cameraName) {
        send("findObjectsByComponent;"+assemblyName+";"  + componentName + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) return new Gson().fromJson(data, AltUnityObject[].class);
        handleErrors(data);
        return new AltUnityObject[]{};
    }

    public AltUnityObject[] findElementsByComponent(String componentName) {
        return findElementsByComponent(componentName, "","");
    }
    public AltUnityObject[] findElementsByComponent(String componentName,String assemblyName) {
        return findElementsByComponent(componentName,assemblyName,"");
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
    public static void setupPortForwarding(String platform, int tcp_port) {
        System.out.println("Setting up port forward for " + platform + " on port " + tcp_port);
        removePortForwarding();
        if (platform.toLowerCase().equals("android".toLowerCase())) {
            try {
                String commandToRun = "adb forward tcp:" + tcp_port + " tcp:" + tcp_port;
                ProcessBuilder pb = new ProcessBuilder(commandToRun);
                Process executionProcess = pb.start();
                executionProcess.waitFor();
//                Runtime.getRuntime().exec(commandToRun);
//                Thread.sleep(1000);
                System.out.println("adb forward enabled.");
            } catch (Exception e) {
                System.err.println("AltUnityServer - abd probably not installed\n" + e);
            }

        } else if (platform.toLowerCase().equals("ios".toLowerCase())) {
            try {
                Runtime.getRuntime().exec("iproxy " + tcp_port + " " + tcp_port + "&");
                Thread.sleep(1000);
                System.out.println("iproxy forward enabled.");
            } catch (Exception e) {
                System.err.println("AltUnityServer - no iproxy process was running/present\n" + e);
            }
        }
    }

    public static void removePortForwarding() {
        try {
            String commandToExecute = "killall iproxy";
            ProcessBuilder pb = new ProcessBuilder(commandToExecute);
            Process executionProcess = pb.start();
            executionProcess.waitFor();
//            Runtime.getRuntime().exec(commandToExecute);
//            Thread.sleep(1000);
            System.out.println("Killed any iproxy process that may have been running...");
        } catch (Exception e) {
            System.err.println("AltUnityServer - no iproxy process was running/present\n" + e);
        }

        try {
            String commandToExecute = "adb forward --remove-all";
            ProcessBuilder pb = new ProcessBuilder(commandToExecute);
            Process executionProcess = pb.start();
            executionProcess.waitFor();
//            Runtime.getRuntime().exec(commandToExecute);
//            Thread.sleep(1000);
            System.out.println("Removed existing adb forwarding...");
        } catch (Exception e) {
            System.err.println("AltUnityServer - abd probably not installed\n" + e);
        }
    }
}
