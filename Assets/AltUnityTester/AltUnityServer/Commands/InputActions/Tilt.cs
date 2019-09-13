using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class Tilt:Command
    {
        UnityEngine.Vector3 acceleration;

        public Tilt(Vector3 acceleration)
        {
            this.acceleration = acceleration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.LogMessage("Tilt device with: " + acceleration);
            Input.acceleration = acceleration;
            return "OK";
#endif
            return null;
        }
    }
}
