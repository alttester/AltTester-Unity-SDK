/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    //TODO remove this class after OldFindObject is no longer supported
    class AltOldFindObjectCommand : AltOldBaseFindObjetsCommand<AltObject>
    {
        public AltOldFindObjectCommand(BaseFindObjectsParams cmdParam) : base(cmdParam) { }

        public override AltObject Execute()
        {
            UnityEngine.Debug.Log("OlfFindObject " + CommandParams.path);
            var path = new OldPathSelector(CommandParams.path);
            var foundGameObject = FindObjects(null, path.FirstBound, true, CommandParams.enabled);
            UnityEngine.Camera camera = null;
            if (!CommandParams.cameraPath.Equals("//"))
            {
                camera = GetCamera(CommandParams.cameraBy, CommandParams.cameraPath);
                if (camera == null) throw new CameraNotFoundException();
            }
            if (foundGameObject.Count() >= 1)
            {
                return
                    AltRunner._altRunner.GameObjectToAltObject(foundGameObject[0], camera);
            }
            throw new NotFoundException(string.Format("Object {0} not found", CommandParams.path));
        }
    }
}