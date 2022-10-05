using Altom.AltDriver;
using Altom.AltDriver.Commands;
using UnityEngine;

namespace Altom.AltTester.Commands
{
    class AltKeyUpCommand : AltCommand<AltKeyUpParams, string>
    {
        public AltKeyUpCommand(AltKeyUpParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            InputController.KeyUp((UnityEngine.KeyCode)CommandParams.keyCode);
            return "Ok";
        }
    }
}
