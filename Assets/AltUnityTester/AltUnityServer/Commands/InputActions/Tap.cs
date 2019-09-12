
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class Tap:Command
    {
        AltUnityObject altUnityObject;

        public Tap(AltUnityObject altUnityObject)
        {
            this.altUnityObject = altUnityObject;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("tapped object by name " + altUnityObject.name);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
                    var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                    UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
                    UnityEngine.Debug.Log("GameOBject: " + gameObject);

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
                    response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera) : AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject));
            return response;
        }
    }
}
