//package ro.altom.unit;
//
//import org.junit.After;
//import org.junit.AfterClass;
//import org.junit.Assert;
//import org.junit.BeforeClass;
//import org.junit.Test;
//import org.mockito.Mockito;
//import ro.altom.altunitytester.AltUnityDriver;
//import ro.altom.altunitytester.AltUnityObject;
//import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;
//import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;
//
//public class WaitForElementsTests {
//    private static final int PORT = 15000;
//    public static final double TIMEOUT = 0.01d;
//    public static final double INTERVAL = TIMEOUT / 2;
//    private static AltUnityDriver spyAltDriver;
//    private static DummyServer dummyServer;
//    private final String elementName = "nameOfTheElement";
//    private final String cameraName = "cameraName";
//    private final Boolean enabled=true;
//
//    @BeforeClass
//    public static void setup() {
//        dummyServer = DummyServer.onPort(PORT);
//        dummyServer.start();
//        spyAltDriver = Mockito.spy(new AltUnityDriver("127.0.0.1", PORT));
//    }
//
//    @After
//    public void prepareSpy() {
//        // Reset mockito verifications
//        Mockito.reset(spyAltDriver);
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
//    @Test(expected = AltUnityException.class)
//    public void waitForElementNotToBePresentWhileElementIsPresentTest() {
//        // GIVEN
//        Mockito.doReturn(getDummyObject()).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElementToNotBePresent(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//    }
//
//    private AltUnityObject getDummyObject() {
//        return new AltUnityObject("", 0, 0, 0, 0, 0, "", true, 0, 0, 0, 0);
//    }
//
//    @Test
//    public void waitForElementNotToBePresentWhileElementIsGoneTest() {
//        // GIVEN
//        Mockito.doReturn(null).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElementToNotBePresent(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test
//    public void waitForElementNotToBePresentExceptionTest() {
//        // GIVEN
//        Mockito.doThrow(AltUnityException.class).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElementToNotBePresent(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementNullIsReturnedTest() {
//        // GIVEN
//        Mockito.doReturn(null).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElement(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test
//    public void waitForElementTest() {
//        // GIVEN
//        AltUnityObject expectedElement = getDummyObject();
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        AltUnityObject equalElement = spyAltDriver.waitForElement(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//
//        // THEN
//        Assert.assertEquals("Expected and returned element are different.", expectedElement, equalElement);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementExceptionTest() {
//        // GIVEN
//        Mockito.doThrow(AltUnityException.class).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElement(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//    }
//
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWithTextNullIsReturnedTest() {
//        // GIVEN
//        Mockito.doReturn(null).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        String textInElement = "textInElement";
//
//        // WHEN
//        spyAltDriver.waitForElementWithText(elementName, textInElement, cameraName, enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test
//    public void waitForElementWithTextTest() {
//        // GIVEN
//        AltUnityObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        final String textInElement = "";
//        Mockito.doReturn(textInElement).when(expectedElement).getText();
//
//        // WHEN
//        AltUnityObject equalElement = spyAltDriver.waitForElementWithText(elementName, textInElement, cameraName, enabled, TIMEOUT, INTERVAL);
//
//        // THEN
//        Assert.assertEquals("Expected and returned element are different.", expectedElement, equalElement);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWithTextButWrongTextReceivedTest() {
//        // GIVEN
//        AltUnityObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        final String textThatIsNotInElement = "someRandomSampleText";
//        Mockito.doReturn("text in element").when(expectedElement).getText();
//
//        // WHEN
//        AltUnityObject equalElement = spyAltDriver.waitForElementWithText(elementName, textThatIsNotInElement, cameraName, enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWithTextExceptionTest() {
//        // GIVEN
//        Mockito.doThrow(AltUnityException.class).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        String testInElement = "testInElement";
//        final String textInElement = "textInElement";
//
//        // WHEN
//        spyAltDriver.waitForElementWithText(elementName, textInElement, cameraName, enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWhereNameContainsNullIsReturnedTest() {
//        // GIVEN
//        Mockito.doReturn(null).when(spyAltDriver).findElementWhereNameContains(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElementWhereNameContains(elementName, cameraName, enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test
//    public void waitForElementWhereNameContainsTest() {
//        // GIVEN
//        AltUnityObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElementWhereNameContains(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        final String textInElement = "";
//        Mockito.doReturn(textInElement).when(expectedElement).getText();
//
//        // WHEN
//        AltUnityObject equalElement = spyAltDriver.waitForElementWhereNameContains(elementName, cameraName, enabled, TIMEOUT, INTERVAL);
//
//        // THEN
//        Assert.assertEquals("Expected and returned element are different.", expectedElement, equalElement);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWhereNameContainsExceptionTest() {
//        // GIVEN
//        Mockito.doThrow(AltUnityException.class).when(spyAltDriver).findElementWhereNameContains(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElementWhereNameContains(elementName, cameraName, enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForCurrentSceneToBeNullIsReturnedTest() {
//        // GIVEN
//        Mockito.doReturn(null).when(spyAltDriver).getCurrentScene();
//
//        // WHEN
//        spyAltDriver.waitForCurrentSceneToBe("sceneToWaitFor", TIMEOUT, INTERVAL);
//    }
//
//    @Test
//    public void waitForCurrentSceneToBeTest() {
//        // GIVEN
//        AltUnityObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn("sceneToWaitFor").when(spyAltDriver).getCurrentScene();
//
//        // WHEN
//        spyAltDriver.waitForCurrentSceneToBe("sceneToWaitFor", TIMEOUT, INTERVAL);
//    }
//
//    @Test (expected = WaitTimeOutException.class)
//    public void waitForCurrentSceneToBeButWrongSceneReturnedTest() {
//        // GIVEN
//        AltUnityObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn("notExpectedScene").when(spyAltDriver).getCurrentScene();
//
//        // WHEN
//        spyAltDriver.waitForCurrentSceneToBe("sceneToWaitFor", TIMEOUT, INTERVAL);
//    }
//
//    @Test(expected = AltUnityException.class)
//    public void waitForCurrentSceneToBeExceptionThrownTest() {
//        // GIVEN
//        Mockito.doThrow(AltUnityException.class).when(spyAltDriver).getCurrentScene();
//
//        // WHEN
//        spyAltDriver.waitForCurrentSceneToBe("sceneToWaitFor", TIMEOUT, INTERVAL);
//    }
//
//}
