package com.alttester;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;

import java.util.Arrays;

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
        AltDriver altDriver = TestsHelper.getAltDriver();

        @Test
        public void testLodeNonExistentScene() {
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
                TestsHelper.addNotifications(altDriver, Arrays.asList(NotificationType.values()));

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
        public void testApplicationPausedNotification() {
                TestsHelper.addNotifications(altDriver, Arrays.asList(NotificationType.values()));
                AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(
                                AltDriver.By.NAME,
                                "AltTesterPrefab").build();
                AltObject altElement = altDriver.findObject(altFindObjectsParameters);

                altElement.callComponentMethod(new AltCallComponentMethodParams.Builder("Altom.AltTester.AltRunner",
                                "OnApplicationPause", "Assembly-CSharp", new Object[] { true }).build(), Void.class);
        }

}
