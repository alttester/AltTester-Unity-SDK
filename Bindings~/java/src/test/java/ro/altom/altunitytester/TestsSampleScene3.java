package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.InputActions.AltMultiPointSwipeParams;
import ro.altom.altunitytester.Commands.InputActions.AltSwipeParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParams;
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
        altUnityDriver = new AltUnityDriver(TestsHelper.GetAltUnityDriverHost(), TestsHelper.GetAltUnityDriverPort(),
                true);
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

        AltLoadSceneParams params = new AltLoadSceneParams.Builder("Scene 3 Drag And Drop").build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void testMultipleDragAndDrop() throws Exception {
        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Image1").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Box1").build();
        AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Image2").build();
        AltFindObjectsParams altFindObjectsParameters4 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Image3").build();
        AltFindObjectsParams altFindObjectsParameters5 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Box2").build();
        AltFindObjectsParams altFindObjectsParameters6 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();
        AltFindObjectsParams altFindObjectsParameters7 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();
        AltUnityObject altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);

        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).withWait(false).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters3);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters5);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).withWait(false).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters4);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(3).withWait(false).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(5).withWait(false).build());

        Thread.sleep(6000);

        AltUnitySprite imageSource = altUnityDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        AltUnitySprite imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);

        imageSource = altUnityDriver.findObject(altFindObjectsParameters3).getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                AltUnitySprite.class);
        imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);
    }

    @Test
    public void testMultipleDragAndDropWait() throws Exception {

        AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Image1").build();
        AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Box1").build();
        AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Image2").build();
        AltFindObjectsParams altFindObjectsParameters4 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drag Image3").build();
        AltFindObjectsParams altFindObjectsParameters5 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Box2").build();
        AltFindObjectsParams altFindObjectsParameters6 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();
        AltFindObjectsParams altFindObjectsParameters7 = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "Drop Image").build();

        AltUnityObject altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
        AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters3);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters5);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(2).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters4);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(3).build());

        altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
        altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
        altUnityDriver
                .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                        .withDuration(1).build());
        AltUnitySprite imageSource = altUnityDriver.findObject(altFindObjectsParameters1)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        AltUnitySprite imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters6)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);

        imageSource = altUnityDriver.findObject(altFindObjectsParameters3).getComponentProperty(
                new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                AltUnitySprite.class);
        imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters7)
                .getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite").build(),
                        AltUnitySprite.class);
        assertNotSame(imageSource.name, imageSourceDropZone.name);
    }

    @Test
    public void testTestPointerEnterAndExit() throws Exception {
        AltFindObjectsParams findObjectParams;

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        AltUnityObject altElement = altUnityDriver.findObject(findObjectParams);
        AltUnityColor color1 = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
                        "Assembly-CSharp").build(),
                AltUnityColor.class);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        altUnityDriver.findObject(findObjectParams).pointerEnter();
        AltUnityColor color2 = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
                        "Assembly-CSharp").build(),
                AltUnityColor.class);
        assertTrue(color1.r != color2.r || color1.g != color2.g || color1.b != color2.b || color1.a != color2.a);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        altUnityDriver.findObject(findObjectParams).pointerExit();
        AltUnityColor color3 = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
                        "Assembly-CSharp").build(),
                AltUnityColor.class);
        assertTrue(color3.r != color2.r || color3.g != color2.g || color3.b != color2.b || color3.a != color2.a);
        assertTrue(color1.r == color3.r && color1.g == color3.g && color1.b == color3.b && color1.a == color3.a);
    }

    @Test
    public void testMultipleDragAndDropWaitWithMultipointSwipe() throws InterruptedException {
        AltFindObjectsParams findObjectParams;

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
        AltUnityObject altElement1 = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box1").build();
        AltUnityObject altElement2 = altUnityDriver.findObject(findObjectParams);

        List<Vector2> positions = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
                new Vector2(altElement2.x, altElement2.y));

        altUnityDriver.multipointSwipe(
                new AltMultiPointSwipeParams.Builder(positions).withDuration(2).withWait(false).build());

        Thread.sleep(2000);
        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
        altElement1 = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box1").build();
        altElement2 = altUnityDriver.findObject(findObjectParams);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box2").build();
        AltUnityObject altElement3 = altUnityDriver.findObject(findObjectParams);

        List<Vector2> positions2 = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
                new Vector2(altElement2.x, altElement2.y), new Vector2(altElement3.x, altElement3.y));
        altUnityDriver.multipointSwipe(new AltMultiPointSwipeParams.Builder(positions2).withDuration(3).build());

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
        String imageSourceName = altUnityDriver.findObject(findObjectParams).getComponentProperty(
                new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                String.class);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
        String imageSourceDropZoneName = altUnityDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        assertNotEquals(imageSourceName, imageSourceDropZoneName);

        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image2").build();
        imageSourceName = altUnityDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop").build();
        imageSourceDropZoneName = altUnityDriver.findObject(findObjectParams)
                .getComponentProperty(new AltGetComponentPropertyParams.Builder(
                        "UnityEngine.UI.Image",
                        "sprite.name").build(),
                        String.class);
        assertNotEquals(imageSourceName, imageSourceDropZoneName);
    }
}
