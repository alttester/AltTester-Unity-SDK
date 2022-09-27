package ro.altom.altunitytester;
import org.glassfish.grizzly.nio.transport.DefaultStreamWriter.Output;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParams;
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

    public static class ImagesDrop {
        public static String imageSource;
        public static String imageSourceDropZone;
     
        public ImagesDrop(String imageSource, String imageSourceDropZone) {
           this.imageSource = imageSource;
           this.imageSourceDropZone = imageSourceDropZone;
        }
     }

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

    private void dropImage(String dragLocationName, String dropLocationName, float duration, boolean wait)
    {
        AltFindObjectsParams dragLocationParams = new AltFindObjectsParams.Builder(
            AltUnityDriver.By.NAME, dragLocationName).build();
        AltFindObjectsParams dropLocationParams = new AltFindObjectsParams.Builder(
            AltUnityDriver.By.NAME, dropLocationName).build();

        AltUnityObject dragLocation = altUnityDriver.findObject(dragLocationParams);
        AltUnityObject dropLocation = altUnityDriver.findObject(dropLocationParams);
        
        altUnityDriver
            .swipe(new AltSwipeParams.Builder(dragLocation.getScreenPosition(), dropLocation.getScreenPosition())
                    .withDuration(duration).withWait(wait).build());
    }

    private void waitForSwipeToFinish()
    {
        AltFindObjectsParams swipeImageFindObject = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, "icon").build();
        AltWaitForObjectsParams swipeImageParam = new AltWaitForObjectsParams.Builder(swipeImageFindObject).build();
    
        altUnityDriver.waitForObjectToNotBePresent(swipeImageParam);
    }
  
    private ImagesDrop getSpriteName(String sourceImageName, String imageSourceDropZoneName)
    {
        AltFindObjectsParams imageSourceParams = new AltFindObjectsParams.Builder(
            AltUnityDriver.By.NAME, sourceImageName).build();
        AltFindObjectsParams imageSourceDropZoneParams = new AltFindObjectsParams.Builder(
            AltUnityDriver.By.NAME, sourceImageName).build();
        
        String imageSource = altUnityDriver.findObject(imageSourceParams).getComponentProperty(
            new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name").build(),
            String.class);
        String imageSourceDropZone = altUnityDriver.findObject(imageSourceDropZoneParams).getComponentProperty(
            new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite.name").build(),
            String.class);

        return new ImagesDrop(imageSource, imageSourceDropZone);
    }

    @Test
    public void testMultipleDragAndDrop() throws Exception {
        String imageSource = ImagesDrop.imageSource;
        String imageSourceDropZone = ImagesDrop.imageSourceDropZone;

        dropImage("Drag Image2", "Drop Box2", 1, false);
        dropImage("Drag Image3", "Drop Box1", 1, false);
        dropImage("Drag Image1", "Drop Box1", 1, false);

        waitForSwipeToFinish();

        getSpriteName("Drag Image1", "Drop Image");

        assertEquals(imageSource, imageSourceDropZone);

        getSpriteName("Drag Image2", "Drop");
        assertEquals(imageSource, imageSourceDropZone);
    }

    @Test
    public void testMultipleDragAndDropWait() throws Exception {
        String imageSource = ImagesDrop.imageSource;
        String imageSourceDropZone = ImagesDrop.imageSourceDropZone;

        dropImage("Drag Image2", "Drop Box2", (float)0.1, true);
        dropImage("Drag Image3", "Drop Box1", (float)0.1, true);
        dropImage("Drag Image1", "Drop Box1", (float)0.1, true);

        waitForSwipeToFinish();

        getSpriteName("Drag Image1", "Drop Image");

        assertEquals(imageSource, imageSourceDropZone);

        getSpriteName("Drag Image2", "Drop");
        assertEquals(imageSource, imageSourceDropZone);
    }

//     @Test
//     public void testTestPointerEnterAndExit() throws Exception {
//         AltFindObjectsParams findObjectParams;

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
//         AltUnityObject altElement = altUnityDriver.findObject(findObjectParams);
//         AltUnityColor color1 = altElement.getComponentProperty(
//                 new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
//                         "Assembly-CSharp").build(),
//                 AltUnityColor.class);

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
//         altUnityDriver.findObject(findObjectParams).pointerEnter();
//         AltUnityColor color2 = altElement.getComponentProperty(
//                 new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
//                         "Assembly-CSharp").build(),
//                 AltUnityColor.class);
//         assertTrue(color1.r != color2.r || color1.g != color2.g || color1.b != color2.b || color1.a != color2.a);

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
//         altUnityDriver.findObject(findObjectParams).pointerExit();
//         AltUnityColor color3 = altElement.getComponentProperty(
//                 new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
//                         "Assembly-CSharp").build(),
//                 AltUnityColor.class);
//         assertTrue(color3.r != color2.r || color3.g != color2.g || color3.b != color2.b || color3.a != color2.a);
//         assertTrue(color1.r == color3.r && color1.g == color3.g && color1.b == color3.b && color1.a == color3.a);
//     }

//     @Test
//     public void testMultipleDragAndDropWaitWithMultipointSwipe() throws InterruptedException {
//         AltFindObjectsParams findObjectParams;

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
//         AltUnityObject altElement1 = altUnityDriver.findObject(findObjectParams);

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box1").build();
//         AltUnityObject altElement2 = altUnityDriver.findObject(findObjectParams);

//         List<Vector2> positions = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
//                 new Vector2(altElement2.x, altElement2.y));

//         altUnityDriver.multipointSwipe(
//                 new AltMultiPointSwipeParams.Builder(positions).withDuration(2).withWait(false).build());

//         Thread.sleep(2000);
//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
//         altElement1 = altUnityDriver.findObject(findObjectParams);

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box1").build();
//         altElement2 = altUnityDriver.findObject(findObjectParams);

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Box2").build();
//         AltUnityObject altElement3 = altUnityDriver.findObject(findObjectParams);

//         List<Vector2> positions2 = Arrays.asList(new Vector2(altElement1.x, altElement1.y),
//                 new Vector2(altElement2.x, altElement2.y), new Vector2(altElement3.x, altElement3.y));
//         altUnityDriver.multipointSwipe(new AltMultiPointSwipeParams.Builder(positions2).withDuration(3).build());

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image1").build();
//         String imageSourceName = altUnityDriver.findObject(findObjectParams).getComponentProperty(
//                 new AltGetComponentPropertyParams.Builder(
//                         "UnityEngine.UI.Image",
//                         "sprite.name").build(),
//                 String.class);

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop Image").build();
//         String imageSourceDropZoneName = altUnityDriver.findObject(findObjectParams)
//                 .getComponentProperty(new AltGetComponentPropertyParams.Builder(
//                         "UnityEngine.UI.Image",
//                         "sprite.name").build(),
//                         String.class);
//         assertNotEquals(imageSourceName, imageSourceDropZoneName);

//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drag Image2").build();
//         imageSourceName = altUnityDriver.findObject(findObjectParams)
//                 .getComponentProperty(new AltGetComponentPropertyParams.Builder(
//                         "UnityEngine.UI.Image",
//                         "sprite.name").build(),
//                         String.class);
//         findObjectParams = new AltFindObjectsParams.Builder(By.NAME, "Drop").build();
//         imageSourceDropZoneName = altUnityDriver.findObject(findObjectParams)
//                 .getComponentProperty(new AltGetComponentPropertyParams.Builder(
//                         "UnityEngine.UI.Image",
//                         "sprite.name").build(),
//                         String.class);
//         assertNotEquals(imageSourceName, imageSourceDropZoneName);
//     }
}
