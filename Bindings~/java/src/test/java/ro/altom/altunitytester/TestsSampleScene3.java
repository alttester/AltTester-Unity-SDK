package ro.altom.altunitytester;
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

import java.util.ArrayList;
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
    
    private AltUnityObject FindObject(By by,String name){
        AltFindObjectsParams altElementParams = new AltFindObjectsParams.Builder(
           by, name).build();

        return altUnityDriver.findObject(altElementParams);
    }

    private void dropImage(String dragLocationName, String dropLocationName, float duration, boolean wait)
    {
        AltUnityObject dragLocation = FindObject(AltUnityDriver.By.NAME, dragLocationName);
        AltUnityObject dropLocation = FindObject(AltUnityDriver.By.NAME, dropLocationName);
        
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

    @Test
    public void testTestPointerEnterAndExit() throws Exception {
        AltUnityObject altElement = FindObject(By.NAME, "Drop Image");
        AltUnityColor color1 = altElement.getComponentProperty(
                new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
                        "Assembly-CSharp").build(),
                AltUnityColor.class);
        FindObject(By.NAME, "Drop Image").pointerEnter();
        AltUnityColor color2 = altElement.getComponentProperty(
            new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
                    "Assembly-CSharp").build(),
            AltUnityColor.class);

        assertNotEquals(color1, color2);

        FindObject(By.NAME, "Drop Image").pointerEnter();
        AltUnityColor color3 = altElement.getComponentProperty(
            new AltGetComponentPropertyParams.Builder("AltUnityExampleScriptDropMe", "highlightColor").withAssembly(
                    "Assembly-CSharp").build(),
            AltUnityColor.class);  

        assertNotEquals(color3, color2);
        assertNotEquals(color1, color3);
    }

    private void dropImageWithMultipointSwipe(List<String> objectNames, float duration, boolean wait) {
        List<Vector2> listPositions = new ArrayList<Vector2>();
        for(int i=0;i<objectNames.size();i++){
            AltFindObjectsParams elementParams = new AltFindObjectsParams.Builder(
                AltUnityDriver.By.NAME, objectNames.get(i)).build();
            AltUnityObject element = altUnityDriver.findObject(elementParams);
            listPositions.add(element.getScreenPosition());
        }

        altUnityDriver.multipointSwipe(
        new AltMultiPointSwipeParams.Builder(listPositions).withDuration(duration).withWait(wait).build());
    }

    @Test
    public void testMultipleDragAndDropWithMultipointSwipe() throws Exception {
        String imageSource = ImagesDrop.imageSource;
        String imageSourceDropZone = ImagesDrop.imageSourceDropZone;
        List<String> objects1 = new ArrayList<String>();
        List<String> objects2 = new ArrayList<String>();
        objects1.add("Drag Image1");
        objects1.add("Drop Box1");

        objects2.add("Drag Image2");
        objects2.add("Drop Box1");
        objects2.add("Drop Box2");

        dropImageWithMultipointSwipe(objects1, 1, false);
        dropImageWithMultipointSwipe(objects2, 1, false);

        getSpriteName("Drag Image1", "Drop Image");
        assertEquals(imageSource, imageSourceDropZone);

        getSpriteName("Drag Image2", "Drop");
        assertEquals(imageSource, imageSourceDropZone);
    }

    @Test
    public void testMultipleDragAndDropWaitWithMultipointSwipe() throws Exception {
        String imageSource = ImagesDrop.imageSource;
        String imageSourceDropZone = ImagesDrop.imageSourceDropZone;
        List<String> objects1 = new ArrayList<String>();
        List<String> objects2 = new ArrayList<String>();
        objects1.add("Drag Image1");
        objects1.add("Drop Box1");

        objects2.add("Drag Image2");
        objects2.add("Drop Box1");
        objects2.add("Drop Box2");

        dropImageWithMultipointSwipe(objects1, 1, false);
        dropImageWithMultipointSwipe(objects2, 1, false);

        getSpriteName("Drag Image1", "Drop Image");
        assertEquals(imageSource, imageSourceDropZone);

        getSpriteName("Drag Image2", "Drop");
        assertEquals(imageSource, imageSourceDropZone);
    }
}
