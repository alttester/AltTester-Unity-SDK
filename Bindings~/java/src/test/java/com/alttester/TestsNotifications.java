/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

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

    @BeforeAll
    public static void setUp() throws Exception {
        altDriver = TestsHelper.getAltDriver();
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

    @AfterAll
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

    @BeforeEach
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

        altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder("AltTester.AltTesterUnitySDK.Commands.AltRunner",
                        "OnApplicationPause", "AltTester.AltTesterUnitySDK", new Object[] { true }).build(),
                Void.class);
    }

}
