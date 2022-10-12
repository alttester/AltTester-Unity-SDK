package ro.altom.alttester;

import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import ro.altom.alttester.AltDriver.By;
import ro.altom.alttester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.alttester.Commands.InputActions.AltMultiPointSwipeParams;
import ro.altom.alttester.Commands.InputActions.AltPressKeyParams;
import ro.altom.alttester.Commands.InputActions.AltPressKeysParams;
import ro.altom.alttester.Commands.InputActions.AltScrollParams;
import ro.altom.alttester.Commands.InputActions.AltSwipeParams;
import ro.altom.alttester.Commands.InputActions.AltKeyDownParams;
import ro.altom.alttester.Commands.InputActions.AltKeyUpParams;
import ro.altom.alttester.Commands.InputActions.AltMoveMouseParams;
import ro.altom.alttester.Commands.InputActions.AltTapClickCoordinatesParams;
import ro.altom.alttester.Commands.InputActions.AltTiltParams;
import ro.altom.alttester.Commands.ObjectCommand.AltTapClickElementParams;
import ro.altom.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.alttester.Commands.ObjectCommand.AltTapClickElementParams;
import ro.altom.alttester.Commands.InputActions.AltTapClickCoordinatesParams;
import ro.altom.alttester.Commands.FindObject.AltWaitForObjectsParams;
import ro.altom.alttester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.alttester.UnityStruct.AltKeyCode;
import ro.altom.alttester.position.Vector2;
import ro.altom.alttester.position.Vector3;
import java.util.Arrays;
import java.util.List;
import static org.junit.Assert.assertNotEquals;
import static junit.framework.TestCase.*;

public class TestsForNIS {
        private static AltDriver altDriver;
        String scene7 = "Assets/AltTester/Examples/Scenes/Scene 7 Drag And Drop NIS.unity";
        String scene8 = "Assets/AltTester/Examples/Scenes/Scene 8 Draggable Panel NIP.unity";
        String scene9 = "Assets/AltTester/Examples/Scenes/scene 9 NIS.unity";
        String scene10 = "Assets/AltTester/Examples/Scenes/Scene 10 Sample NIS.unity";
        String scene11 = "Assets/AltTester/Examples/Scenes/Scene 7 New Input System Actions.unity";

        class AltSprite {
                public String name;
        }

        @BeforeClass
        public static void setUp() throws Exception {
                altDriver = new AltDriver(TestsHelper.GetAltDriverHost(),
                                TestsHelper.GetAltDriverPort(),
                                true);
        }

        @AfterClass
        public static void tearDown() throws Exception {
                if (altDriver != null) {
                        altDriver.stop();
                }
                Thread.sleep(1000);
        }

        public void loadLevel(String sceneName) throws Exception {
                AltLoadSceneParams params = new AltLoadSceneParams.Builder(sceneName).build();
                altDriver.loadScene(params);
        }

        @Test
        public void TestScroll() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Player").build();
                AltObject player = altDriver.findObject(altFindObjectsParams);
                Boolean isScrolling = player
                                .getComponentProperty(
                                                new AltGetComponentPropertyParams.Builder("AltNIPDebugScript",
                                                                "wasScrolled").withAssembly(
                                                                                "Assembly-CSharp")
                                                                .build(),
                                                Boolean.class);
                assertFalse(isScrolling);
                AltScrollParams altScrollParams = new AltScrollParams.Builder().withDuration(1).withSpeed(300)
                                .withWait(true).build();
                altDriver.scroll(altScrollParams);
                isScrolling = player.getComponentProperty(
                                new AltGetComponentPropertyParams.Builder("AltNIPDebugScript",
                                                "wasScrolled").withAssembly(
                                                                "Assembly-CSharp")
                                                .build(),
                                Boolean.class);
                assertTrue(isScrolling);
        }

        @Test
        public void TestTapElement() throws Exception {
                String componentName = "AltExampleNewInputSystem";
                String propertyName = "jumpCounter";
                String assembly = "Assembly-CSharp";
                loadLevel(scene11);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Capsule").build();
                AltObject capsule = altDriver.findObject(altFindObjectsParams);
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
        public void TestScrollElement() throws Exception {
                loadLevel(scene9);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Scrollbar Vertical").build();
                AltObject scrollbar = altDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.UI.Scrollbar", "value").withAssembly("UnityEngine.UI").build();
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
        public void TestClickElement() throws Exception {
                String componentName = "AltExampleNewInputSystem";
                String propertyName = "jumpCounter";
                String assembly = "Assembly-CSharp";
                loadLevel(scene11);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Capsule").build();
                AltObject capsule = altDriver.findObject(altFindObjectsParams);
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
        public void TestTilt() throws Exception {
                loadLevel(scene11);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Cube (1)").build();
                AltObject capsule = altDriver.findObject(altFindObjectsParams);
                Vector3 initialPosition = capsule.getWorldPosition();
                altDriver.tilt(new AltTiltParams.Builder(new Vector3(1000, 10, 10)).withDuration(3f).build());
                assertNotEquals(initialPosition, altDriver.findObject(altFindObjectsParams).getWorldPosition());
                Boolean isMoved = capsule.getComponentProperty(
                        new AltGetComponentPropertyParams.Builder("AltCubeNIS",
                                        "isMoved").withAssembly("Assembly-CSharp").build(),
                        Boolean.class);
                assertTrue(isMoved);
        }

        @Test
        public void TestKeyDownAndKeyUp() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Player").build();
                AltObject player = altDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.Transform",
                                "position").build();
                Vector3 initialPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.A).build());
                Thread.sleep(1000);
                altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.A).build());
                Vector3 posUp = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(initialPos.x, posUp.x);
                altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.D).build());
                Thread.sleep(1000);
                altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.D).build());
                Vector3 posDown = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posUp.x, posDown.x);
                altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.W).build());
                Thread.sleep(1000);
                altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.W).build());
                Vector3 posLeft = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posDown.z, posLeft.z);
                altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.S).build());
                Thread.sleep(1000);
                altDriver.keyUp(new AltKeyUpParams.Builder(AltKeyCode.S).build());
                Vector3 posRight = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posLeft.z, posRight.z);
        }

        @Test
        public void TestPressKey() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Player").build();
                AltObject player = altDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.Transform",
                                "position").build();
                Vector3 initialPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.A).withDuration(1).build());
                Vector3 posUp = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(initialPos.x, posUp.x);
                altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.D).withDuration(1).build());
                Vector3 posDown = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posUp.x, posDown.x);
                altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.W).withDuration(1).build());
                Vector3 posLeft = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posDown.z, posLeft.z);
                altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.S).withDuration(1).build());
                Vector3 posRight = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(posLeft.z, posRight.z);
        }

        @Test
        public void TestPressKeys() throws Exception {
                loadLevel(scene10);
                AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                                "Player").build();
                AltObject player = altDriver.findObject(altFindObjectsParams);
                AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                                "UnityEngine.Transform",
                                "position").build();
                Vector3 initialPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                AltKeyCode[] keys = {AltKeyCode.W, AltKeyCode.Mouse0};
                altDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());
                
                Vector3 finalPos = player
                                .getComponentProperty(altGetComponentPropertyParams,
                                                Vector3.class);
                Assert.assertNotEquals(initialPos.z, finalPos.z);
                AltFindObjectsParams findObjectParams = new AltFindObjectsParams.Builder(By.NAME,
                "SimpleProjectile(Clone)").build();
                AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findObjectParams)
                                .build();
                altDriver.waitForObject(waitParams);
                
        }

        @Test
        public void TestMultipointSwipe() throws Exception { 
            loadLevel(scene7);
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
    
        @Test
        public void TestSwipe() throws Exception {
            loadLevel(scene9);
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.PATH,
                    "//Scroll View/Viewport/Content/Button (4)").build();
    
            AltObject scrollbar = altDriver.findObject(altFindObjectsParams);
            Vector2 scrollbarPosition = scrollbar.getScreenPosition();
    
            AltFindObjectsParams altFindButtonParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    "Handle").build();
            AltObject button = altDriver.findObject(altFindButtonParams);
    
            altDriver
            .swipe(new AltSwipeParams.Builder(new Vector2(button.x, button.y), new Vector2(button.x, button.y+20))
                    .withDuration(1).build());
    
            AltFindObjectsParams altFindObjectsParamsFinal = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    "Handle").build();
            AltObject scrollbarFinal = altDriver.findObject(altFindObjectsParamsFinal);
            Vector2 scrollbarPositionFinal = scrollbarFinal.getScreenPosition();
            assertNotEquals(scrollbarPosition, scrollbarPositionFinal);
        }
}