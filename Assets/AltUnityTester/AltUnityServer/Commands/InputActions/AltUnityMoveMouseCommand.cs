using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityMoveMouseCommand : AltUnityCommand
    {
        Vector2 location;
        readonly float duration;

        public AltUnityMoveMouseCommand(params string[] parameters) : base(parameters, 4)
        {
            this.location = JsonConvert.DeserializeObject<Vector2>(parameters[2]);
            this.duration = JsonConvert.DeserializeObject<float>(parameters[3]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.MoveMouse(location, duration);
            return "Ok";
#else
            return null; ;
#endif
        }
    }
}
