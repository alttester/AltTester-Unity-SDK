package ro.altom.altunitytester;

import com.google.gson.Gson;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

import java.io.*;
import java.net.Socket;

public class AltUnityDriver {

    public static class PlayerPrefsKeyType {
        public static int IntType = 1;
        public static int StringType = 2;
        public static int FloatType = 3;
    }

    public Socket socket;
    private static String tcp_ip = "127.0.0.1";
    private static int tcp_port = 13000;
    private static int BUFFER_SIZE = 1024;
    private PrintWriter out;
    private DataInputStream in;


    public AltUnityDriver(String ip, int port) throws IOException {
        tcp_ip = ip;
        tcp_port = port;
        socket = new Socket(tcp_ip, tcp_port);
        out = new PrintWriter(socket.getOutputStream(), true);
        in = new DataInputStream(socket.getInputStream());
        AltUnityObject.altUnityDriver = this;

    }

    public void send(String message) throws Exception {
        out.print(message);
        out.flush();
    }

    public void stop() throws Exception {
        send("closeConnection;&");
        Thread.sleep(2000);
        socket.close();
    }

    public String recvall() throws Exception {
        String data = "";
        boolean end = false;
        while (!end) {
            byte[] messageByte = new byte[BUFFER_SIZE];
            int bytesRead = in.read(messageByte);
            if (bytesRead > 0)
                data += new String(messageByte, 0, bytesRead);
            if (data.contains("::altend")) {
                end = true;
            }
        }
        try {
            data = data.split("altstart::")[1].split("::altend")[0];
        } catch (Exception e) {
            System.out.println("Data received from socket doesn't have correct start and end control Strings");
            throw e;
        }
        System.out.println("Data received: " + data);
        return data;
    }
    public String callStaticMethods(String assembly,String typeName, String methodName,
                                    String parameters,String typeOfParameters) throws Exception {
        String actionInfo =new Gson().toJson(new AltUnityObjectAction(typeName, methodName, parameters,typeOfParameters,assembly));
        send("callComponentMethodForObject;" + "" + "; " + actionInfo + ";&");
        String data = recvall();
        if (!data.contains("error:")) return data;
        handleErrors(data);
        return null;
    }
    public String callStaticMethods(String assembly,String typeName, String methodName,
                                    String parameters) throws Exception {
        return callStaticMethods(typeName,methodName,parameters,"");
    }
    public String callStaticMethods(String typeName, String methodName,String parameters ) throws Exception {
        return callStaticMethods("",typeName,methodName,parameters,"");
    }

    public void loadScene(String scene) throws Exception {
        System.out.println("Load scene: " + scene + "...");
        send("loadScene;" + scene + ";&");
        String data = recvall();
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }

    public void deletePlayerPref() throws Exception {
        send("deletePlayerPref;&");
        String data = recvall();
        if (data.equals("Ok"))
            return;
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
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, float valueName) throws Exception {
        send("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PlayerPrefsKeyType.FloatType + ";&");
        String data = recvall();
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }

    public void setKeyPlayerPref(String keyName, String valueName) throws Exception {
        send("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PlayerPrefsKeyType.StringType + ";&");
        String data = recvall();
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }


    public int getIntKeyPlayerPref(String keyname) throws Exception {
        send("getKeyPlayerPref;" + keyname + ";" + PlayerPrefsKeyType.IntType + ";&");
        String data = recvall();
        if (!data.contains("error:")) return Integer.parseInt(data);
        handleErrors(data);
        return 0;
    }

    public float getFloatKeyPlayerPref(String keyname) throws Exception {
        send("getKeyPlayerPref;" + keyname + ";" + PlayerPrefsKeyType.FloatType + ";&");
        String data = recvall();
        if (!data.contains("error:")) return Float.parseFloat(data);
        handleErrors(data);
        return 0;

    }

    public String getStringKeyPlayerPref(String keyname) throws Exception {
        send("getKeyPlayerPref;" + keyname + ";" + PlayerPrefsKeyType.StringType + ";&");
        String data = recvall();
        if (!data.contains("error:")) return data;
        handleErrors(data);
        return null;

    }

    public String getCurrentScene() throws Exception {
        System.out.println("Get current scene...");
        send("getCurrentScene;&");
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject.class)).name;
        }
        handleErrors(data);
        return null;
    }


    public void swipe(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) throws Exception {
        String vectorStartJson = vectorToJsonString(xStart, yStart);
        String vectorEndJson = vectorToJsonString(xEnd, yEnd);
        send("movingTouch;" + vectorStartJson + ";" + vectorEndJson + ";" + durationInSecs + ";&");
        String data = recvall();
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }


    public void swipeAndWait(int xStart, int yStart, int xEnd, int yEnd, float durationInSecs) throws Exception {
        swipe(xStart, yStart, xEnd, yEnd, durationInSecs);
        Thread.sleep((int) durationInSecs * 1000);
        String data;
        do {
            send("swipeFinished;&");
            data = recvall();
        } while (data.equals("No"));
        if (data.equals("Yes"))
            return;
        handleErrors(data);
    }

    public void holdButton(int xPosition, int yPosition, float durationInSecs) throws Exception {
        swipe(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    public void holdButtonAndWait(int xPosition, int yPosition, float durationInSecs) throws Exception {
        swipeAndWait(xPosition, yPosition, xPosition, yPosition, durationInSecs);
    }

    public AltUnityObject clickScreen(float x, float y) throws Exception {
        send("clickScreenOnXY;" + x + ";" + y + ";&");
        String data = recvall();
        if (!data.contains("error:")) return (new Gson().fromJson(data, AltUnityObject.class));
        handleErrors(data);
        return null;
    }

    public void tilt(int x, int y, int z) throws Exception {
        String accelerationString = vectorToJsonString(x, y, z);
        send("tilt;" + accelerationString + ";&");
        String data = recvall();
        if (data.equals("OK")) return;
        handleErrors(data);
    }

    public AltUnityObject findElementWhereNameContains(String name, String cameraName) throws Exception {
        send("findObjectWhereNameContains;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
           return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElementWhereNameContains(String name) throws Exception {
        return findElementWhereNameContains(name, "");
    }

    public AltUnityObject[] getAllElements(String cameraName) throws Exception {
        send("findAllObjects;" + ";" + cameraName + "&");
        String data = recvall();
        if (!data.contains("error:")) return (new Gson().fromJson(data, AltUnityObject[].class));
        handleErrors(data);
        return null;
    }

    public AltUnityObject[] getAllElements() throws Exception {
        return getAllElements("");
    }


    public AltUnityObject findElement(String name, String cameraName) throws Exception {
        send("findObjectByName;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
           return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElement(String name) throws Exception {
        return findElement(name, "");
    }

    public AltUnityObject[] findElements(String name, String cameraName) throws Exception {
        send("findObjectsByName;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) return new Gson().fromJson(data, AltUnityObject[].class);
        handleErrors(data);
        return null;
    }

    public AltUnityObject[] findElements(String name) throws Exception {
        return findElements(name, "");
    }


    public AltUnityObject[] findElementsWhereNameContains(String name, String cameraName) throws Exception {
        send("findObjectsWhereNameContains;" + name + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) return new Gson().fromJson(data, AltUnityObject[].class);
        handleErrors(data);
        return null;
    }

    public AltUnityObject tapScreen(int x, int y) throws Exception {
        send("tapScreen;" + x + ";" + y + ";&");
        String data = recvall();
        if (!data.contains("error:")) return new Gson().fromJson(data, AltUnityObject.class);
        handleErrors(data);
        return null;
    }

    public AltUnityObject[] findElementsWhereNameContains(String name) throws Exception {
        return findElementsWhereNameContains(name, "");
    }


    public String waitForCurrentSceneToBe(String sceneName, double timeout, double interval) throws Exception {
        double time = 0;
        String currentScene = "";
        while (time < timeout) {
            currentScene = getCurrentScene();
            if (!currentScene.equals(sceneName)) {
                System.out.println("Waiting for scene to be " + sceneName + "...");
                Thread.sleep((long) (interval * 1000));
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

    public AltUnityObject waitForElementWhereNameContains(String name,String cameraName, double timeout, double interval) throws Exception {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            try {
                altElement = findElementWhereNameContains(name,cameraName);
                break;
            } catch (Exception e) {
                System.out.println("Waiting for element where name contains " + name + "....");
                Thread.sleep((long) (interval * 1000));
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


    public void waitForElementToNotBePresent(String name,String cameraName, double timeout, double interval) throws Exception {
        double time = 0;
        AltUnityObject altElement = null;
        while (time <= timeout) {
            try {
                altElement = findElement(name,cameraName);
                break;
            } catch (Exception e) {
                Thread.sleep((long) (interval * 1000));
                time += interval;
                System.out.println("Waiting for element " + name + " to not be present");
            }
        }

        if (!altElement.equals(null))
            throw new Exception("Element " + name + " still not found after " + timeout + " seconds");
    }

    public void waitForElementToNotBePresent(String name) throws Exception {
        waitForElementToNotBePresent(name,"", 20, 0.5);
    }


    public AltUnityObject waitForElement(String name,String cameraName, double timeout, double interval) throws Exception {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            try {
                altElement = findElement(name,cameraName);
                break;
            } catch (Exception e) {
                Thread.sleep((long) (interval * 1000));
                time += interval;
                System.out.println("Waiting for element " + name + "...");

            }
        }

        if (altElement != null) {
            return altElement;
        }
        throw new WaitTimeOutException("Element " + name + " not loaded after " + timeout + " seconds");
    }

    public AltUnityObject waitForElement(String name) throws Exception {
        return waitForElement(name,"", 20, 0.5);
    }

    public AltUnityObject waitForElementWithText(String name, String text,String cameraName, double timeout, double interval) throws Exception {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout) {
            try {
                altElement = findElement(name, cameraName);
                if (altElement.getText().equals(text))
                    break;
                throw new Exception("Not the wanted text");
            } catch (Exception e) {
                Thread.sleep((long) (interval * 1000));
                time += interval;
                System.out.println("Waiting for element " + name + " to have text " + text);
            }
        }
        if (altElement.getText().equals(text)) {
            return altElement;
        }
        throw new WaitTimeOutException("Element with text: " + text + " not loaded after " + timeout + " seconds");
    }

    public AltUnityObject waitForElementWithText(String name, String text) throws Exception {
        return waitForElementWithText(name, text,"", 20, 0.5);
    }


    public AltUnityObject findElementByComponent(String componentName,String assemblyName, String cameraName) throws Exception {
        send("findObjectByComponent;"+assemblyName+";" + componentName + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }

    public AltUnityObject findElementByComponent(String componentName) throws Exception {
        return findElementByComponent(componentName, "","");
    }
    public AltUnityObject findElementByComponent(String componentName,String assemblyName) throws Exception{
        return findElementByComponent(componentName,assemblyName,"");
    }



    public AltUnityObject[] findElementsByComponent(String componentName,String assemblyName, String cameraName) throws Exception {
        send("findObjectsByComponent;"+assemblyName+";"  + componentName + ";" + cameraName + ";&");
        String data = recvall();
        if (!data.contains("error:")) return new Gson().fromJson(data, AltUnityObject[].class);
        handleErrors(data);
        return null;
    }

    public AltUnityObject[] findElementsByComponent(String componentName) throws Exception {
        return findElementsByComponent(componentName, "","");
    }
    public AltUnityObject[] findElementsByComponent(String componentName,String assemblyName) throws Exception{
        return findElementsByComponent(componentName,assemblyName,"");
    }


    public String vectorToJsonString(int x, int y) {
        return "{\"x\":" + x + ", \"y\":" + y + "}";
    }

    public String vectorToJsonString(int x, int y, int z) {
        return "{\"x\":" + x + ", \"y\":" + y + ", \"z\":" + z + "}";
    }


    public void handleErrors(String data) throws Exception {
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

    public static void setupPortForwarding(String platform, int tcp_port) {
        System.out.println("Setting up port forward for " + platform + " on port " + tcp_port);
        removePortForwarding();
        if (platform.equals("android")) {
            try {
                Runtime.getRuntime().exec("adb forward tcp:" + tcp_port + " tcp:" + tcp_port);
                Thread.sleep(1000);
                System.out.println("adb forward enabled.");
            } catch (Exception e) {
                System.out.println("AltUnityServer - abd probably not installed\n" + e);
            }

        } else if (platform.equals("ios")) {
            try {
                Runtime.getRuntime().exec("iproxy " + tcp_port + " " + tcp_port + "&");
                Thread.sleep(1000);
                System.out.println("iproxy forward enabled.");
            } catch (Exception e) {
                System.out.println("AltUnityServer - no iproxy process was running/present\n" + e);
            }
        }
    }

    public static void removePortForwarding() {
        try {
            Runtime.getRuntime().exec("killall iproxy");
            Thread.sleep(1000);
            System.out.println("Killed any iproxy process that may have been running...");
        } catch (Exception e) {
            System.out.println("AltUnityServer - no iproxy process was running/present\n" + e);
        }

        try {
            Runtime.getRuntime().exec("adb forward --remove-all");
            Thread.sleep(1000);
            System.out.println("Removed existing adb forwarding...");
        } catch (Exception e) {
            System.out.println("AltUnityServer - abd probably not installed\n" + e);
        }
    }
}
