using System.Linq;
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindObjectCommand(params string[] parameters) : base(parameters) { }

        public override string Execute()
        {
            LogMessage("findObject for: " + ObjectName);

            string response = AltUnityErrors.errorNotFoundMessage;
            var path = ProcessPath(ObjectName);
            var isDirectChild = IsNextElementDirectChild(path[0]);
            var foundGameObject = FindObjects(null, path, 1, true, isDirectChild, Enabled);
            UnityEngine.Camera camera = null;
            if (!CameraPath.Equals("//"))
            {
                camera = GetCamera(CameraBy, CameraPath);
                if (camera == null)
                    return AltUnityErrors.errorCameraNotFound;
            }
            if (foundGameObject.Count() == 1)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(
                    AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject[0], camera));
            }
            return response;
        }
    }
}
