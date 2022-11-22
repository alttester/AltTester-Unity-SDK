package com.alttester;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import com.alttester.Commands.AltCommands.AltAddNotificationListenerParams;
import com.alttester.Commands.AltCommands.AltRemoveNotificationListenerParams;
import com.alttester.Commands.AltCommands.NotificationType;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.ObjectCommand.AltCallComponentMethodParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;
import com.alttester.Commands.UnityCommand.AltUnloadSceneParams;
import com.alttester.Logging.AltLogLevel;
import com.alttester.altTesterExceptions.WaitTimeOutException;

public class TestsNotifications {
    private static AltDriver altDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altDriver = new AltDriver(TestsHelper.GetAltDriverHost(), TestsHelper.GetAltDriverPort(),
                true);
        AltAddNotificationListenerParams altSetNotificationParams = new AltAddNotificationListenerParams.Builder(
                NotificationType.LOADSCENE, new MockNotificationCallBacks()).build();
        AltAddNotificationListenerParams altSetNotificationParams2 = new AltAddNotificationListenerParams.Builder(
                NotificationType.UNLOADSCENE, new MockNotificationCallBacks()).build();
        AltAddNotificationListenerParams altSetNotificationParams3 = new AltAddNotificationListenerParams.Builder(
                NotificationType.LOG, new MockNotificationCallBacks()).build();
        AltAddNotificationListenerParams altSetNotificationParams4 = new AltAddNotificationListenerParams.Builder(
                NotificationType.APPLICATION_PAUSED, new MockNotificationCallBacks()).build();
        altDriver.addNotification(altSetNotificationParams);
        altDriver.addNotification(altSetNotificationParams2);
        altDriver.addNotification(altSetNotificationParams3);
        altDriver.addNotification(altSetNotificationParams4);
    }

    @AfterClass
    public static void tearDown() throws Exception {

        AltRemoveNotificationListenerParams altSetNotificationParams = new AltRemoveNotificationListenerParams.Builder(
                NotificationType.LOADSCENE).build();
        AltRemoveNotificationListenerParams altSetNotificationParams2 = new AltRemoveNotificationListenerParams.Builder(
                NotificationType.UNLOADSCENE).build();
        AltRemoveNotificationListenerParams altSetNotificationParams3 = new AltRemoveNotificationListenerParams.Builder(
                NotificationType.LOG).build();
        AltRemoveNotificationListenerParams altSetNotificationParams4 = new AltRemoveNotificationListenerParams.Builder(
                NotificationType.APPLICATION_PAUSED).build();
        altDriver.removeNotificationListener(altSetNotificationParams);
        altDriver.removeNotificationListener(altSetNotificationParams2);
        altDriver.removeNotificationListener(altSetNotificationParams3);
        altDriver.removeNotificationListener(altSetNotificationParams4);
        if (altDriver != null) {
            altDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {

        altDriver.resetInput();
        altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
    }

    @Test
    public void testLodeNonExistentScene() throws Exception {
        assertEquals("Scene 1 AltDriverTestScene", MockNotificationCallBacks.lastLoadedScene);
    }

    private void waitForNotificationToBeSent(String lastSceneLoaded, String expectedValue, float timeout)
            throws Exception {
        while (!lastSceneLoaded.equals(expectedValue)) {
            Thread.sleep(200);
            timeout -= 0.2f;
            if (timeout <= 0)
                throw new WaitTimeOutException(
                        "Notification variable not set to the desired value in time");
        }
    }

    @Test
    public void testUnloadSceneNotification() throws Exception {
        altDriver.loadScene(
                new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").loadSingle(false).build());
        altDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 2 Draggable Panel").build());
        waitForNotificationToBeSent(MockNotificationCallBacks.lastUnloadedScene, "Scene 2 Draggable Panel", 10);
        assertEquals("Scene 2 Draggable Panel", MockNotificationCallBacks.lastUnloadedScene);
    }

    @Test
    public void testLogNotification() {
        assertTrue(MockNotificationCallBacks.logMessage.contains("Scene Loaded"));
        assertEquals(AltLogLevel.Debug, MockNotificationCallBacks.logLevel);
    }

    @Test
    public void testApplicationPausedNotification() throws Exception {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME,
                "AltTesterPrefab").build();
        AltObject altElement = altDriver.findObject(altFindObjectsParameters);

        altElement.callComponentMethod(new AltCallComponentMethodParams.Builder("Altom.AltTester.AltRunner",
                "OnApplicationPause", "Assembly-CSharp", new Object[] { true }).build(), Void.class);
    }

}
