using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDropObjectCommand : AltUnityCommand
    {
        UnityEngine.Vector2 position;
        AltUnityObject altUnityObject;

        public AltUnityDropObjectCommand(params string[] parameters) : base(parameters, 4)
        {
            this.position = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[3]);
        }

        public override string Execute()
        {
            LogMessage("Drop object: " + altUnityObject);
            string response = AltUnityErrors.errorNotFoundMessage;
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            LogMessage("GameOBject: " + gameObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dropHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
            return response;
        }
    }
}
