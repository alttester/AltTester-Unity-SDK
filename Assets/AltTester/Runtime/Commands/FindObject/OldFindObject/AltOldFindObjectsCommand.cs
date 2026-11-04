/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    //TODO remove this class after OldFindObject is no longer supported
    class AltOldFindObjectsCommand : AltOldBaseFindObjetsCommand<List<AltObject>>
    {
        public AltOldFindObjectsCommand(BaseFindObjectsParams cmdParams) : base(cmdParams) { }

        public override List<AltObject> Execute()
        {
            UnityEngine.Debug.Log("OlfFindObject " + CommandParams.path);
            UnityEngine.Camera camera = null;
            if (!CommandParams.cameraPath.Equals("//"))
            {
                camera = GetCamera(CommandParams.cameraBy, CommandParams.cameraPath);
                if (camera == null) throw new CameraNotFoundException();
            }
            var path = new OldPathSelector(CommandParams.path);
            var foundObjects = new List<AltObject>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path.FirstBound, false, CommandParams.enabled))
            {
                foundObjects.Add(AltRunner._altRunner.GameObjectToAltObject(testableObject, camera));
            }

            return foundObjects;
        }
    }
}