package ro.altom.altunitytester;

import static org.junit.Assert.assertEquals;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityAddNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltUnityRemoveNotificationListenerParams;
import ro.altom.altunitytester.Commands.AltUnityCommands.NotificationType;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class TestNotifications {
    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13010, true);
        AltUnityAddNotificationListenerParams altUnitySetNotificationParams = new AltUnityAddNotificationListenerParams.Builder(
                NotificationType.LOADSCENE, new MockNotificationCallBacks()).build();
        AltUnityAddNotificationListenerParams altUnitySetNotificationParams2 = new AltUnityAddNotificationListenerParams.Builder(
                NotificationType.UNLOADSCENE, new MockNotificationCallBacks()).build();
        altUnityDriver.AddNotification(altUnitySetNotificationParams);
        altUnityDriver.AddNotification(altUnitySetNotificationParams2);
    }

    @AfterClass
    public static void tearDown() throws Exception {

        AltUnityRemoveNotificationListenerParams altUnitySetNotificationParams = new AltUnityRemoveNotificationListenerParams.Builder(
                NotificationType.LOADSCENE).build();
        AltUnityRemoveNotificationListenerParams altUnitySetNotificationParams2 = new AltUnityRemoveNotificationListenerParams.Builder(
                NotificationType.UNLOADSCENE).build();
        altUnityDriver.RemoveNotificationListener(altUnitySetNotificationParams);
        altUnityDriver.RemoveNotificationListener(altUnitySetNotificationParams2);
        if (altUnityDriver != null) {
            altUnityDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {

        altUnityDriver.loadScene(new AltLoadSceneParameters.Builder("Scene 1 AltUnityDriverTestScene").build());
    }

    @Test
    public void testLodeNonExistentScene() throws Exception {
        assertEquals("Scene 1 AltUnityDriverTestScene", MockNotificationCallBacks.lastLoadedScene);

    }

    @Test
    public void testUnloadSceneNotification() throws Exception{
        altUnityDriver.loadScene(new AltLoadSceneParameters.Builder("Scene 2 Draggable Panel").loadMode(false).build());
        altUnityDriver.unloadScene("Scene 2 Draggable Panel");
        waitForNotificationToBeSent(MockNotificationCallBacks.lastUnloadedScene, "Scene 2 Draggable Panel", 10);
        assertEquals("Scene 2 Draggable Panel", MockNotificationCallBacks.lastUnloadedScene);
    }

    private void waitForNotificationToBeSent(String lastSceneLoaded, String expectedValue, float timeout) throws Exception
    {
        while (!lastSceneLoaded.equals(expectedValue))
        {
            Thread.sleep(200);
            timeout -= 0.2f;
            if (timeout <= 0)
                throw new WaitTimeOutException("Notification variable not set to the desired value in time");
        }
    }

}
