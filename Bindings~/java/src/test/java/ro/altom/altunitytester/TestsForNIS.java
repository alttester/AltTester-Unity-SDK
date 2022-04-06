package ro.altom.altunitytester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.InputActions.AltMultiPointSwipeParams;
import ro.altom.altunitytester.Commands.InputActions.AltScrollParams;
import ro.altom.altunitytester.Commands.InputActions.AltSwipeParams;
import ro.altom.altunitytester.Commands.InputActions.AltMoveMouseParams;
import ro.altom.altunitytester.Commands.InputActions.AltTapClickCoordinatesParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltTapClickElementParams;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import ro.altom.altunitytester.Commands.UnityCommand.AltLoadSceneParams;
import ro.altom.altunitytester.position.Vector2;

import java.util.Arrays;
import java.util.List;

import static org.junit.Assert.*;

public class TestsForNIS {
    private static AltUnityDriver altUnityDriver;
    String scene7 = "Assets/AltUnityTester/Examples/Scenes/Scene 7 Drag And Drop NIS";
    String scene8 = "Assets/AltUnityTester/Examples/Scenes/Scene 8 Draggable Panel NIP.unity";
    String scene9 = "Assets/AltUnityTester/Examples/Scenes/scene 9 NIS.unity";
    String scene10 = "Assets/AltUnityTester/Examples/Scenes/Scene 10 Sample NIS.unity";
    String scene11 = "Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity";

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

    public void loadLevel(String sceneName) throws Exception {

        AltLoadSceneParams params = new AltLoadSceneParams.Builder(sceneName).build();
        altUnityDriver.loadScene(params);
    }

    @Test
    public void TestScroll() throws Exception {
        loadLevel(scene10);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                "Player").build();

        AltUnityObject player = altUnityDriver.findObject(altFindObjectsParams);
        Boolean isScrolling = player
                .getComponentProperty(new AltGetComponentPropertyParams.Builder("AltUnityNIPDebugScript",
                        "wasScrolled").withAssembly(
                                "Assembly-CSharp")
                        .build(), Boolean.class);
        assertFalse(isScrolling);
        AltScrollParams altScrollParams = new AltScrollParams.Builder().withDuration(1).withSpeed(300)
                .withWait(true).build();
        altUnityDriver.scroll(altScrollParams);
        isScrolling = player.getComponentProperty(new AltGetComponentPropertyParams.Builder("AltUnityNIPDebugScript",
                "wasScrolled").withAssembly(
                        "Assembly-CSharp")
                .build(), Boolean.class);
        assertTrue(isScrolling);
    }

    @Test
    public void TestScrollElement() throws Exception {
        loadLevel(scene9);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                "Handle").build();

        AltUnityObject scrollbar = altUnityDriver.findObject(altFindObjectsParams);
        Vector2 scrollbarPosition = scrollbar.getScreenPosition();

        AltMoveMouseParams altMoveMouseParams = new AltMoveMouseParams.Builder(scrollbar.getScreenPosition())
                .withDuration(0.1f).build();
        altUnityDriver.moveMouse(altMoveMouseParams);
        Thread.sleep(1000);

        AltScrollParams altScrollParams = new AltScrollParams.Builder().withDuration(1).withSpeed(300)
                .withWait(true).build();
        altUnityDriver.scroll(altScrollParams);

        AltFindObjectsParams altFindObjectsParamsFinal = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                "Handle").build();
        AltUnityObject scrollbarFinal = altUnityDriver.findObject(altFindObjectsParamsFinal);
        Vector2 scrollbarPositionFinal = scrollbarFinal.getScreenPosition();
        assertNotEquals(scrollbarPosition, scrollbarPositionFinal);

    }

    public void TestClickElement() throws Exception {
        loadLevel(scene11);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();

        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParams);

        AltTapClickElementParams clickParams = new AltTapClickElementParams.Builder().build();
        capsule.click(clickParams);

        Boolean wasClicked = capsule
                .getComponentProperty(new AltGetComponentPropertyParams.Builder("AltUnityExampleNewInputSystem",
                        "wasClicked").withAssembly(
                                "Assembly-CSharp")
                        .build(), Boolean.class);
        assertTrue(wasClicked);
    }

    @Test
    public void TestClickCoordinates() throws Exception {
        loadLevel(scene11);
        AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.NAME,
                "Capsule").build();

        AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParams);

        AltTapClickCoordinatesParams clickParams = new AltTapClickCoordinatesParams.Builder(
                capsule.getScreenPosition()).build();
        altUnityDriver.click(clickParams);

        Boolean wasClicked = capsule
                .getComponentProperty(new AltGetComponentPropertyParams.Builder("AltUnityExampleNewInputSystem",
                        "wasClicked").withAssembly(
                                "Assembly-CSharp")
                        .build(), Boolean.class);
        assertTrue(wasClicked);
    }

}
