//package ro.altom.unit;
//
//import org.junit.After;
//import org.junit.AfterClass;
//import org.junit.Assert;
//import org.junit.BeforeClass;
//import org.junit.Test;
//import org.mockito.Mockito;
//import ro.altom.alttester.AltDriver;
//import ro.altom.alttester.AltObject;
//import ro.altom.alttester.altTesterExceptions.AltException;
//import ro.altom.alttester.altTesterExceptions.WaitTimeOutException;
//
//public class WaitForElementsTests {
//    private static final int PORT = 15000;
//    public static final double TIMEOUT = 0.01d;
//    public static final double INTERVAL = TIMEOUT / 2;
//    private static AltDriver spyAltDriver;
//    private static DummyServer dummyServer;
//    private final String elementName = "nameOfTheElement";
//    private final String cameraName = "cameraName";
//    private final Boolean enabled=true;
//
//    @BeforeClass
//    public static void setup() {
//        dummyServer = DummyServer.onPort(PORT);
//        dummyServer.start();
//        spyAltDriver = Mockito.spy(new AltDriver("127.0.0.1", PORT));
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
//    @Test(expected = AltException.class)
//    public void waitForElementNotToBePresentWhileElementIsPresentTest() {
//        // GIVEN
//        Mockito.doReturn(getDummyObject()).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        spyAltDriver.waitForElementToNotBePresent(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//    }
//
//    private AltObject getDummyObject() {
//        return new AltObject("", 0, 0, 0, 0, 0, "", true, 0, 0, 0, 0);
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
//        Mockito.doThrow(AltException.class).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
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
//        AltObject expectedElement = getDummyObject();
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//
//        // WHEN
//        AltObject equalElement = spyAltDriver.waitForElement(elementName, cameraName,enabled, TIMEOUT, INTERVAL);
//
//        // THEN
//        Assert.assertEquals("Expected and returned element are different.", expectedElement, equalElement);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementExceptionTest() {
//        // GIVEN
//        Mockito.doThrow(AltException.class).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
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
//        AltObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        final String textInElement = "";
//        Mockito.doReturn(textInElement).when(expectedElement).getText();
//
//        // WHEN
//        AltObject equalElement = spyAltDriver.waitForElementWithText(elementName, textInElement, cameraName, enabled, TIMEOUT, INTERVAL);
//
//        // THEN
//        Assert.assertEquals("Expected and returned element are different.", expectedElement, equalElement);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWithTextButWrongTextReceivedTest() {
//        // GIVEN
//        AltObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        final String textThatIsNotInElement = "someRandomSampleText";
//        Mockito.doReturn("text in element").when(expectedElement).getText();
//
//        // WHEN
//        AltObject equalElement = spyAltDriver.waitForElementWithText(elementName, textThatIsNotInElement, cameraName, enabled, TIMEOUT, INTERVAL);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWithTextExceptionTest() {
//        // GIVEN
//        Mockito.doThrow(AltException.class).when(spyAltDriver).findElement(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
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
//        AltObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn(expectedElement).when(spyAltDriver).findElementWhereNameContains(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
//        final String textInElement = "";
//        Mockito.doReturn(textInElement).when(expectedElement).getText();
//
//        // WHEN
//        AltObject equalElement = spyAltDriver.waitForElementWhereNameContains(elementName, cameraName, enabled, TIMEOUT, INTERVAL);
//
//        // THEN
//        Assert.assertEquals("Expected and returned element are different.", expectedElement, equalElement);
//    }
//
//    @Test(expected = WaitTimeOutException.class)
//    public void waitForElementWhereNameContainsExceptionTest() {
//        // GIVEN
//        Mockito.doThrow(AltException.class).when(spyAltDriver).findElementWhereNameContains(Mockito.anyString(), Mockito.anyString(),Mockito.anyBoolean());
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
//        AltObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn("sceneToWaitFor").when(spyAltDriver).getCurrentScene();
//
//        // WHEN
//        spyAltDriver.waitForCurrentSceneToBe("sceneToWaitFor", TIMEOUT, INTERVAL);
//    }
//
//    @Test (expected = WaitTimeOutException.class)
//    public void waitForCurrentSceneToBeButWrongSceneReturnedTest() {
//        // GIVEN
//        AltObject expectedElement = Mockito.spy(getDummyObject());
//        Mockito.doReturn("notExpectedScene").when(spyAltDriver).getCurrentScene();
//
//        // WHEN
//        spyAltDriver.waitForCurrentSceneToBe("sceneToWaitFor", TIMEOUT, INTERVAL);
//    }
//
//    @Test(expected = AltException.class)
//    public void waitForCurrentSceneToBeExceptionThrownTest() {
//        // GIVEN
//        Mockito.doThrow(AltException.class).when(spyAltDriver).getCurrentScene();
//
//        // WHEN
//        spyAltDriver.waitForCurrentSceneToBe("sceneToWaitFor", TIMEOUT, INTERVAL);
//    }
//
//}
