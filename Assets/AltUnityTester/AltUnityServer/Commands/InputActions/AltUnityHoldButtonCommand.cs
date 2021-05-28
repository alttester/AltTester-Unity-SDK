using UnityEngine;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityHoldButtonCommand : AltUnityCommand<AltUnityPressKeyboardKeyParams, string>
    {

        public AltUnityHoldButtonCommand(AltUnityPressKeyboardKeyParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            Input.SetKeyDown((UnityEngine.KeyCode)CommandParams.keyCode, CommandParams.power, CommandParams.duration);
#endif      
            return "Ok";
        }
    }
}
