/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class Utils {
  protected static final Logger logger = LoggerFactory.getLogger(Utils.class);

  /**
   * Sleeps for certain amount of seconds.
   *
   * @param interval Seconds to sleep for.
   */
  public static void sleepFor(double interval) {
    long timeToSleep = (long) (interval * 1000);
    try {
      Thread.sleep(timeToSleep);
    } catch (InterruptedException e) {
      logger.warn("Could not sleep for " + timeToSleep + " ms");
    }
  }
}
