namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickEventCommand : AltUnityCommand
    {
        AltUnityObject altUnityObject;

        public AltUnityClickEventCommand (AltUnityObject altObject)
        {
            this.altUnityObject = altObject;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("ClickEvent on " + altUnityObject);
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(altUnityObject.getScreenPosition().x, altUnityObject.getScreenPosition().y));
            
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.GameObject foundGameObject = AltUnityRunner.GetGameObject(altUnityObject);
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.EventSystems.ExecuteEvents.Execute(foundGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(foundGameObject));
            return response;
        }
    }
}
