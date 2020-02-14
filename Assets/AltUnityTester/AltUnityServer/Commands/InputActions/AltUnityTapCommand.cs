namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTapCommand :AltUnityCommand
    {
        AltUnityObject altUnityObject;

        public AltUnityTapCommand (AltUnityObject altUnityObject)
        {
            this.altUnityObject = altUnityObject;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("tapped object by name " + altUnityObject.name);
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(altUnityObject.getScreenPosition().x, altUnityObject.getScreenPosition().y));
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
            AltUnityRunner._altUnityRunner.LogMessage("GameOBject: " + gameObject);

            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
            gameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            gameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
            gameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
            gameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            gameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
            gameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);

            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera));
            
            return response;
        }
    }
}
