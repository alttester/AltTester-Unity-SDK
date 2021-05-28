using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityClickOnScreenAtXyCommand : AltUnityCommand<AltUnityTapScreenParams, AltUnityObject>
    {
        public AltUnityClickOnScreenAtXyCommand(AltUnityTapScreenParams cmdParams) : base(cmdParams)
        {
        }

        public override AltUnityObject Execute()
        {
            var clickPosition = new UnityEngine.Vector2(CommandParams.x, CommandParams.y);
            AltUnityRunner._altUnityRunner.ShowClick(clickPosition);
            var mockUp = new AltUnityMockUpPointerInputModule();
            var touch = new UnityEngine.Touch { position = clickPosition, phase = UnityEngine.TouchPhase.Began };
            var pointerEventData = mockUp.ExecuteTouchEvent(touch);
            if (pointerEventData.pointerPress == null &&
                pointerEventData.pointerEnter == null &&
                pointerEventData.pointerDrag == null)
            {
                throw new NotFoundException();
            }

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

            return AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(gameObject, pointerEventData.enterEventCamera);
        }
    }
}
