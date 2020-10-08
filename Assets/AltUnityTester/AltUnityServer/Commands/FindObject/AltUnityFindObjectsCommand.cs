using System.Linq;
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectsCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindObjectsCommand(params string[] pieces) : base(pieces) { }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("findObjects for: " + ObjectName);
            UnityEngine.Camera camera = null;
            if (!CameraPath.Equals("//"))
            {
                camera = GetCamera(CameraBy, CameraPath);
                if (camera == null)
                    return AltUnityRunner._altUnityRunner.errorCameraNotFound;
            }
            var path = ProcessPath(ObjectName);
            var isDirectChild = IsNextElementDirectChild(path[0]);
            System.Collections.Generic.List<AltUnityObject> foundObjects = new System.Collections.Generic.List<AltUnityObject>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path, 1, false, isDirectChild, Enabled))
            {
                foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(testableObject, camera));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);

        }
    }
}
