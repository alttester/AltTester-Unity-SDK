using System.Reflection;
using AltTester.AltTesterUnitySDK.Driver.Notifications;
using AltTester.AltTesterUnitySDK.Communication;
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