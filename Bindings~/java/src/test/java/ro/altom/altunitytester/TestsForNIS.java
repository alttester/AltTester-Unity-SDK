package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.InputActions.AltMultiPointSwipeParams;
import ro.altom.altunitytester.Commands.InputActions.AltPressKeyParams;
import ro.altom.altunitytester.Commands.InputActions.AltPressKeysParams;
import ro.altom.altunitytester.Commands.InputActions.AltScrollParams;
import ro.altom.altunitytester.Commands.InputActions.AltSwipeParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeyDownParams;
import ro.altom.altunitytester.Commands.InputActions.AltKeyUpParams;
import ro.altom.altunitytester.Commands.InputActions.AltMoveMouseParams;
import ro.altom.altunitytester.Commands.InputActions.AltTapClickCoordinatesParams;
import ro.altom.altunitytester.Commands.InputActions.AltTiltParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltTapClickElementParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltTapClickElementParams;
import ro.altom.altunitytester.Commands.InputActions.AltTapClickCoordinatesParams;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;
import ro.altom.altunitytester.position.Vector2;
import ro.altom.altunitytester.position.Vector3;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import static org.junit.Assert.assertNotEquals;
import static junit.framework.TestCase.*;

public class TestsForNIS {
        private static AltUnityDriver altUnityDriver;
        String scene7 = "Assets/AltUnityTester/Examples/Scenes/Scene 7 Drag And Drop NIS.unity";
        String scene8 = "Assets/AltUnityTester/Examples/Scenes/Scene 8 Draggable Panel NIP.unity";
        String scene9 = "Assets/AltUnityTester/Examples/Scenes/scene 9 NIS.unity";
        String scene10 = "Assets/AltUnityTester/Examples/Scenes/Scene 10 Sample NIS.unity";
        String scene11 = "Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity";

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
                altUnityDriver = new AltUnityDriver(TestsHelper.GetAltUnityDriverHost(),
                                TestsHelper.GetAltUnityDriverPort(),
                                true);
        }

        @AfterClass
        public static void tearDown() throws Exception {
                if (altUnityDriver != null) {
                        altUnityDriver.stop();
                }
                Thread.sleep(1000);
        }

        public void loadLevel(String sceneName) throws Exception {
                AltLoadSceneParams params = new AltLoadSceneParams.Builder(sceneName).build();
                altUnityDriver.loadScene(params);
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
        public void TestScroll() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Player").build();
                AltUnityObject player = altUnityDriver.findObject(altFindObjectsParams);
                Boolean isScrolling = player
                                .getComponentProperty(
                                                new AltGetComponentPropertyParams.Builder("AltUnityNIPDebugScript",
                                                                "wasScrolled").withAssembly(
                                                                                "Assembly-CSharp")
                                                                .build(),
                                                Boolean.class);
                assertFalse(isScrolling);
                AltScrollParams altScrollParams = new AltScrollParams.Builder().withDuration(1).withSpeed(300)
                                .withWait(true).build();
                altUnityDriver.scroll(altScrollParams);
                isScrolling = player.getComponentProperty(
                                new AltGetComponentPropertyParams.Builder("AltUnityNIPDebugScript",
                                                "wasScrolled").withAssembly(
                                                                "Assembly-CSharp")
                                                .build(),
                                Boolean.class);
                assertTrue(isScrolling);
        }

        @Test
        public void TestTapElement() throws Exception {
                String componentName = "AltUnityExampleNewInputSystem";
                String propertyName = "jumpCounter";
                String assembly = "Assembly-CSharp";
                loadLevel(scene11);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Capsule").build();
                AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParams);
                AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
                capsule.tap(tapParams);
                int propertyValue = capsule.getComponentProperty(
                                new AltGetComponentPropertyParams.Builder(componentName,
                                                propertyName).withAssembly(assembly).build(),
                                int.class);
                assertEquals(1, propertyValue);
        }

        @Test
        public void TestTapCoordinates() throws Exception {
                loadLevel(scene11);
                AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                                .build();
                AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParams);
                AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
                                capsule.getScreenPosition()).build();
                altUnityDriver.tap(tapParams);
                AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                                "//ActionText[@text=Capsule was tapped!]").build();
                AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                                .build();
                altUnityDriver.waitForObject(waitParams);
        }

        @Test
        public void TestScrollElement() throws Exception {
                loadLevel(scene9);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Scrollbar Vertical").build();
                AltUnityObject scrollbar = altUnityDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.UI.Scrollbar", "value").withAssembly("UnityEngine.UI").build();
                Float scrollbarPosition = scrollbar.getComponentProperty(altGetComponentPropertyParams, Float.class);
                AltFindObjectsParams altFindObjectsParamsScrollView = new AltFindObjectsParams.Builder(
                                AltUnityDriver.By.NAME,
                                "Scroll View").build();
                AltUnityObject scrollView = altUnityDriver.findObject(altFindObjectsParamsScrollView);
                AltMoveMouseParams altMoveMouseParams = new AltMoveMouseParams.Builder(
                                scrollView.getScreenPosition())
                                .withDuration(1f).build();
                altUnityDriver.moveMouse(altMoveMouseParams);
                AltScrollParams altScrollParams = new AltScrollParams.Builder().withDuration(1).withSpeed(-300)
                                .withWait(true).build();
                altUnityDriver.scroll(altScrollParams);
                AltUnityObject scrollbarFinal = altUnityDriver.findObject(altFindObjectsParams);
                Float scrollbarPositionFinal = scrollbarFinal.getComponentProperty(altGetComponentPropertyParams,
                                Float.class);
                assertNotEquals(scrollbarPosition, scrollbarPositionFinal);
        }

        @Test
        public void TestClickElement() throws Exception {
                String componentName = "AltUnityExampleNewInputSystem";
                String propertyName = "jumpCounter";
                String assembly = "Assembly-CSharp";
                loadLevel(scene11);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Capsule").build();
                AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParams);
                AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
                capsule.click(tapParams);
                int propertyValue = capsule.getComponentProperty(
                                new AltGetComponentPropertyParams.Builder(componentName,
                                                propertyName).withAssembly(assembly).build(),
                                int.class);
                assertEquals(1, propertyValue);
        }

        @Test
        public void TestClickCoordinates() throws Exception {
                loadLevel(scene11);
                AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                                .build();
                AltUnityObject capsule = altUnityDriver.findObject(findCapsuleParams);
                AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
                                capsule.getScreenPosition()).build();
                altUnityDriver.click(tapParams);
                AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                                "//ActionText[@text=Capsule was clicked!]").build();
                AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                                .build();
                altUnityDriver.waitForObject(waitParams);
        }

        @Test
        public void TestTilt() throws Exception {
                loadLevel(scene11);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Cube (1)").build();
                AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParams);
                Vector3 initialPosition = capsule.getWorldPosition();
                altUnityDriver.tilt(new AltTiltParams.Builder(new Vector3(1000, 10, 10)).withDuration(3f).build());
                assertNotEquals(initialPosition, altUnityDriver.findObject(altFindObjectsParams).getWorldPosition());
                Boolean isMoved = capsule.getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("AltUnityCubeNIS",
                                        "isMoved").withAssembly("Assembly-CSharp").build(),
                        Boolean.class);
                assertTrue(isMoved);
        }

        @Test
        public void TestKeyDownAndKeyUp() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Player").build();
                AltUnityObject player = altUnityDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.Transform",
                                "position").build();
                Vector3 initialPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                altUnityDriver.keyDown(new AltKeyDownParams.Builder(AltUnityKeyCode.A).build());
                Thread.sleep(1000);
                altUnityDriver.keyUp(new AltKeyUpParams.Builder(AltUnityKeyCode.A).build());
                Vector3 posUp = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(initialPos.x, posUp.x);
                altUnityDriver.keyDown(new AltKeyDownParams.Builder(AltUnityKeyCode.D).build());
                Thread.sleep(1000);
                altUnityDriver.keyUp(new AltKeyUpParams.Builder(AltUnityKeyCode.D).build());
                Vector3 posDown = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posUp.x, posDown.x);
                altUnityDriver.keyDown(new AltKeyDownParams.Builder(AltUnityKeyCode.W).build());
                Thread.sleep(1000);
                altUnityDriver.keyUp(new AltKeyUpParams.Builder(AltUnityKeyCode.W).build());
                Vector3 posLeft = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posDown.z, posLeft.z);
                altUnityDriver.keyDown(new AltKeyDownParams.Builder(AltUnityKeyCode.S).build());
                Thread.sleep(1000);
                altUnityDriver.keyUp(new AltKeyUpParams.Builder(AltUnityKeyCode.S).build());
                Vector3 posRight = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posLeft.z, posRight.z);
        }

        @Test
        public void TestPressKey() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Player").build();
                AltUnityObject player = altUnityDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.Transform",
                                "position").build();
                Vector3 initialPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.A).withDuration(1).build());
                Vector3 posUp = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(initialPos.x, posUp.x);
                altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.D).withDuration(1).build());
                Vector3 posDown = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posUp.x, posDown.x);
                altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.W).withDuration(1).build());
                Vector3 posLeft = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posDown.z, posLeft.z);
                altUnityDriver.pressKey(new AltPressKeyParams.Builder(AltUnityKeyCode.S).withDuration(1).build());
                Vector3 posRight = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posLeft.z, posRight.z);
        }

        @Test
        public void TestPressKeys() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                                "Player").build();
                AltUnityObject player = altUnityDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.Transform",
                                "position").build();
                Vector3 initialPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                AltUnityKeyCode[] keys = {AltUnityKeyCode.W, AltUnityKeyCode.Mouse0};
                altUnityDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());
                
                Vector3 finalPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(initialPos.z, finalPos.z);
                AltFindObjectsParams findObjectParams = new AltFindObjectsParams.Builder(By.NAME,
                "SimpleProjectile(Clone)").build();
                AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findObjectParams)
                                .build();
                altUnityDriver.waitForObject(waitParams);
                
        }

        @Test
        public void TestMultipointSwipe() throws Exception { 
            loadLevel(scene7);
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
        public void TestSwipe() throws Exception {
            loadLevel(scene9);
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.PATH,
                    "//Scroll View/Viewport/Content/Button (4)").build();
    
            AltUnityObject scrollbar = altUnityDriver.findObject(altFindObjectsParams);
            Vector2 scrollbarPosition = scrollbar.getScreenPosition();
    
            AltFindObjectsParams altFindButtonParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                    "Handle").build();
            AltUnityObject button = altUnityDriver.findObject(altFindButtonParams);
    
            altUnityDriver
            .swipe(new AltSwipeParams.Builder(new Vector2(button.x, button.y), new Vector2(button.x, button.y+20))
                    .withDuration(1).build());
    
            AltFindObjectsParams altFindObjectsParamsFinal = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                    "Handle").build();
            AltUnityObject scrollbarFinal = altUnityDriver.findObject(altFindObjectsParamsFinal);
            Vector2 scrollbarPositionFinal = scrollbarFinal.getScreenPosition();
            assertNotEquals(scrollbarPosition, scrollbarPositionFinal);
        }
}