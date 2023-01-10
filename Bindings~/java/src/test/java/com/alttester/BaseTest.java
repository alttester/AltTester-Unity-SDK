package com.alttester;

import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;

import com.alttester.Commands.UnityCommand.AltLoadSceneParams;

public class BaseTest {
    static AltDriver altDriver = TestsHelper.GetAltDriver();
    protected String sceneName;

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
        altDriver.resetInput();
        AltLoadSceneParams params = new AltLoadSceneParams.Builder(sceneName).build();
        altDriver.loadScene(params);
    }

}
