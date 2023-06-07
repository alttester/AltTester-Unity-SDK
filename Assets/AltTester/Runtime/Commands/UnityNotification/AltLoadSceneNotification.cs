﻿/*
    Copyright(C) 2023  Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using AltTester.AltTesterUnitySDK.Driver.Notifications;
using AltTester.AltTesterUnitySDK.Communication;
using UnityEngine.SceneManagement;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class AltLoadSceneNotification : BaseNotification
    {
        public AltLoadSceneNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            SceneManager.sceneLoaded -= onSceneLoaded;

            if (isOn)
            {
                SceneManager.sceneLoaded += onSceneLoaded;
            }
        }

        static void onSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var data = new AltLoadSceneNotificationResultParams(scene.name, (AltLoadSceneMode)mode);
            SendNotification(data, "loadSceneNotification");
        }
    }
}