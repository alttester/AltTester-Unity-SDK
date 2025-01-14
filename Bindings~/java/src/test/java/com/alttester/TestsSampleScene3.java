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
import com.alttester.Commands.FindObject.AltWaitForObjectsParams;
import com.alttester.Commands.InputActions.AltMultiPointSwipeParams;
import com.alttester.Commands.InputActions.AltSwipeParams;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.Commands.UnityCommand.AltLoadSceneParams;
import com.alttester.position.Vector2;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotEquals;

public class TestsSampleScene3 extends BaseTest {

        @BeforeEach
        public void loadLevel() {
                altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 3 Drag And Drop").build());
        }

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

        private AltObject FindObject(By by, String name) {
                AltFindObjectsParams altElementParams = new AltFindObjectsParams.Builder(
                                by, name).build();
                return altDriver.findObject(altElementParams);
        }

        private void dropImage(String dragLocationName, String dropLocationName, float duration, boolean wait) {
                AltObject dragLocation = FindObject(AltDriver.By.NAME, dragLocationName);
                AltObject dropLocation = FindObject(AltDriver.By.NAME, dropLocationName);
                altDriver
                                .swipe(new AltSwipeParams.Builder(dragLocation.getScreenPosition(),
                                                dropLocation.getScreenPosition())
                                                .withDuration(duration).withWait(wait).build());
        }

        private void waitForSwipeToFinish() {
                AltFindObjectsParams swipeImageFindObject = new AltFindObjectsParams.Builder(
                                AltDriver.By.NAME, "icon").build();
                AltWaitForObjectsParams swipeImageParam = new AltWaitForObjectsParams.Builder(swipeImageFindObject)
                                .build();
                altDriver.waitForObjectToNotBePresent(swipeImageParam);
        }

        private ImagesDrop getSpriteName(String sourceImageName, String imageSourceDropZoneName) {

                AltFindObjectsParams imageSourceParams = new AltFindObjectsParams.Builder(
                                AltDriver.By.NAME, sourceImageName).build();
                AltFindObjectsParams imageSourceDropZoneParams = new AltFindObjectsParams.Builder(
                                AltDriver.By.NAME, imageSourceDropZoneName).build();

                String imageSource = altDriver.findObject(imageSourceParams).getComponentProperty(

                                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name",
                                                "UnityEngine.UI")
                                                .build(),
                                String.class);
                String imageSourceDropZone = altDriver.findObject(imageSourceDropZoneParams).getComponentProperty(
                                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name",
                                                "UnityEngine.UI")
                                                .build(),
                                String.class);

                return new ImagesDrop(imageSource, imageSourceDropZone);
        }

        @Test
        public void testMultipleDragAndDrop() throws Exception {

                dropImage("Drag Image2", "Drop Box2", 1, false);
                dropImage("Drag Image2", "Drop Box1", 1, false);
                dropImage("Drag Image1", "Drop Box1", 1, false);
                waitForSwipeToFinish();

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
        public void testMultipleDragAndDropWait() throws Exception {

                dropImage("Drag Image2", "Drop Box2", (float) 1, true);
                dropImage("Drag Image2", "Drop Box1", (float) 1, true);
                dropImage("Drag Image1", "Drop Box1", (float) 1, true);
                waitForSwipeToFinish();

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
        public void testTestPointerEnterAndExit() throws Exception {

                AltObject altElement = FindObject(By.NAME, "Drop Image");
                AltColor color1 = altElement.getComponentProperty(
                                new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                                "Assembly-CSharp").build(),
                                AltColor.class);
                FindObject(By.NAME, "Drop Image").pointerEnter();
                AltColor color2 = altElement.getComponentProperty(
                                new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                                "Assembly-CSharp").build(),
                                AltColor.class);
                assertNotEquals(color1, color2);
                FindObject(By.NAME, "Drop Image").pointerEnter();
                AltColor color3 = altElement.getComponentProperty(
                                new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                                "Assembly-CSharp").build(),
                                AltColor.class);

                assertNotEquals(color3, color2);
                assertNotEquals(color1, color3);
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

        @Test
        public void testMultipleDragAndDropWaitWithMultipointSwipe() throws Exception {

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
}
