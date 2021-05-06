using System.Linq;
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindObjectCommand(params string[] parameters) : base(parameters) { }

        public override string Execute()
        {
            var path = new PathSelector(ObjectPath);
            var foundGameObject = FindObjects(null, path.FirstBound, true, Enabled);
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
            return AltUnityErrors.errorNotFoundMessage;
        }
    }
}
