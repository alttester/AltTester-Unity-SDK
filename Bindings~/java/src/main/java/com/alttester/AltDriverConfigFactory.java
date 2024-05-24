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

package com.alttester;

import java.net.URI;

import org.apache.logging.log4j.Level;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.core.LoggerContext;
import org.apache.logging.log4j.core.appender.ConsoleAppender;
import org.apache.logging.log4j.core.config.Configuration;
import org.apache.logging.log4j.core.config.ConfigurationFactory;
import org.apache.logging.log4j.core.config.ConfigurationSource;
import org.apache.logging.log4j.core.config.Order;
import org.apache.logging.log4j.core.config.builder.api.AppenderComponentBuilder;
import org.apache.logging.log4j.core.config.builder.api.ConfigurationBuilder;
import org.apache.logging.log4j.core.config.builder.api.LayoutComponentBuilder;
import org.apache.logging.log4j.core.config.builder.api.LoggerComponentBuilder;
import org.apache.logging.log4j.core.config.builder.impl.BuiltConfiguration;
import org.apache.logging.log4j.core.config.plugins.Plugin;

@Plugin(name = "AltDriverConfigFactory", category = ConfigurationFactory.CATEGORY)
@Order(50)

public class AltDriverConfigFactory extends ConfigurationFactory {

    @Override
    public Configuration getConfiguration(final LoggerContext loggerContext, final ConfigurationSource source) {
        return getConfiguration(loggerContext, source.toString(), null);
    }

    @Override
    public Configuration getConfiguration(final LoggerContext loggerContext, final String name,
            final URI configLocation) {
        ConfigurationBuilder<BuiltConfiguration> builder = newConfigurationBuilder();
        return createConfiguration(name, builder);
    }

    @Override
    protected String[] getSupportedTypes() {
        return new String[] { "*" };
    }

    public static void DisableLogging() {
        final LoggerContext ctx = (LoggerContext) LogManager.getContext(false);
        final Configuration config = ctx.getConfiguration();
        config.getLoggerConfig("com.alttester").setLevel(Level.OFF);

        ctx.updateLoggers();
    }

    static Configuration createConfiguration(final String name, ConfigurationBuilder<BuiltConfiguration> builder) {
        builder.setStatusLevel(Level.ERROR);
        builder.setConfigurationName(name);

        // create a console appender
        AppenderComponentBuilder consoleAppender = builder.newAppender("AltConsoleAppender", "Console")
                .addAttribute("target", ConsoleAppender.Target.SYSTEM_OUT);
        builder.add(consoleAppender);

        // create a rolling file appender

        AppenderComponentBuilder fileAppender = builder.newAppender("AltFileAppender", "File")
                .addAttribute("fileName", "./AltTester.log").addAttribute("append", false);

        builder.add(fileAppender);

        LayoutComponentBuilder standard = builder.newLayout("PatternLayout");
        standard.addAttribute("pattern", "%d [%t] %-5level: %msg%n%throwable");

        consoleAppender.add(standard);
        fileAppender.add(standard);

        LoggerComponentBuilder logger = builder.newLogger("com.alttester", Level.DEBUG);
        logger.add(builder.newAppenderRef("AltConsoleAppender"));
        logger.add(builder.newAppenderRef("AltFileAppender"));
        logger.addAttribute("additivity", false);

        builder.add(logger);

        return builder.build();
    }
}
