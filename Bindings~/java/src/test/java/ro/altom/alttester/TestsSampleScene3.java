package ro.altom.alttester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.alttester.AltDriver.By;
import ro.altom.alttester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.alttester.Commands.InputActions.AltMultiPointSwipeParams;
import ro.altom.alttester.Commands.InputActions.AltSwipeParams;
import ro.altom.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.alttester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.alttester.position.Vector2;

import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene3 {
    private static AltDriver altDriver;

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

    @Test
    public void testMultipleDragAndDrop() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Image1").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Box1").build();
        AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Image2").build();
        AltFindObjectsParams altFindObjectsParameters4 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Image3").build();
        AltFindObjectsParams altFindObjectsParameters5 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Box2").build();
        AltFindObjectsParams altFindObjectsParameters6 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Image").build();
        AltFindObjectsParams altFindObjectsParameters7 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Image").build();
        AltObject altElement1 = altDriver.findObject(altFindObjectsParameters1);

        AltObject altElement2 = altDriver.findObject(altFindObjectsParameters2);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).withWait(false).build());

        altElement1 = altDriver.findObject(altFindObjectsParameters3);
        altElement2 = altDriver.findObject(altFindObjectsParameters5);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).withWait(false).build());

        altElement1 = altDriver.findObject(altFindObjectsParameters4);
        altElement2 = altDriver.findObject(altFindObjectsParameters2);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(3).withWait(false).build());

        altElement1 = altDriver.findObject(altFindObjectsParameters1);
        altElement2 = altDriver.findObject(altFindObjectsParameters2);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(5).withWait(false).build());

        Thread.sleep(6000);

        AltSprite imageSource = altDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltSprite.class);
        AltSprite imageSourceDropZone = altDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltSprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);

        imageSource = altDriver.findObject(altFindObjectsParameters3).getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                AltSprite.class);
        imageSourceDropZone = altDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltSprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);
    }

    @Test
    public void testMultipleDragAndDropWait() throws Exception {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Image1").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Box1").build();
        AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Image2").build();
        AltFindObjectsParams altFindObjectsParameters4 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drag Image3").build();
        AltFindObjectsParams altFindObjectsParameters5 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Box2").build();
        AltFindObjectsParams altFindObjectsParameters6 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Image").build();
        AltFindObjectsParams altFindObjectsParameters7 = new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Drop Image").build();

        AltObject altElement1 = altDriver.findObject(altFindObjectsParameters1);
        AltObject altElement2 = altDriver.findObject(altFindObjectsParameters2);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).build());

        altElement1 = altDriver.findObject(altFindObjectsParameters3);
        altElement2 = altDriver.findObject(altFindObjectsParameters5);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).build());

        altElement1 = altDriver.findObject(altFindObjectsParameters4);
        altElement2 = altDriver.findObject(altFindObjectsParameters2);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(3).build());

        altElement1 = altDriver.findObject(altFindObjectsParameters1);
        altElement2 = altDriver.findObject(altFindObjectsParameters2);
        altDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(1).build());
        AltSprite imageSource = altDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltSprite.class);
        AltSprite imageSourceDropZone = altDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltSprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);

        imageSource = altDriver.findObject(altFindObjectsParameters3).getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                AltSprite.class);
        imageSourceDropZone = altDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltSprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);
    }

    @Test
    public void testTestPointerEnterAndExit() throws Exception {
        AltFindObjectsParams findObjectParams;

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        AltObject altElement = altDriver.findObject(findObjectParams);
        AltColor color1 = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp").build(),
                AltColor.class);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        altDriver.findObject(findObjectParams).pointerEnter();
        AltColor color2 = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp").build(),
                AltColor.class);
        assertTrue(color1.r != color2.r || color1.g != color2.g || color1.b != color2.b || color1.a != color2.a);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        altDriver.findObject(findObjectParams).pointerExit();
        AltColor color3 = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp").build(),
                AltColor.class);
        assertTrue(color3.r != color2.r || color3.g != color2.g || color3.b != color2.b || color3.a != color2.a);
        assertTrue(color1.r == color3.r && color1.g == color3.g && color1.b == color3.b && color1.a == color3.a);
    }

    @Test
    public void testMultipleDragAndDropWaitWithMultipointSwipe() throws InterruptedException {
        AltFindObjectsParams findObjectParams;

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
        AltObject altElement1 = altDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box1").build();
        AltObject altElement2 = altDriver.findObject(findObjectParams);

        List<Vector2> positions = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
                new Vector2(altElement2.x, altElement2.y));

        altDriver.multipointSwipe(
                new AltMultiPointSwipeParams.Builder(positions).withDuration(2).withWait(false).build());

        Thread.sleep(2000);
        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
        altElement1 = altDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box1").build();
        altElement2 = altDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box2").build();
        AltObject altElement3 = altDriver.findObject(findObjectParams);

        List<Vector2> positions2 = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
                new Vector2(altElement2.x, altElement2.y), new Vector2(altElement3.x, altElement3.y));
        altDriver.multipointSwipe(new AltMultiPointSwipeParams.Builder(positions2).withDuration(3).build());

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
        String imageSourceName = altDriver.findObject(findObjectParams).getComponentProperty(
                new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                String.class);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        String imageSourceDropZoneName = altDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        assertNotEquals(imageSourceName, imageSourceDropZoneName);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image2").build();
        imageSourceName = altDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop").build();
        imageSourceDropZoneName = altDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        assertNotEquals(imageSourceName, imageSourceDropZoneName);
    }
}
