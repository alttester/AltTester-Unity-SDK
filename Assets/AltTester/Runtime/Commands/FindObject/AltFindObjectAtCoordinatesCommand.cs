/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltFindObjectAtCoordinatesCommand : AltCommand<AltFindObjectAtCoordinatesParams, AltObject>
    {
        public AltFindObjectAtCoordinatesCommand(AltFindObjectAtCoordinatesParams cmdParam) : base(cmdParam) { }

        public override AltObject Execute()
        {
            var result = FindObjectViaRayCast.FindObjectAtCoordinates(new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y));

            if (result.obj == null) return null;

            if (result.isGameObject)
            {
                return AltRunner._altRunner.GameObjectToAltObject(result.obj as GameObject);
            }

            return null;
        }


    }
}
