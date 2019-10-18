using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class TiltCommand:Command
    {
        UnityEngine.Vector3 acceleration;

        public TiltCommand(Vector3 acceleration)
        {
            this.acceleration = acceleration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            UnityEngine.Debug.Log("Tilt device with: " + acceleration);
            Input.acceleration = acceleration;
            return "OK";
#endif
            return null;
        }
    }
}
