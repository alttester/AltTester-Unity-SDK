package ro.altom.altunitytester.Commands;

import static org.junit.Assert.assertEquals;

import java.util.List;

import com.google.gson.Gson;

import org.junit.AfterClass;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.AltUnityCommands.AltSetServerLoggingParameters;
import ro.altom.altunitytester.Logging.AltUnityLogLevel;
import ro.altom.altunitytester.Logging.AltUnityLogger;

public class TestsAltUnityCommands {

    class Rule {
        public List<String> Levels;
    }

    private static AltUnityDriver altUnityDriver;

    @BeforeClass
    public static void setUp() throws Exception {
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13000, ";", "&", true);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
    }

    @Test
    public void testSetServerLogging() {
        String result = altUnityDriver
                .callStaticMethod(new AltCallStaticMethodParameters.Builder("Altom.Server.Logging.ServerLogManager",
                        "Instance.Configuration.FindRuleByName", "AltUnityServerFileRule")
                                .withAssembly("Assembly-CSharp").build());

        Rule rule = new Gson().fromJson(result, Rule.class);

        assertEquals(5, rule.Levels.size());

        altUnityDriver.setServerLogging(
                new AltSetServerLoggingParameters.Builder(AltUnityLogger.File, AltUnityLogLevel.Off).build());
        result = altUnityDriver
                .callStaticMethod(new AltCallStaticMethodParameters.Builder("Altom.Server.Logging.ServerLogManager",
                        "Instance.Configuration.FindRuleByName", "AltUnityServerFileRule")
                                .withAssembly("Assembly-CSharp").build());

        rule = new Gson().fromJson(result, Rule.class);
        assertEquals(0, rule.Levels.size());
    }
}
