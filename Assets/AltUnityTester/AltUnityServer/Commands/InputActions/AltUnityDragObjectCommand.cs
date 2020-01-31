namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDragObjectCommand : AltUnityCommand
    {
        UnityEngine.Vector2 position;
        AltUnityObject altUnityObject;

        public AltUnityDragObjectCommand (UnityEngine.Vector2 position, AltUnityObject altUnityObject)
        {
            this.position = position;
            this.altUnityObject = altUnityObject;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("Drag object: " + altUnityObject);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            AltUnityMockUpPointerInputModule mockUp = new AltUnityMockUpPointerInputModule();
            var pointerEventData = mockUp.ExecuteTouchEvent(new UnityEngine.Touch() { position = position });
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            UnityEngine.Camera viewingCamera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            UnityEngine.Vector3 gameObjectPosition = viewingCamera.WorldToScreenPoint(gameObject.transform.position);
            pointerEventData.delta = pointerEventData.position - new UnityEngine.Vector2(gameObjectPosition.x, gameObjectPosition.y);
            AltUnityRunner._altUnityRunner.LogMessage("GameOBject: " + gameObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dragHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
            return response;
        }
    }
}
