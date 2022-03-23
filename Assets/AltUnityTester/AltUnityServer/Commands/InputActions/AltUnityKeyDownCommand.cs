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
            var powerClamped = Mathf.Clamp01(CommandParams.power);
            InputController.KeyDown((UnityEngine.KeyCode)CommandParams.keyCode, powerClamped);
            return "Ok";
        }
    }
}
