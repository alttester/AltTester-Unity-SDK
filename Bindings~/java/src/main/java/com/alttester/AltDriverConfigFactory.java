/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester;

import ch.qos.logback.classic.Level;
import ch.qos.logback.classic.Logger;
import ch.qos.logback.classic.LoggerContext;
import org.slf4j.ILoggerFactory;
import org.slf4j.LoggerFactory;

public final class AltDriverConfigFactory {

  private static final String ALTTESTER_LOGGER_NAME = "com.alttester";

  private AltDriverConfigFactory() {
  }

  public static void DisableLogging() {
    ILoggerFactory factory = LoggerFactory.getILoggerFactory();
    if (!(factory instanceof LoggerContext)) {
      return;
    }
    LoggerContext context = (LoggerContext) factory;
    Logger altLogger = context.getLogger(ALTTESTER_LOGGER_NAME);
    altLogger.setLevel(Level.OFF);
  }
}
