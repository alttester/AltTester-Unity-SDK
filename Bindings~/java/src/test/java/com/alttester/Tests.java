package com.alttester;

// For more info on the setup, check out our docs https://alttester.com/docs/sdk/latest/pages/get-started.html#write-and-execute-first-test-for-your-app  
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNull;
import com.google.gson.JsonElement;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonParser;
import com.alttester.Commands.AltCommands.AltSetServerLoggingParams;
import com.alttester.Commands.FindObject.*;
import com.alttester.Commands.UnityCommand.*;
import com.alttester.Logging.AltLogLevel;
import com.alttester.Logging.AltLogger;
import com.alttester.Commands.InputActions.*;
import com.alttester.UnityStruct.*;
import com.alttester.Commands.ObjectCommand.*;
import com.alttester.position.Vector2;
import com.alttester.AltObject;
import com.alttester.AltDriver;

public class Tests {
    private static AltDriver altDriver;
    public String SkyAtmospherePath = "/SkyAtmosphere";

    @BeforeAll
    public static void oneTimeSetUp() throws Exception {
        altDriver = new AltDriver("127.0.0.1", 13000, true, 60, "__default__");

        // You might want to load the scene here
        // altDriver.loadScene(new AltLoadSceneParams.Builder("Scene1").build());
    }

    @AfterAll
    public static void oneTimeTearDown() throws Exception {
        altDriver.stop();
    }

    @Test
    public void Test() throws InterruptedException {
        AltObject SkyAtmosphere = altDriver.waitForObject(new AltWaitForObjectsParams.Builder(
                new AltFindObjectsParams.Builder(AltDriver.By.PATH, SkyAtmospherePath).build()).withTimeout(1)
                .build());
    }
}
