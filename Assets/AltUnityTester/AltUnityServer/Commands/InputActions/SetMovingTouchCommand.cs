using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class SetMovingTouchCommand:Command
    {
        UnityEngine.Vector2 start;
        UnityEngine.Vector2 destination;
        string duration;

        public SetMovingTouchCommand(UnityEngine.Vector2 start, UnityEngine.Vector2 destination, string duration)
        {
            this.start = start;
            this.destination = destination;
            this.duration = duration;
        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.LogMessage("Touch at: " + start);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.Touch touch = new UnityEngine.Touch();
            touch.phase = UnityEngine.TouchPhase.Began;
            touch.position = start;
            System.Collections.Generic.List<UnityEngine.Touch> touches = Input.touches.ToList();
            touches.Sort((touch1, touch2) => (touch1.fingerId.CompareTo(touch2.fingerId)));
            int fingerId = 0;
            foreach (UnityEngine.Touch iter in touches)
            {
                if (iter.fingerId != fingerId)
                    break;
                fingerId++;
            }

            touch.fingerId = fingerId;
            Input.SetMovingTouch(touch, destination, float.Parse(duration));
            response = "Ok";
            return response;
#endif
            return null;
        }
    }
}
