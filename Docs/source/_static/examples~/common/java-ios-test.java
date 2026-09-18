// On iOS there is no driver-side reverse port forwarding: IProxy cannot set it up.
// Connect over the network instead - see "In case of iOS" in Advanced Usage for the
// personal-hotspot-over-USB workaround. The test code itself is unchanged; only the
// IP configured in the instrumented app differs.
import com.alttester.AltDriver;
import com.alttester.AltObject;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.AltDriverCommands.AltLoadSceneParams;
import org.junit.jupiter.api.*;

public class MyFirstTest {
    private static AltDriver altDriver;

    @BeforeAll
    public static void setUp() {
        altDriver = new AltDriver();
    }

    @AfterAll
    public static void tearDown() {
        altDriver.stop();
    }

    @Test
    public void testStartGame() {
        altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").build());

        altDriver.findObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Close Button").build()).tap();
        altDriver.findObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Button").build()).tap();

        AltObject panelElement = altDriver.waitForObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Panel").build());
        Assertions.assertTrue(panelElement.enabled);
    }
}
