
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindActiveObjectsByNameCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindActiveObjectsByNameCommand(params string[] parameters) : base(parameters)
        {
        }

        public override string Execute()
        {
            var foundGameObject = UnityEngine.GameObject.Find(ObjectPath);
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
            return AltUnityErrors.errorNotFoundMessage;
        }
    }
}
