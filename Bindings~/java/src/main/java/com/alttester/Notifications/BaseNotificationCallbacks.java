/*
    Copyright(C) 2025 Altom Consulting

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

package com.alttester.Notifications;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import com.alttester.Logging.AltLogLevel;

public class BaseNotificationCallbacks implements INotificationCallbacks {
    protected static final Logger logger = LogManager.getLogger(BaseNotificationCallbacks.class);

    @Override
    public void SceneLoadedCallBack(
            AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams) {
        logger.info("Scene " + altLoadSceneNotificationResultParams.sceneName + " was loaded "
                + altLoadSceneNotificationResultParams.loadSceneMode);
    }

    @Override
    public void SceneUnloadedCallBack(String sceneName) {
        logger.info("Scene " + sceneName + " was unloaded ");
    }

    @Override
    public void LogCallBack(AltLogNotificationResultParams altLogNotificationResultParams) {
        logger.info("Log of type " + AltLogLevel.values()[altLogNotificationResultParams.level] + " with message " +
                altLogNotificationResultParams.message + " was received");
    }

    @Override
    public void ApplicationPausedCallBack(boolean applicationPaused) {
        logger.info("Application paused: " + applicationPaused);
    }
}
