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

import com.alttester.Logging.AltLogLevel;
import com.alttester.Notifications.AltLoadSceneNotificationResultParams;
import com.alttester.Notifications.AltLogNotificationResultParams;
import com.alttester.Notifications.BaseNotificationCallbacks;

public class MockNotificationCallBacks extends BaseNotificationCallbacks {
    public static String lastLoadedScene;
    public static String lastUnloadedScene;
    public static String logMessage;
    public static String logStackTrace;
    public static AltLogLevel logLevel = AltLogLevel.Error;
    public static boolean applicationPaused;

    @Override
    public void SceneLoadedCallBack(
            AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams) {
        lastLoadedScene = altLoadSceneNotificationResultParams.sceneName;
    }

    @Override
    public void SceneUnloadedCallBack(String sceneName) {
        lastUnloadedScene = sceneName;
    }

    @Override
    public void LogCallBack(AltLogNotificationResultParams altLogNotificationResultParams) {
        logMessage = altLogNotificationResultParams.message;
        logStackTrace = altLogNotificationResultParams.stackTrace;
        logLevel = AltLogLevel.values()[altLogNotificationResultParams.level];
    }

    @Override
    public void ApplicationPausedCallBack(boolean applicationPaused) {
        MockNotificationCallBacks.applicationPaused = applicationPaused;
    }
}
