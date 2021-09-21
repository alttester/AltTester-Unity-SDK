using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityPressKeyboardKeyCommand : AltUnityCommand<AltUnityPressKeyboardKeyParams, string>
    {

        public AltUnityPressKeyboardKeyCommand(AltUnityPressKeyboardKeyParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            Input.KeyPress((UnityEngine.KeyCode)CommandParams.keyCode, CommandParams.power, CommandParams.duration);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
