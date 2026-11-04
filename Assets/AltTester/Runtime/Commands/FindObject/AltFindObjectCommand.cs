/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Linq;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltFindObjectCommand : AltBaseFindObjectsCommand<AltObject>
    {
        public AltFindObjectCommand(BaseGameFindObjectParams cmdParam) : base(cmdParam) { }

        public override AltObject Execute()
        {

            var foundGameObject = FindObjects(null, CommandParams.objectConditions, 0, true, CommandParams.enabled);
            UnityEngine.Camera camera = null;
            if (IsCameraSpecified(CommandParams.cameraConditions))
            {
                camera = GetCamera(CommandParams.cameraConditions);
                if (camera == null) throw new CameraNotFoundException();
            }
            if (foundGameObject.Count() >= 1)
            {
                return
                    AltRunner._altRunner.GameObjectToAltObject(foundGameObject[0], camera);
            }
            throw new NotFoundException(string.Format("Object not found"));
        }
    }
}
