/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Notifications;
using AltTester.AltTesterUnitySDK.Commands;
using UnityEngine.SceneManagement;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class AltLoadSceneNotification : BaseNotification
    {
        public AltLoadSceneNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            StopSceneLoaded();

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
        public static void StopSceneLoaded()
        {
            SceneManager.sceneLoaded -= onSceneLoaded;
        }
    }
}
