using AltTester.AltDriver;
using AltTester.AltDriver.Commands;

namespace AltTester.Commands
{
    class AltPointerUpFromObjectCommand : AltCommand<AltPointerUpFromObjectParams, AltObject>
    {
        public AltPointerUpFromObjectCommand(AltPointerUpFromObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
            var camera = AltRunner._altRunner.FoundCameraById(CommandParams.altObject.idCamera);

            return camera != null ?
                AltRunner._altRunner.GameObjectToAltObject(gameObject, camera) :
                AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }
    }
}
