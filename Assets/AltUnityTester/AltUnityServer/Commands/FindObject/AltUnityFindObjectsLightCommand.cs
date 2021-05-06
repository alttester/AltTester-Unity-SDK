using System.Collections.Generic;
using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectsLightCommand : AltUnityBaseClassFindObjectsCommand
    {
        public AltUnityFindObjectsLightCommand(params string[] parameters) : base(parameters)
        {
        }

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
            var foundObjects = new List<AltUnityObjectLight>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path.FirstBound, false, Enabled))
            {
                foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObjectLight(testableObject, camera));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);
        }
    }
}
