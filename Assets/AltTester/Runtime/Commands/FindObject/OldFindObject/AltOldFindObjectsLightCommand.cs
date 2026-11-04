/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    //TODO remove this class after OldFindObject is no longer supported
    class AltOldFindObjectsLightCommand : AltOldBaseFindObjetsCommand<List<AltObjectLight>>
    {
        public AltOldFindObjectsLightCommand(BaseFindObjectsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltObjectLight> Execute()
        {
            var path = new OldPathSelector(CommandParams.path);
            var foundObjects = new List<AltObjectLight>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, path.FirstBound, false, CommandParams.enabled))
            {
                foundObjects.Add(AltRunner._altRunner.GameObjectToAltObjectLight(testableObject));
            }

            return foundObjects;
        }
    }
}