using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityPointerDownFromObjectCommand : AltUnityCommand<AltUnityPointerDownFromObjectParams, AltUnityObject>
    {

        public AltUnityPointerDownFromObjectCommand(AltUnityPointerDownFromObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltUnityObject Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(CommandParams.altUnityObject.idCamera);
            if (camera != null)
            {
                return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera);
            }
            else
            {
                return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
            }
        }
    }
}
