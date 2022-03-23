using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityKeyUpCommand : AltUnityCommand<AltUnityKeyUpParams, string>
    {
        public AltUnityKeyUpCommand(AltUnityKeyUpParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            InputController.KeyUp((UnityEngine.KeyCode)CommandParams.keyCode);
            return "Ok";
        }
    }
}
