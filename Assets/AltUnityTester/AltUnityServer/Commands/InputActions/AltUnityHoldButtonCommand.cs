using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHoldButtonCommand : AltUnityCommand
    {
#if ALTUNITYTESTER
        readonly KeyCode keyCode;
        readonly float power;
        readonly float duration;
#endif      

        public AltUnityHoldButtonCommand(params string[] parameters) : base(parameters, 5)
        {
#if ALTUNITYTESTER
            keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), parameters[2]);

            this.power = JsonConvert.DeserializeObject<float>(parameters[3]);
            this.duration = JsonConvert.DeserializeObject<float>(parameters[4]);
#endif
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(power);
            Input.SetKeyDown(keyCode, power, duration);
#endif      
            return "Ok";
        }
    }
}
