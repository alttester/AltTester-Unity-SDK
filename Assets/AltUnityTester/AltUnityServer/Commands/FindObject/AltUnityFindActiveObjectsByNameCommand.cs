using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindActiveObjectsByNameCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindActiveObjectsByNameCommand(params string[] pieces) : base(pieces)
        {
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("findActiveObjectByName for: " + ObjectName);

            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;

            var foundGameObject = UnityEngine.GameObject.Find(ObjectName);
            if (foundGameObject != null)
            {
                UnityEngine.Camera camera = null;
                if (!CameraPath.Equals("//"))
                {
                    camera = GetCamera(CameraBy, CameraPath);
                    if (camera == null)
                        return AltUnityRunner._altUnityRunner.errorCameraNotFound;
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(
                    AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject, camera));

            }
            return response;
        }
    }
}
