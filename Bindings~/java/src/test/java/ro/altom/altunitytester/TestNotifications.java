package ro.altom.altunitytester;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityAddNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityRemoveNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.NotificationType;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltCallComponentMethodParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltUnloadSceneParams;
import ro.altom.altunitytester.Logging.AltUnityLogLevel;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class TestNotifications {
    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver(TestsHelper.GetAltUnityDriverHost(), TestsHelper.GetAltUnityDriverPort(),
                true);
        AltUnityAddNotificationListenerParams altUnitySetNotificationParams = new AltUnityAddNotificationListenerParams.Builder(
                NotificationType.LOADSCENE, new MockNotificationCallBacks()).build();
        AltUnityAddNotificationListenerParams altUnitySetNotificationParams2 = new AltUnityAddNotificationListenerParams.Builder(
                NotificationType.UNLOADSCENE, new MockNotificationCallBacks()).build();
        AltUnityAddNotificationListenerParams altUnitySetNotificationParams3 = new AltUnityAddNotificationListenerParams.Builder(
                NotificationType.LOG, new MockNotificationCallBacks()).build();
        AltUnityAddNotificationListenerParams altUnitySetNotificationParams4 = new AltUnityAddNotificationListenerParams.Builder(
                NotificationType.APPLICATION_PAUSED, new MockNotificationCallBacks()).build();
        altUnityDriver.addNotification(altUnitySetNotificationParams);
        altUnityDriver.addNotification(altUnitySetNotificationParams2);
        altUnityDriver.addNotification(altUnitySetNotificationParams3);
        altUnityDriver.addNotification(altUnitySetNotificationParams4);
    }

    @AfterClass
    public static void tearDown() throws Exception {

        AltUnityRemoveNotificationListenerParams altUnitySetNotificationParams = new AltUnityRemoveNotificationListenerParams.Builder(
                NotificationType.LOADSCENE).build();
        AltUnityRemoveNotificationListenerParams altUnitySetNotificationParams2 = new AltUnityRemoveNotificationListenerParams.Builder(
                NotificationType.UNLOADSCENE).build();
        AltUnityRemoveNotificationListenerParams altUnitySetNotificationParams3 = new AltUnityRemoveNotificationListenerParams.Builder(
                NotificationType.LOG).build();
        AltUnityRemoveNotificationListenerParams altUnitySetNotificationParams4 = new AltUnityRemoveNotificationListenerParams.Builder(
                NotificationType.APPLICATION_PAUSED).build();
        altUnityDriver.removeNotificationListener(altUnitySetNotificationParams);
        altUnityDriver.removeNotificationListener(altUnitySetNotificationParams2);
        altUnityDriver.removeNotificationListener(altUnitySetNotificationParams3);
        altUnityDriver.removeNotificationListener(altUnitySetNotificationParams4);
        if (altUnityDriver != null) {
            altUnityDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {

        altUnityDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltUnityDriverTestScene").build());
    }

    @Test
    public void testLodeNonExistentScene() throws Exception {
        assertEquals("Scene 1 AltUnityDriverTestScene", MockNotificationCallBacks.lastLoadedScene);
    }

    private void waitForNotificationToBeSent(String lastSceneLoaded, String expectedValue, float timeout)
            throws Exception {
        while (!lastSceneLoaded.equals(expectedValue)) {
            Thread.sleep(200);
            timeout -= 0.2f;
            if (timeout <= 0)
                throw new WaitTimeOutException("Notification variable not set to the desired value in time");
        }
    }

    @Test
    public void testUnloadSceneNotification() throws Exception {
        altUnityDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").loadSingle(false).build());
        altUnityDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 2 Draggable Panel").build());
        waitForNotificationToBeSent(MockNotificationCallBacks.lastUnloadedScene, "Scene 2 Draggable Panel", 10);
        assertEquals("Scene 2 Draggable Panel", MockNotificationCallBacks.lastUnloadedScene);
    }

    @Test
    public void testLogNotification() {
        assertTrue(MockNotificationCallBacks.logMessage.contains("Scene Loaded"));
        assertEquals(AltUnityLogLevel.Debug, MockNotificationCallBacks.logLevel);
    }

    @Test
    public void testApplicationPausedNotification() throws Exception {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME,
                "AltUnityRunnerPrefab").build();
        AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);

        altElement.callComponentMethod(new AltCallComponentMethodParams.Builder("Altom.AltUnityTester.AltUnityRunner",
                "OnApplicationPause", new Object[] { true }).build(), Void.class);
    }

}
