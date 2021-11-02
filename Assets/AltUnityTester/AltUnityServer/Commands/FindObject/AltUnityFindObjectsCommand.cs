using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityFindObjectsCommand : AltUnityBaseClassFindObjectsCommand<List<AltUnityObject>>
    {
        public AltUnityFindObjectsCommand(BaseFindObjectsParams cmdParams) : base(cmdParams) { }

        public override List<AltUnityObject> Execute()
        {
            UnityEngine.Camera camera = null;
            if (!CommandParams.cameraPath.Equals("//"))
            {
                camera = GetCamera(CommandParams.cameraBy, CommandParams.cameraPath);
                if (camera == null) throw new CameraNotFoundException();
            }
            var path = new PathSelector(CommandParams.path);
            var foundObjects = new List<AltUnityObject>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path.FirstBound, false, CommandParams.enabled))
            {
                foundObjects.Add(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(testableObject, camera));
            }

            return foundObjects;
        }
    }
}
