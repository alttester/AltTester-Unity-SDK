package ro.altom.altunitytester.Commands;

import static org.junit.Assert.assertEquals;

import java.util.List;

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
        altUnityDriver = new AltUnityDriver("127.0.0.1", 13010, true);
    }

    @AfterClass
    public static void tearDown() throws Exception {
        altUnityDriver.stop();
    }

    @Test
    public void testSetServerLogging() {
        altUnityDriver.setServerLogging(
                new AltSetServerLoggingParameters.Builder(AltUnityLogger.File, AltUnityLogLevel.Debug).build());
        Rule rule = altUnityDriver
                .callStaticMethod(new AltCallStaticMethodParameters.Builder("Altom.Server.Logging.ServerLogManager",
                        "Instance.Configuration.FindRuleByName", new Object[] { "AltUnityServerFileRule" })
                                .withAssembly("Assembly-CSharp").build(),
                        Rule.class);

        assertEquals(5, rule.Levels.size());

        altUnityDriver.setServerLogging(
                new AltSetServerLoggingParameters.Builder(AltUnityLogger.File, AltUnityLogLevel.Off).build());
        rule = altUnityDriver
                .callStaticMethod(new AltCallStaticMethodParameters.Builder("Altom.Server.Logging.ServerLogManager",
                        "Instance.Configuration.FindRuleByName", new Object[] { "AltUnityServerFileRule" })
                                .withAssembly("Assembly-CSharp").build(),
                        Rule.class);

        assertEquals(0, rule.Levels.size());
    }
}
