/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterUnitySDK.Commands;
using UnityEngine.SceneManagement;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class AltUnloadSceneNotification : BaseNotification
    {
        public AltUnloadSceneNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            StopSceneUnloaded();

            if (isOn)
            {
                SceneManager.sceneUnloaded += onSceneUnloaded;
            }
        }

        static void onSceneUnloaded(Scene scene)
        {
            SendNotification(scene.name, "unloadSceneNotification");
        }
        public static void StopSceneUnloaded()
        {
            SceneManager.sceneUnloaded -= onSceneUnloaded;
        }
    }
}
