namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityPointerDownFromObjectCommand :AltUnityCommand
    {
        AltUnityObject altUnityObject;

        public AltUnityPointerDownFromObjectCommand (AltUnityObject altUnityObject)
        {
            this.altUnityObject = altUnityObject;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("PointerDown object: " + altUnityObject);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            AltUnityRunner._altUnityRunner.LogMessage("GameOBject: " + gameObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            if (camera != null)
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera));
            }
            else
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
            }
            return response;
        }
    }
}
