using AltTester.AltDriver;
using AltTester.AltDriver.Commands;



namespace AltTester.Commands
{
    class AltDragObjectCommand : AltCommand<AltDragObjectParams, AltObject>
    {
        public AltDragObjectCommand(AltDragObjectParams cmdParams) : base(cmdParams)
        {
        }

        public override AltObject Execute()
        {
            var mockUp = new AltMockUpPointerInputModule();

            var pointerEventData = mockUp.ExecuteTouchEvent(new UnityEngine.Touch() { position = new UnityEngine.Vector2(CommandParams.position.x, CommandParams.position.y) });
            UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);
            UnityEngine.Camera viewingCamera = AltRunner._altRunner.FoundCameraById(CommandParams.altObject.idCamera);
            UnityEngine.Vector3 gameObjectPosition = viewingCamera.WorldToScreenPoint(gameObject.transform.position);
            pointerEventData.delta = pointerEventData.position - new UnityEngine.Vector2(gameObjectPosition.x, gameObjectPosition.y);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dragHandler);
            var camera = AltRunner._altRunner.FoundCameraById(CommandParams.altObject.idCamera);

            return camera != null ? AltRunner._altRunner.GameObjectToAltObject(gameObject, camera) : AltRunner._altRunner.GameObjectToAltObject(gameObject);
        }
    }
}
