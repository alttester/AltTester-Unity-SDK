using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

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
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
