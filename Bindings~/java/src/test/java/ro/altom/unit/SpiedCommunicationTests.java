//package ro.altom.unit;
//
//import org.junit.After;
//import org.junit.AfterClass;
//import org.junit.Assert;
//import org.junit.BeforeClass;
//import org.junit.Test;
//import org.mockito.ArgumentCaptor;
//import org.mockito.Mockito;
//import ro.altom.altunitytester.AltUnityDriver;
//
//public class SpiedCommunicationTests {
//    private static final String FIND_OBJECT_BY_NAME_MESSAGE_PATTERN = "findObjectByName;%s;%s;true;&";
//    private static final String CLOSE_CONNECTION_MESSAGE = "closeConnection;&";
//    private static final String LOAD_SCENE_MESSAGE_PATTERN = "loadScene;%s;&";
//    private static final int PORT = 15000;
//    private static AltUnityDriver spyAltDriver;
//    private static ArgumentCaptor<String> captor;
//    private static DummyServer dummyServer;
//
//    @BeforeClass
//    public static void setup() {
//        dummyServer = DummyServer.onPort(PORT);
//        dummyServer.start();
//        spyAltDriver = Mockito.spy(new AltUnityDriver("127.0.0.1", PORT));
//        prepareStubbing();
//    }
//
//    @After
//    public void prepareSpy() {
//        // Reset mockito verifications
//        Mockito.reset(spyAltDriver);
//        prepareStubbing();
//    }
//
//    private static void prepareStubbing() {
//        // Don't send anything
//        Mockito.doNothing().when(spyAltDriver).send(Mockito.anyString());
//        // Don't evaluate response since nothing will be returned
//        Mockito.doReturn("").when(spyAltDriver).recvall();
//        captor = ArgumentCaptor.forClass(String.class);
//    }
//
//    @AfterClass
//    public static void tearDown() {
//        if (spyAltDriver != null) {
//            spyAltDriver.stop();
//        }
//        if (dummyServer != null) {
//            dummyServer.stop();
//        }
//    }
//
//    @Test
//    public void findElementWithCameraTest() {
//        // WHEN
//        String elementName = "element";
//        String cameraName = "camera";
//        spyAltDriver.findElement(elementName, cameraName);
//        Mockito.verify(spyAltDriver, Mockito.times(1)).send(captor.capture());
//        // THEN
//        Assert.assertEquals(String.format(FIND_OBJECT_BY_NAME_MESSAGE_PATTERN, elementName, cameraName), captor.getValue());
//    }
//
//    @Test
//    public void closeConnectionTest() {
//        // WHEN
//        spyAltDriver.stop();
//        Mockito.verify(spyAltDriver, Mockito.times(1)).send(captor.capture());
//        // THEN
//        Assert.assertEquals(CLOSE_CONNECTION_MESSAGE, captor.getValue());
//    }
//
//    @Test
//    public void loadSceneTest() {
//        // WHEN
//        String sceneToLoad = "sceneToLoad";
//        spyAltDriver.loadScene(sceneToLoad);
//        Mockito.verify(spyAltDriver, Mockito.times(1)).send(captor.capture());
//        // THEN
//        Assert.assertEquals(String.format(LOAD_SCENE_MESSAGE_PATTERN, sceneToLoad), captor.getValue());
//    }
//}
