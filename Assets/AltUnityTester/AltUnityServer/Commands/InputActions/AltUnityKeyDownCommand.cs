using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityKeyDownCommand : AltUnityCommand<AltUnityKeyDownParams, string>
    {
        public AltUnityKeyDownCommand(AltUnityKeyDownParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {
            var powerClamped = Mathf.Clamp(CommandParams.power, -1, 1);
            InputController.KeyDown((UnityEngine.KeyCode)CommandParams.keyCode, powerClamped);
            return "Ok";
        }
    }
}
