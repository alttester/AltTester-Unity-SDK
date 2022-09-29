using Altom.AltDriver;
using Altom.AltDriver.Commands;
using UnityEngine;

namespace Altom.AltTester.Commands
{
    class AltKeyDownCommand : AltCommand<AltKeyDownParams, string>
    {
        public AltKeyDownCommand(AltKeyDownParams cmdParams) : base(cmdParams)
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
