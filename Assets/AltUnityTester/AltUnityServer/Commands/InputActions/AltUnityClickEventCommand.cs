using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickEventCommand : AltUnityCommand
    {
        AltUnityObject altUnityObject;

        public AltUnityClickEventCommand(params string[] parameters) : base(parameters, 3)
        {
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[2]);
        }

        public override string Execute()
        {
            LogMessage("ClickEvent on " + altUnityObject);
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(altUnityObject.getScreenPosition().x, altUnityObject.getScreenPosition().y));

            string response = AltUnityErrors.errorNotFoundMessage;
            UnityEngine.GameObject foundGameObject = AltUnityRunner.GetGameObject(altUnityObject);
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.EventSystems.ExecuteEvents.Execute(foundGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject));
            return response;
        }
    }
}
