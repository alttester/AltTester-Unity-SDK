/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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
                AltDriver altDriver = TestsHelper.getAltDriver();
                altDriver.setServerLogging(
                                new AltSetServerLoggingParams.Builder(AltLogger.File, AltLogLevel.Debug).build());
                Rule rule = altDriver.callStaticMethod(
                                new AltCallStaticMethodParams.Builder(
                                                "Altom.AltTester.AltTesterUnitySDK.Logging.ServerLogManager",
                                                "Instance.Configuration.FindRuleByName", "Assembly-CSharp",
                                                new Object[] { "AltServerFileRule" })
                                                .build(),
                                Rule.class);

                assertEquals(5, rule.Levels.size());

                altDriver.setServerLogging(
                                new AltSetServerLoggingParams.Builder(AltLogger.File, AltLogLevel.Off).build());
                rule = altDriver.callStaticMethod(
                                new AltCallStaticMethodParams.Builder(
                                                "Altom.AltTester.AltTesterUnitySDK.Logging.ServerLogManager",
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
