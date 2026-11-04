/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltFindObjectsCommand : AltBaseFindObjectsCommand<List<AltObject>>
    {
        public AltFindObjectsCommand(BaseGameFindObjectParams cmdParams) : base(cmdParams) { }

        public override List<AltObject> Execute()
        {
            UnityEngine.Camera camera = null;
            if (IsCameraSpecified(CommandParams.cameraConditions))
            {
                camera = GetCamera(CommandParams.cameraConditions);
                if (camera == null) throw new CameraNotFoundException();
            }
            var foundObjects = new List<AltObject>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, CommandParams.objectConditions, 0, false, CommandParams.enabled))
            {
                foundObjects.Add(AltRunner._altRunner.GameObjectToAltObject(testableObject, camera));
            }

            return foundObjects;
        }
    }


}
