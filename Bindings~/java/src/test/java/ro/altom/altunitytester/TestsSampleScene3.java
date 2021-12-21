package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.InputActions.AltMultiPointSwipeParameters;
import ro.altom.altunitytester.Commands.InputActions.AltSwipeParameters;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParameters;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParameters;
import ro.altom.altunitytester.position.Vector2;

import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsSampleScene3 {
    private static AltUnityDriver altUnityDriver;

    class AltUnitySprite {
        public String name;
    }

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

        AltUnitySprite imageSource = altUnityDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty(
                        new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        AltUnitySprite imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty(
                        new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);

        imageSource = altUnityDriver.findObject(altFindObjectsParameters3).getComponentProperty(
                new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                AltUnitySprite.class);
        imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty(
                        new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);
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
        AltUnitySprite imageSource = altUnityDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty(
                        new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        AltUnitySprite imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty(
                        new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);

        imageSource = altUnityDriver.findObject(altFindObjectsParameters3).getComponentProperty(
                new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                AltUnitySprite.class);
        imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty(
                        new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);
    }

    @Test
    public void testTestPointerEnterAndExit() throws Exception {
        AltFindObjectsParameters findObjectParams;

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        AltUnityObject altElement = altUnityDriver.findObject(findObjectParams);
        AltUnityColor color1 = altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder("AltUnityExampleScriptDropMe", "highlightColor").build(),
                AltUnityColor.class);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        altUnityDriver.findObject(findObjectParams).pointerEnter();
        AltUnityColor color2 = altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder("AltUnityExampleScriptDropMe", "highlightColor").build(),
                AltUnityColor.class);
        assertTrue(color1.r != color2.r || color1.g != color2.g || color1.b != color2.b || color1.a != color2.a);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        altUnityDriver.findObject(findObjectParams).pointerExit();
        AltUnityColor color3 = altElement.getComponentProperty(
                new AltGetComponentPropertyParameters.Builder("AltUnityExampleScriptDropMe", "highlightColor").build(),
                AltUnityColor.class);
        assertTrue(color3.r != color2.r || color3.g != color2.g || color3.b != color2.b || color3.a != color2.a);
        assertTrue(color1.r == color3.r && color1.g == color3.g && color1.b == color3.b && color1.a == color3.a);
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
        String imageSourceName = altUnityDriver.findObject(findObjectParams).getComponentProperty(
                new AltGetComponentPropertyParameters.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                String.class);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop Image").build();
        String imageSourceDropZoneName = altUnityDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParameters.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        assertNotEquals(imageSourceName, imageSourceDropZoneName);

        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drag Image2").build();
        imageSourceName = altUnityDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParameters.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        findObjectParams = new AltFindObjectsParameters.Builder(By.NAME, "Drop").build();
        imageSourceDropZoneName = altUnityDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParameters.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        assertNotEquals(imageSourceName, imageSourceDropZoneName);
    }
}
