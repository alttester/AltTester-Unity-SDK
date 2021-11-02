using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;



namespace Altom.AltUnityTester.Commands
{
    class AltUnityDragObjectCommand : AltUnityCommand<AltUnityDragObjectParams, AltUnityObject>
    {
        public AltUnityDragObjectCommand(AltUnityDragObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltUnityObject Execute()
        {
            var mockUp = new AltUnityMockUpPointerInputModule();

            var pointerEventData = mockUp.ExecuteTouchEvent(new UnityEngine.Touch() { position = new UnityEngine.Vector2(CommandParams.position.x, CommandParams.position.y) });
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObject);
            UnityEngine.Camera viewingCamera = AltUnityRunner._altUnityRunner.FoundCameraById(CommandParams.altUnityObject.idCamera);
            UnityEngine.Vector3 gameObjectPosition = viewingCamera.WorldToScreenPoint(gameObject.transform.position);
            pointerEventData.delta = pointerEventData.position - new UnityEngine.Vector2(gameObjectPosition.x, gameObjectPosition.y);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dragHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(CommandParams.altUnityObject.idCamera);

            return camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject);
        }
    }
}
