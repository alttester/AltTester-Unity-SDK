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

using AltTester.AltTesterUnitySDK.Commands;
using UnityEngine.SceneManagement;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class AltUnloadSceneNotification : BaseNotification
    {
        public AltUnloadSceneNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            SceneManager.sceneUnloaded -= onSceneUnloaded;

            if (isOn)
            {
                SceneManager.sceneUnloaded += onSceneUnloaded;
            }
        }

        static void onSceneUnloaded(Scene scene)
        {
            SendNotification(scene.name, "unloadSceneNotification");
        }
    }
}
