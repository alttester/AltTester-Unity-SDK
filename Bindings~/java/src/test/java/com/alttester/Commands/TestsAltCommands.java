package com.alttester.Commands;

import static org.junit.jupiter.api.Assertions.assertEquals;

import java.util.List;

import org.junit.jupiter.api.Tag;
import org.junit.jupiter.api.Test;

import com.alttester.AltDriver;
import com.alttester.TestsHelper;
import com.alttester.Commands.AltCommands.AltSetServerLoggingParams;
import com.alttester.Logging.AltLogLevel;
import com.alttester.Logging.AltLogger;

public class TestsAltCommands {

        class Rule {
                public List<String> Levels;
        }

        @Test
		@Tag("WebGLUnsupported")
        public void testSetServerLogging() {
                AltDriver altDriver = TestsHelper.GetAltDriver();
                altDriver.setServerLogging(
                                new AltSetServerLoggingParams.Builder(AltLogger.File, AltLogLevel.Debug).build());
                Rule rule = altDriver.callStaticMethod(
                                new AltCallStaticMethodParams.Builder("Altom.AltTester.AltTesterUnitySdk.Logging.ServerLogManager",
                                                "Instance.Configuration.FindRuleByName", "Assembly-CSharp",
                                                new Object[] { "AltServerFileRule" })
                                                .build(),
                                Rule.class);

                assertEquals(5, rule.Levels.size());

                altDriver.setServerLogging(
                                new AltSetServerLoggingParams.Builder(AltLogger.File, AltLogLevel.Off).build());
                rule = altDriver.callStaticMethod(
                                new AltCallStaticMethodParams.Builder("Altom.AltTester.AltTesterUnitySdk.Logging.ServerLogManager",
                                                "Instance.Configuration.FindRuleByName", "Assembly-CSharp",
                                                new Object[] { "AltServerFileRule" })
                                                .build(),
                                Rule.class);

                assertEquals(0, rule.Levels.size());

                // Reset logging level
                altDriver.setServerLogging(
                                new AltSetServerLoggingParams.Builder(AltLogger.File, AltLogLevel.Debug).build());
        }
}
