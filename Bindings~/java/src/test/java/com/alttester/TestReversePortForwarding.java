/*
    Copyright(C) 2025 Altom Consulting

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

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;

import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.FindObject.AltWaitForObjectsParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;

import java.io.IOException;

public class TestReversePortForwarding {

    private static AltDriver altDriver;

    @BeforeAll
    public static void setUp() throws IOException {
        AltReversePortForwarding.reversePortForwardingAndroid();
        altDriver = new AltDriver();
    }

    @AfterAll
    public static void tearDown() throws Exception {
        altDriver.stop();
        AltReversePortForwarding.removeReversePortForwardingAndroid();
    }

    @Test
    public void openClosePanelTest() {
        altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").build());

        AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(
                AltDriver.By.PATH, "//Main Camera")
                .build();
        AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);

        AltFindObjectsParams closeButtonObjectsParameters = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Close Button")
                .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                .build();
        altDriver.findObject(closeButtonObjectsParameters).tap();

        AltFindObjectsParams buttonObjectsParameters = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Button")
                .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                .build();
        altDriver.findObject(buttonObjectsParameters).tap();

        AltFindObjectsParams panelObjectsParameters = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Panel")
                .withCamera(AltDriver.By.ID, String.valueOf(camera.id))
                .build();
        AltWaitForObjectsParams panelWaitForObjectsParameters = new AltWaitForObjectsParams.Builder(
                panelObjectsParameters).build();
        AltObject panelElement = altDriver.waitForObject(panelWaitForObjectsParameters);

        Assertions.assertTrue(panelElement.isEnabled());
    }
}
