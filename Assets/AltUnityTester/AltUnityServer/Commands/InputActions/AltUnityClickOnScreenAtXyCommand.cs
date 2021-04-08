using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickOnScreenAtXyCommand : AltUnityCommand
    {
        readonly float x;
        readonly float y;

        public AltUnityClickOnScreenAtXyCommand(params string[] parameters) : base(parameters, 4)
        {
            this.x = JsonConvert.DeserializeObject<float>(parameters[2]);
            this.y = JsonConvert.DeserializeObject<float>(parameters[3]);
        }

        public override string Execute()
        {
            var clickPosition = new UnityEngine.Vector2(x, y);
            AltUnityRunner._altUnityRunner.ShowClick(clickPosition);
            string response = AltUnityErrors.errorNotFoundMessage;
            var mockUp = new AltUnityMockUpPointerInputModule();
            var touch = new UnityEngine.Touch { position = clickPosition, phase = UnityEngine.TouchPhase.Began };
            var pointerEventData = mockUp.ExecuteTouchEvent(touch);
            if (pointerEventData.pointerPress == null &&
                pointerEventData.pointerEnter == null &&
                pointerEventData.pointerDrag == null)
            {
                response = AltUnityErrors.errorNotFoundMessage;
            }
            else
            {
                UnityEngine.GameObject gameObject = pointerEventData.pointerPress.gameObject;

                gameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
                gameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
                gameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
                gameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
                gameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
                gameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
                touch.phase = UnityEngine.TouchPhase.Ended;
                mockUp.ExecuteTouchEvent(touch, pointerEventData);

                response = JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, pointerEventData.enterEventCamera));
            }
            return response;
        }
    }
}
