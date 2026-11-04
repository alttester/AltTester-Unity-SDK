/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltFindObjectsLightCommand : AltBaseFindObjectsCommand<List<AltObjectLight>>
    {
        public AltFindObjectsLightCommand(BaseGameFindObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltObjectLight> Execute()
        {
            var foundObjects = new List<AltObjectLight>();
            foreach (UnityEngine.GameObject testableObject in FindObjects(null, CommandParams.objectConditions, 0, false, CommandParams.enabled))
            {
                foundObjects.Add(AltRunner._altRunner.GameObjectToAltObjectLight(testableObject));
            }

            return foundObjects;
        }
    }
}
