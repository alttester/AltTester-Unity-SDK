﻿using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltPointerEnterObjectCommand : AltCommand<AltPointerEnterObjectParams, AltObject>
    {

        public AltPointerEnterObjectCommand(AltPointerEnterObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
            var camera = AltRunner._altRunner.FoundCameraById(CommandParams.altObject.idCamera);
            return camera != null ? AltRunner._altRunner.GameObjectToAltObject(gameObject, camera) : AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }
    }
}
