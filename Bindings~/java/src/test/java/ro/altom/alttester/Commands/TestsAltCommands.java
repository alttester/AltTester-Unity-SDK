package ro.altom.alttester.Commands;

import static org.junit.Assert.assertEquals;

import java.util.List;

import org.junit.AfterClass;
import org.junit.BeforeClass;
import org.junit.Test;

import ro.altom.alttester.AltDriver;
import ro.altom.alttester.TestsHelper;
import ro.altom.alttester.Commands.AltCommands.AltSetServerLoggingParams;
import ro.altom.alttester.Logging.AltLogLevel;
import ro.altom.alttester.Logging.AltLogger;

public class TestsAltCommands {

    class Rule {
        public List<String> Levels;
    }

    private static AltDriver altDriver;

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
    }

    @Test
    public void testSetServerLogging() {
        altDriver.setServerLogging(
                new AltSetServerLoggingParams.Builder(AltLogger.File, AltLogLevel.Debug).build());
        Rule rule = altDriver.callStaticMethod(
                new AltCallStaticMethodParams.Builder("Altom.AltTester.Logging.ServerLogManager",
                        "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new Object[] { "AltServerFileRule" }).build(),
                Rule.class);

        assertEquals(5, rule.Levels.size());

        altDriver.setServerLogging(
                new AltSetServerLoggingParams.Builder(AltLogger.File, AltLogLevel.Off).build());
        rule = altDriver.callStaticMethod(
                new AltCallStaticMethodParams.Builder("Altom.AltTester.Logging.ServerLogManager",
                        "Instance.Configuration.FindRuleByName", "Assembly-CSharp", new Object[] { "AltServerFileRule" }).build(),
                Rule.class);

        assertEquals(0, rule.Levels.size());
    }
}
