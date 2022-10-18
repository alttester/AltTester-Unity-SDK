package ro.altom.alttester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.alttester.AltDriver.By;
import ro.altom.alttester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.alttester.Commands.FindObject.AltWaitForObjectsParams;
import ro.altom.alttester.Commands.InputActions.AltMultiPointSwipeParams;
import ro.altom.alttester.Commands.InputActions.AltSwipeParams;
import ro.altom.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.alttester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.alttester.position.Vector2;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene3 {
    private static AltDriver altDriver;

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

    @BeforeClass
    public static void setUp() throws Exception {
        altDriver = new AltDriver(TestsHelper.GetAltDriverHost(), TestsHelper.GetAltDriverPort(),
                true);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        if (altDriver != null) {
            altDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {

        AltLoadSceneParams params = new AltLoadSceneParams.Builder("Scene 3 Drag And Drop").build();
        altDriver.loadScene(params);
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
                .swipe(new AltSwipeParams.Builder(dragLocation.getScreenPosition(), dropLocation.getScreenPosition())
                        .withDuration(duration).withWait(wait).build());
    }

    private void waitForSwipeToFinish() {
        AltFindObjectsParams swipeImageFindObject = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "icon").build();
        AltWaitForObjectsParams swipeImageParam = new AltWaitForObjectsParams.Builder(swipeImageFindObject).build();
        altDriver.waitForObjectToNotBePresent(swipeImageParam);
    }

    private ImagesDrop getSpriteName(String sourceImageName, String imageSourceDropZoneName) {

        AltFindObjectsParams imageSourceParams = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, sourceImageName).build();
        AltFindObjectsParams imageSourceDropZoneParams = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, imageSourceDropZoneName).build();

        String imageSource = altDriver.findObject(imageSourceParams).getComponentProperty(

                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI")
                        .build(),
                String.class);
        String imageSourceDropZone = altDriver.findObject(imageSourceDropZoneParams).getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI")
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

        dropImage("Drag Image2", "Drop Box2", (float) 0.1, true);
        dropImage("Drag Image2", "Drop Box1", (float) 0.1, true);
        dropImage("Drag Image1", "Drop Box1", (float) 0.1, true);
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

        }
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
    }}
    
