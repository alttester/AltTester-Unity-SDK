using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class MoveMouseCommand:Command
    {
        UnityEngine.Vector2 location;
        float duration;

        public MoveMouseCommand(Vector2 location, float duration)
        {
            this.location = location;
            this.duration = duration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
                AltUnityRunner._altUnityRunner.LogMessage("moveMouse to: " + location);
                Input.MoveMouse(location, duration);
                return "Ok";
#endif
            return null; ;
        }
    }
}
