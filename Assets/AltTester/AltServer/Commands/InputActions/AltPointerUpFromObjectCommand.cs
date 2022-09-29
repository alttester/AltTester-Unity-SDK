using Altom.AltDriver;
using Altom.AltDriver.Commands;

namespace Altom.AltTester.Commands
{
    class AltPointerUpFromObjectCommand : AltCommand<AltPointerUpFromObjectParams, AltObject>
    {
        public AltPointerUpFromObjectCommand(AltPointerUpFromObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altUnityObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
            var camera = AltRunner._altUnityRunner.FoundCameraById(CommandParams.altUnityObject.idCamera);

            return camera != null ?
                AltRunner._altUnityRunner.GameObjectToAltObject(gameObject, camera) :
                AltRunner._altUnityRunner.GameObjectToAltObject(gameObject);
        }
    }
}
