using System.Reflection;
using Altom.AltUnityDriver.Notifications;
using Altom.AltUnityTester.Communication;
using UnityEngine.SceneManagement;

namespace Altom.AltUnityTester.Notification
{
    public class AltUnityUnloadSceneNotification : BaseNotification
    {
        public AltUnityUnloadSceneNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
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