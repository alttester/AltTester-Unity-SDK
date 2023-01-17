using AltTester.AltDriver.Notifications;
using AltTester.Communication;
using UnityEngine.SceneManagement;

namespace AltTester.Notification
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