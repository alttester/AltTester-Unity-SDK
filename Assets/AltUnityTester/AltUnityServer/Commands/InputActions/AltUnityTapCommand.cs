using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTapCommand : AltUnityCommand
    {
        readonly AltUnityObject altUnityObject;
        readonly int count;

        public AltUnityTapCommand(params string[] parameters) : base(parameters, 4)
        {
            altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(Parameters[2]);
            count = string.IsNullOrEmpty(Parameters[3]) ? 1 : JsonConvert.DeserializeObject<int>(Parameters[3]);
            count = count < 1 ? 1 : count;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.ShowClick(new UnityEngine.Vector2(altUnityObject.getScreenPosition().x, altUnityObject.getScreenPosition().y));
            var response = AltUnityErrors.errorNotFoundMessage;
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);

            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
            gameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);

            for (var i = 0; i < count; i++)
                initiateClick(gameObject, pointerEventData);

            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
            gameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);

            var camera = AltUnityRunner._altUnityRunner.FoundCameraById(altUnityObject.idCamera);
            response = JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, camera));

            return response;
        }

        private void initiateClick(UnityEngine.GameObject gameObject, UnityEngine.EventSystems.PointerEventData pointerEventData)
        {
            pointerEventData.clickTime = UnityEngine.Time.unscaledTime;

            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
            gameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
            gameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
            gameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
            UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
            gameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
        }
    }
}
