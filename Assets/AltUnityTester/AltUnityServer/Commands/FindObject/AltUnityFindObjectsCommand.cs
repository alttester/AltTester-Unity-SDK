using System.Collections.Generic;
using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectsCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindObjectsCommand(params string[] paramters) : base(paramters) { }

        public override string Execute()
        {
            UnityEngine.Camera camera = null;
            if (!CameraPath.Equals("//"))
            {
                camera = GetCamera(CameraBy, CameraPath);
                if (camera == null)
                    return AltUnityErrors.errorCameraNotFound;
            }
            var path = new PathSelector(ObjectPath);
            var foundObjects = new List<AltUnityObject>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path.FirstBound, false, Enabled))
            {
                foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(testableObject, camera));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);

        }
    }
}
