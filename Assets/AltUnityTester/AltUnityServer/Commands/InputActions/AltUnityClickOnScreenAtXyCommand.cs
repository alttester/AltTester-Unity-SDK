namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickOnScreenAtXyCommand : AltUnityCommand
    {
        string x;
        string y;

        public AltUnityClickOnScreenAtXyCommand(params string[] parameters) : base(parameters, 4)
        {
            this.x = parameters[2];
            this.y = parameters[3];
        }

        public override string Execute()
        {
            LogMessage("Screen tapped at X:" + x + " Y:" + y);
            var clickPosition = new UnityEngine.Vector2(float.Parse(x), float.Parse(y));
            AltUnityRunner._altUnityRunner.ShowClick(clickPosition);
            string response = AltUnityErrors.errorNotFoundMessage;
            AltUnityMockUpPointerInputModule mockUp = new AltUnityMockUpPointerInputModule();
            UnityEngine.Touch touch = new UnityEngine.Touch { position = clickPosition, phase = UnityEngine.TouchPhase.Began };
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

                LogMessage("GameOBject: " + gameObject);

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

                response = Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, pointerEventData.enterEventCamera));
            }
            return response;
        }
    }
}
