using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindActiveObjectsByNameCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindActiveObjectsByNameCommand(params string[] parameters) : base(parameters)
        {
        }

        public override string Execute()
        {
            LogMessage("findActiveObjectByName for: " + ObjectName);

            string response = AltUnityErrors.errorNotFoundMessage;

            var foundGameObject = UnityEngine.GameObject.Find(ObjectName);
            if (foundGameObject != null)
            {
                UnityEngine.Camera camera = null;
                if (!CameraPath.Equals("//"))
                {
                    camera = GetCamera(CameraBy, CameraPath);
                    if (camera == null)
                        return AltUnityErrors.errorCameraNotFound;
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(
                    AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject, camera));

            }
            return response;
        }
    }
}
