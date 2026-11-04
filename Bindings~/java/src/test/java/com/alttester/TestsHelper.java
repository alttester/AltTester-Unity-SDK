/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester;

public class TestsHelper {
  public static int getAltDriverPort() {
    String port = System.getenv("ALTSERVER_PORT");

    if (port != null && port != "") {
      return Integer.parseInt(port);
    }

    return 13000;
  }

  public static String getAltDriverHost() {
    String host = System.getenv("ALTSERVER_HOST");

    if (host != null && host != "") {
      return host;
    }

    return "127.0.0.1";
  }

  public static String getAltDriverPlatformVersion() {
    String platformVersion = System.getenv("ALTSERVER_PLATFORM_VERSION");
    if (platformVersion != null && !platformVersion.isEmpty()) {
      return platformVersion;
    }
    return "unknown";
  }

  public static AltDriver getAltDriver() {
    return new AltDriver(
        TestsHelper.getAltDriverHost(),
        TestsHelper.getAltDriverPort(),
        true,
        60,
        "__default__",
        "unknown",
        TestsHelper.getAltDriverPlatformVersion(),
        "unknown",
        "unknown");
  }
}
