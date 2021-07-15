using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityKeyDownCommand : AltUnityCommand
    {
#if ALTUNITYTESTER
        readonly KeyCode keyCode;
        readonly float power;
#endif      

        public AltUnityKeyDownCommand(params string[] parameters) : base(parameters, 4)
        {
#if ALTUNITYTESTER
            keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), parameters[2]);

            this.power = JsonConvert.DeserializeObject<float>(parameters[3]);
#endif
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(power);
            Input.KeyDown(keyCode, power);
            return "Ok";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
