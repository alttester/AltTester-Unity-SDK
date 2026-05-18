/*
    Copyright(C) 2026 Altom Consulting

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

import ch.qos.logback.classic.Level;
import ch.qos.logback.classic.Logger;
import ch.qos.logback.classic.LoggerContext;
import org.slf4j.ILoggerFactory;
import org.slf4j.LoggerFactory;

public final class AltDriverConfigFactory {

  private static final String ALTTESTER_LOGGER_NAME = "com.alttester";

  private AltDriverConfigFactory() {}

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
