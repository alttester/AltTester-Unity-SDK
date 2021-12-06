package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.InputActions.AltMultiPointSwipeParameters;
import ro.altom.altunitytester.Commands.InputActions.AltSwipeParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;
import ro.altom.altunitytester.position.Vector2;

import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene3 {
    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13010, true);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        if (altUnityDriver != null) {
            altUnityDriver.stop();
        }
        Thread.sleep(1000);
    }

    @Before
    public void loadLevel() throws Exception {

        AltLoadSceneParameters params = new AltLoadSceneParameters.Builder("Scene 3 Drag And Drop").build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void testMultipleDragAndDrop() throws Exception {
        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Image1").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Box1").build();
        AltFindObjectsParameters altFindObjectsParameters3 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Image2").build();
        AltFindObjectsParameters altFindObjectsParameters4 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Image3").build();
        AltFindObjectsParameters altFindObjectsParameters5 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Box2").build();
        AltFindObjectsParameters altFindObjectsParameters6 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();
        AltFindObjectsParameters altFindObjectsParameters7 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();
        AltUnityObject altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);

        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).withWait(false).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters3);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters5);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).withWait(false).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters4);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(3).withWait(false).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(5).withWait(false).build());

        Thread.sleep(6000);

        String imageSource = altUnityDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty("UnityEngine.UI.Image", "sprite");
        String imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);

        imageSource = altUnityDriver.findObject(altFindObjectsParameters3).getComponentProperty("UnityEngine.UI.Image",
                "sprite");
        imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);
    }

    @Test
    public void testMultipleDragAndDropWait() throws Exception {

        AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Image1").build();
        AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Box1").build();
        AltFindObjectsParameters altFindObjectsParameters3 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Image2").build();
        AltFindObjectsParameters altFindObjectsParameters4 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drag Image3").build();
        AltFindObjectsParameters altFindObjectsParameters5 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Box2").build();
        AltFindObjectsParameters altFindObjectsParameters6 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();
        AltFindObjectsParameters altFindObjectsParameters7 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();

        AltUnityObject altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters3);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters5);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters4);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(3).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParameters.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(1).build());
        String imageSource = altUnityDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty("UnityEngine.UI.Image", "sprite");
        String imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);

        imageSource = altUnityDriver.findObject(altFindObjectsParameters3).getComponentProperty("UnityEngine.UI.Image",
                "sprite");
        imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotSame(imageSource, imageSourceDropZone);
    }

    @Test
    public void testTestPointerEnterAndExit() throws Exception {
        AltFindObjectsParameters findObjectParams;

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        AltUnityObject altElement = altUnityDriver.findObject(findObjectParams);
        String color1 = altElement.getComponentProperty("AltUnityExampleScriptDropMe", "highlightColor");

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        altUnityDriver.findObject(findObjectParams).pointerEnter();
        String color2 = altElement.getComponentProperty("AltUnityExampleScriptDropMe", "highlightColor");
        assertNotSame(color1, color2);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        altUnityDriver.findObject(findObjectParams).pointerExit();
        String color3 = altElement.getComponentProperty("AltUnityExampleScriptDropMe", "highlightColor");
        assertNotSame(color3, color2);
        assertEquals(color1, color3);
    }

    @Test
    public void testMultipleDragAndDropWaitWithMultipointSwipe() throws InterruptedException {
        AltFindObjectsParameters findObjectParams;

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drag Image1").build();
        AltUnityObject altElement1 = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Box1").build();
        AltUnityObject altElement2 = altUnityDriver.findObject(findObjectParams);

        List<Vector2> positions = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
                new Vector2(altElement2.x, altElement2.y));

        altUnityDriver.multipointSwipe(
                new AltMultiPointSwipeParameters.Builder(positions).withDuration(2).withWait(false).build());

        Thread.sleep(2000);
        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drag Image1").build();
        altElement1 = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Box1").build();
        altElement2 = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Box2").build();
        AltUnityObject altElement3 = altUnityDriver.findObject(findObjectParams);

        List<Vector2> positions2 = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
                new Vector2(altElement2.x, altElement2.y), new Vector2(altElement3.x, altElement3.y));
        altUnityDriver.multipointSwipe(new AltMultiPointSwipeParameters.Builder(positions2).withDuration(3).build());

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drag Image1").build();
        String imageSource = altUnityDriver.findObject(findObjectParams).getComponentProperty("UnityEngine.UI.Image",
                "sprite");

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        String imageSourceDropZone = altUnityDriver.findObject(findObjectParams)
                .getComponentProperty("UnityEngine.UI.Image", "sprite");
        assertNotEquals(imageSource, imageSourceDropZone);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drag Image2").build();
        imageSource = altUnityDriver.findObject(findObjectParams).getComponentProperty("UnityEngine.UI.Image",
                "sprite");
        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop").build();
        imageSourceDropZone = altUnityDriver.findObject(findObjectParams).getComponentProperty("UnityEngine.UI.Image",
                "sprite");
        assertNotEquals(imageSource, imageSourceDropZone);
    }
}
