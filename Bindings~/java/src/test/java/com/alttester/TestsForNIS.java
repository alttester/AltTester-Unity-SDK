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

import java.util.ArrayList;
import java.util.List;

import com.alttester.AltDriver.By;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.InputActions.AltMultiPointSwipeParams;
import com.alttester.Commands.InputActions.AltPressKeyParams;
import com.alttester.Commands.InputActions.AltPressKeysParams;
import com.alttester.Commands.InputActions.AltScrollParams;
import com.alttester.Commands.InputActions.AltSwipeParams;
import com.alttester.Commands.InputActions.AltKeyDownParams;
import com.alttester.Commands.InputActions.AltKeyUpParams;
import com.alttester.Commands.InputActions.AltMoveMouseParams;
import com.alttester.Commands.InputActions.AltTapClickCoordinatesParams;
import com.alttester.Commands.InputActions.AltTiltParams;
import com.alttester.Commands.ObjectCommand.AltTapClickElementParams;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.Commands.FindObject.AltWaitForObjectsParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;
import com.alttester.UnityStruct.AltKeyCode;
import com.alttester.position.Vector2;
import com.alttester.position.Vector3;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import org.junit.jupiter.api.Test;

public class TestsForNIS extends BaseTest {
    String scene7 = "Assets/Examples/Scenes/Scene 7 Drag And Drop NIS.unity";
    String scene8 = "Assets/Examples/Scenes/Scene 8 Draggable Panel NIP.unity";
    String scene9 = "Assets/Examples/Scenes/scene 9 NIS.unity";
    String scene10 = "Assets/Examples/Scenes/Scene 10 Sample NIS.unity";
    String scene11 = "Assets/Examples/Scenes/Scene 7 New Input System Actions.unity";

    public static class ImagesDrop {
        public static String imageSource;
        public static String imageSourceDropZone;

        public ImagesDrop(String imageSource, String imageSourceDropZone) {
            this.imageSource = imageSource;
            this.imageSourceDropZone = imageSourceDropZone;
        }
    }

    class AltSprite {
        public String name;
    }

    public void loadLevel(AltDriver altDriver, String sceneName) {
        AltLoadSceneParams params = new AltLoadSceneParams.Builder(sceneName).build();
        altDriver.loadScene(params);
    }

    private void dropImageWithMultipointSwipe(List<String> objectNames, float duration, boolean wait) {
        List<Vector2> listPositions = new ArrayList<Vector2>();
        for (int i = 0; i < objectNames.size(); i++) {
            AltFindObjectsParams elementParams = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, objectNames.get(i)).build();
            AltObject element = altDriver.findObject(elementParams);
            listPositions.add(element.getScreenPosition());
        }

        altDriver.multipointSwipe(
                new AltMultiPointSwipeParams.Builder(listPositions).withDuration(duration)
                        .withWait(wait).build());
    }

    private ImagesDrop getSpriteName(String sourceImageName, String imageSourceDropZoneName) {

        AltFindObjectsParams imageSourceParams = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, sourceImageName).build();
        AltFindObjectsParams imageSourceDropZoneParams = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, imageSourceDropZoneName).build();

        String imageSource = altDriver.findObject(imageSourceParams).getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name",
                        "UnityEngine.UI").build(),
                String.class);
        String imageSourceDropZone = altDriver.findObject(imageSourceDropZoneParams).getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name",
                        "UnityEngine.UI").build(),
                String.class);

        return new ImagesDrop(imageSource, imageSourceDropZone);
    }

    @Test
    public void TestScroll() {
        loadLevel(altDriver, scene10);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Player").build();
        AltObject player = altDriver.findObject(altFindObjectsParams);
        Boolean isScrolling = player
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("AltNIPDebugScript",
                                "wasScrolled", "Assembly-CSharp")
                                .build(),
                        Boolean.class);
        assertFalse(isScrolling);
        AltScrollParams altScrollParams = new AltScrollParams.Builder().withDuration(1).withSpeed(300)
                .withWait(true).build();
        altDriver.scroll(altScrollParams);
        isScrolling = player.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltNIPDebugScript",
                        "wasScrolled", "Assembly-CSharp")
                        .build(),
                Boolean.class);
        assertTrue(isScrolling);
    }

    @Test
    public void TestTapElement() throws Exception {
        String componentName = "AltExampleNewInputSystem";
        String propertyName = "jumpCounter";
        String assembly = "Assembly-CSharp";
        loadLevel(altDriver, scene11);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject capsule = altDriver.findObject(altFindObjectsParams);
        AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
        capsule.tap(tapParams);
        int propertyValue = capsule.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName, assembly).build(),
                int.class);
        assertEquals(1, propertyValue);
    }

    @Test
    public void TestTapCoordinates() {
        loadLevel(altDriver, scene11);
        AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject capsule = altDriver.findObject(findCapsuleParams);
        AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
                capsule.getScreenPosition()).build();
        altDriver.tap(tapParams);
        AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                "//ActionText[@text=Capsule was tapped!]").build();
        AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                .build();
        altDriver.waitForObject(waitParams);
    }

    @Test
    public void TestScrollElement() {
        loadLevel(altDriver, scene9);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Scrollbar Vertical").build();
        AltObject scrollbar = altDriver.findObject(altFindObjectsParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI").build();
        Float scrollbarPosition = scrollbar.getComponentProperty(altGetComponentPropertyParams, Float.class);
        AltFindObjectsParams altFindObjectsParamsScrollView = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME,
                "Scroll View").build();

        AltObject scrollView = altDriver.findObject(altFindObjectsParamsScrollView);
        AltMoveMouseParams altMoveMouseParams = new AltMoveMouseParams.Builder(
                scrollView.getScreenPosition())
                .withDuration(1f).build();
        altDriver.moveMouse(altMoveMouseParams);
        AltScrollParams altScrollParams = new AltScrollParams.Builder().withDuration(1).withSpeed(-300)
                .withWait(true).build();
        altDriver.scroll(altScrollParams);
        AltObject scrollbarFinal = altDriver.findObject(altFindObjectsParams);
        Float scrollbarPositionFinal = scrollbarFinal.getComponentProperty(altGetComponentPropertyParams,
                Float.class);
        assertNotEquals(scrollbarPosition, scrollbarPositionFinal);
    }

    @Test
    public void TestClickElement() {
        String componentName = "AltExampleNewInputSystem";
        String propertyName = "jumpCounter";
        String assembly = "Assembly-CSharp";
        loadLevel(altDriver, scene11);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
        AltObject capsule = altDriver.findObject(altFindObjectsParams);
        AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
        capsule.click(tapParams);
        int propertyValue = capsule.getComponentProperty(
                new AltGetComponentPropertyParams.Builder(componentName,
                        propertyName, assembly).build(),
                int.class);
        assertEquals(1, propertyValue);
    }

    @Test
    public void TestClickCoordinates() {
        loadLevel(altDriver, scene11);
        AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                .build();
        AltObject capsule = altDriver.findObject(findCapsuleParams);
        AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
                capsule.getScreenPosition()).build();
        altDriver.click(tapParams);
        AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                "//ActionText[@text=Capsule was clicked!]").build();
        AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                .build();
        altDriver.waitForObject(waitParams);
    }

    @Test
    public void TestTilt() {
        loadLevel(altDriver, scene11);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Cube (1)").build();
        AltObject capsule = altDriver.findObject(altFindObjectsParams);
        Vector3 initialPosition = capsule.getWorldPosition();
        altDriver.tilt(new AltTiltParams.Builder(new Vector3(1000, 10, 10)).withDuration(3f).build());
        assertNotEquals(initialPosition.x, altDriver.findObject(altFindObjectsParams).x);
        Boolean isMoved = capsule.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltCubeNIS",
                        "isMoved", "Assembly-CSharp").build(),
                Boolean.class);
        assertTrue(isMoved);
    }

    @Test
    public void TestKeyDownAndKeyUp() throws InterruptedException {
        loadLevel(altDriver, scene10);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Player").build();
        AltObject player = altDriver.findObject(altFindObjectsParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "UnityEngine.Transform",
                "position", "UnityEngine.CoreModule").build();
        Vector3 initialPos = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.A).build());
        Thread.sleep(1000);
        altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.A).build());
        Vector3 posUp = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(initialPos.x, posUp.x);
        altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.D).build());
        Thread.sleep(1000);
        altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.D).build());
        Vector3 posDown = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(posUp.x, posDown.x);
        altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.W).build());
        Thread.sleep(1000);
        altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.W).build());
        Vector3 posLeft = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(posDown.z, posLeft.z);
        altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.S).build());
        Thread.sleep(1000);
        altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.S).build());
        Vector3 posRight = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(posLeft.z, posRight.z);
    }

    @Test
    public void TestPressKey() {
        loadLevel(altDriver, scene10);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Player").build();
        AltObject player = altDriver.findObject(altFindObjectsParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "UnityEngine.Transform",
                "position", "UnityEngine.CoreModule").build();
        Vector3 initialPos = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.A).withDuration(1).build());
        Vector3 posUp = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(initialPos.x, posUp.x);
        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.D).withDuration(1).build());
        Vector3 posDown = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(posUp.x, posDown.x);
        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.W).withDuration(1).build());
        Vector3 posLeft = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(posDown.z, posLeft.z);
        altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.S).withDuration(1).build());
        Vector3 posRight = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(posLeft.z, posRight.z);
    }

    @Test
    public void TestPressKeys() {
        loadLevel(altDriver, scene10);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Player").build();
        AltObject player = altDriver.findObject(altFindObjectsParams);
        AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "UnityEngine.Transform",
                "position", "UnityEngine.CoreModule").build();
        Vector3 initialPos = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        AltKeyCode[] keys = { AltKeyCode.W, AltKeyCode.Mouse0 };
        altDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());

        Vector3 finalPos = player
                .getComponentProperty(altGetComponentPropertyParams,
                        Vector3.class);
        assertNotEquals(initialPos.z, finalPos.z);
        AltFindObjectsParams findObjectParams = new AltFindObjectsParams.Builder(By.NAME,
                "SimpleProjectile(Clone)").build();
        AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findObjectParams)
                .build();
        altDriver.waitForObject(waitParams);

    }

    @Test
    public void TestMultipointSwipe() {
        loadLevel(altDriver, scene7);
        List<String> objects1 = new ArrayList<String>();
        List<String> objects2 = new ArrayList<String>();
        objects1.add("Drag Image1");
        objects1.add("Drop Box1");

        objects2.add("Drag Image2");
        objects2.add("Drop Box1");
        objects2.add("Drop Box2");

        dropImageWithMultipointSwipe(objects1, 1, true);
        dropImageWithMultipointSwipe(objects2, 1, true);

        getSpriteName("Drag Image1", "Drop Image");
        String imageSource = ImagesDrop.imageSource;
        String imageSourceDropZone = ImagesDrop.imageSourceDropZone;
        assertEquals(imageSource, imageSourceDropZone);

        getSpriteName("Drag Image2", "Drop");
        imageSource = ImagesDrop.imageSource;
        imageSourceDropZone = ImagesDrop.imageSourceDropZone;
        assertEquals(imageSource, imageSourceDropZone);
    }

    @Test
    public void TestSwipe() {
        loadLevel(altDriver, scene9);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.PATH,
                "//Scroll View/Viewport/Content/Button (4)").build();

        AltObject scrollbar = altDriver.findObject(altFindObjectsParams);
        Vector2 scrollbarPosition = scrollbar.getScreenPosition();

        AltFindObjectsParams altFindButtonParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Handle").build();
        AltObject button = altDriver.findObject(altFindButtonParams);

        altDriver
                .swipe(new AltSwipeParams.Builder(new Vector2(button.x, button.y),
                        new Vector2(button.x, button.y + 20))
                        .withDuration(1).build());

        AltFindObjectsParams altFindObjectsParamsFinal = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Handle").build();
        AltObject scrollbarFinal = altDriver.findObject(altFindObjectsParamsFinal);
        Vector2 scrollbarPositionFinal = scrollbarFinal.getScreenPosition();
        assertNotEquals(scrollbarPosition, scrollbarPositionFinal);
    }
}
