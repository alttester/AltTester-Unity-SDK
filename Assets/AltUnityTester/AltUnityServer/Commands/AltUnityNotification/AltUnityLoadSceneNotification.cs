using System.Reflection;
using Altom.AltUnityDriver.Notifications;
using Altom.AltUnityTester.Communication;
using UnityEngine.SceneManagement;

namespace Altom.AltUnityTester.Notification
{
    public class AltUnityLoadSceneNotification : BaseNotification
    {
        public AltUnityLoadSceneNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            SceneManager.sceneLoaded -= onSceneLoaded;

            if (isOn)
            {
                SceneManager.sceneLoaded += onSceneLoaded;
            }

        }

        static void onSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var data = new AltUnityLoadSceneNotificationResultParams(scene.name, (AltUnityLoadSceneMode)mode);
            SendNotification(data, "loadSceneNotification");
        }
    }
}