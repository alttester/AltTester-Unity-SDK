using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDropObjectCommand : AltUnityCommand
    {
        //TODO: TO REMOVE 
        private readonly UnityEngine.Vector2 position;
        private readonly AltUnityObject altUnityObject;

        public AltUnityDropObjectCommand(params string[] parameters) : base(parameters, 4)
        {
            this.position = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[3]);
        }

        public override string Execute()
        {
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dropHandler);
            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            string response = JsonConvert.SerializeObject(camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
            return response;
        }
    }
}
