using Newtonsoft.Json;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHoldButtonCommand : AltUnityCommand
    {
        UnityEngine.KeyCode keyCode;
        float power;
        float duration;

        public AltUnityHoldButtonCommand(params string[] parameters) : base(parameters, 5)
        {
            this.keyCode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), parameters[2]);
            this.power = JsonConvert.DeserializeObject<float>(parameters[3]);
            this.duration = JsonConvert.DeserializeObject<float>(parameters[4]);
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            LogMessage("pressKeyboardKey: " + keyCode);
            var powerClamped = UnityEngine.Mathf.Clamp01(power);
            Input.SetKeyDown(keyCode, power, duration);
#endif      
            return "Ok";
        }
    }
}
