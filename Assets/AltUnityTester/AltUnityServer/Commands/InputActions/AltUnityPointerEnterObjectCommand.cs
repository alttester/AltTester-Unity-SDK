using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityPointerEnterObjectCommand : AltUnityCommand<AltUnityPointerEnterObjectParams, AltUnityObject>
    {

        public AltUnityPointerEnterObjectCommand(AltUnityPointerEnterObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltUnityObject Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(CommandParams.altUnityObject.idCamera);
            return camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
        }
    }
}
