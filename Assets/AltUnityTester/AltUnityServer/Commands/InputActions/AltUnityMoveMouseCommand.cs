using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityMoveMouseCommand : AltUnityCommand
    {
        UnityEngine.Vector2 location;
        float duration;

        public AltUnityMoveMouseCommand(params string[] parameters) : base(parameters, 4)
        {
            this.location = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.duration = JsonConvert.DeserializeObject<float>(parameters[3]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            LogMessage("moveMouse to: " + location);
            Input.MoveMouse(location, duration);
            return "Ok";
#else
            return null; ;
#endif
        }
    }
}
