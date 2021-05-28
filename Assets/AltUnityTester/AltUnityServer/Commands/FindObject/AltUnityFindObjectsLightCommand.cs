using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityFindObjectsLightCommand : AltUnityBaseClassFindObjectsCommand<List<AltUnityObjectLight>>
    {
        public AltUnityFindObjectsLightCommand(BaseFindObjectsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltUnityObjectLight> Execute()
        {
            UnityEngine.Camera camera = null;
            if (!CommandParams.cameraPath.Equals("//"))
            {
                camera = GetCamera(CommandParams.cameraBy, CommandParams.cameraPath);
                if (camera == null) throw new CameraNotFoundException();
            }
            var path = new PathSelector(CommandParams.path);
            var foundObjects = new List<AltUnityObjectLight>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path.FirstBound, false, CommandParams.enabled))
            {
                foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObjectLight(testableObject, camera));
            }

            return foundObjects;
        }
    }
}
