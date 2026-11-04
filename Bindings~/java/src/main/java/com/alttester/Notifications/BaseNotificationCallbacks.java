/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Notifications;

import com.alttester.Logging.AltLogLevel;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class BaseNotificationCallbacks implements INotificationCallbacks {
  protected static final Logger logger = LoggerFactory.getLogger(BaseNotificationCallbacks.class);

  @Override
  public void SceneLoadedCallBack(
      AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams) {
    logger.info(
        "Scene "
            + altLoadSceneNotificationResultParams.sceneName
            + " was loaded "
            + altLoadSceneNotificationResultParams.loadSceneMode);
  }

  @Override
  public void SceneUnloadedCallBack(String sceneName) {
    logger.info("Scene " + sceneName + " was unloaded ");
  }

  @Override
  public void LogCallBack(AltLogNotificationResultParams altLogNotificationResultParams) {
    logger.info(
        "Log of type "
            + AltLogLevel.values()[altLogNotificationResultParams.level]
            + " with message "
            + altLogNotificationResultParams.message
            + " was received");
  }

  @Override
  public void ApplicationPausedCallBack(boolean applicationPaused) {
    logger.info("Application paused: " + applicationPaused);
  }
}
