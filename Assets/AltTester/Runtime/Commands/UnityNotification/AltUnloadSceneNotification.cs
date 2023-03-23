using System.Reflection;
using AltTester.AltTesterUnitySdk.Driver.Notifications;
using AltTester.AltTesterUnitySdk.Communication;
using UnityEngine.SceneManagement;

namespace AltTester.AltTesterUnitySdk.Notification
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