using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityMoveMouseCommand :AltUnityCommand
    {
        UnityEngine.Vector2 location;
        float duration;

        public AltUnityMoveMouseCommand (Vector2 location, float duration)
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
