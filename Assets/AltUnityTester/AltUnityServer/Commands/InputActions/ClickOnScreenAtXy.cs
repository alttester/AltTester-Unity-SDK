using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class ClickOnScreenAtXy : Command
    {
        string x;
        string y;

        public ClickOnScreenAtXy(string x, string y)
        {
            this.x = x;
            this.y = y;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("Screen tapped at X:" + x + " Y:" + y);
            string response =  AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            MockUpPointerInputModule mockUp = new MockUpPointerInputModule();
            UnityEngine.Touch touch = new UnityEngine.Touch { position = new UnityEngine.Vector2(float.Parse(x), float.Parse(y)), phase = UnityEngine.TouchPhase.Began };
            var pointerEventData = mockUp.ExecuteTouchEvent(touch);
            if (pointerEventData.pointerPress == null &&
                pointerEventData.pointerEnter == null &&
                pointerEventData.pointerDrag == null)
            {
                response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            }
            else
            {
                UnityEngine.GameObject gameObject = pointerEventData.pointerPress.gameObject;

                AltUnityRunner._altUnityRunner.LogMessage("GameOBject: " + gameObject);

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
