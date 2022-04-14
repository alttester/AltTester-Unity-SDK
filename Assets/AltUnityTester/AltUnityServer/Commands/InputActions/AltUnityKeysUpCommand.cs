using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using UnityEngine;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityKeysUpCommand : AltUnityCommand<AltUnityKeysUpParams, string>
    {
        public AltUnityKeysUpCommand(AltUnityKeysUpParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {

#if ALTUNITYTESTER
            foreach (var keyCode in CommandParams.keyCodes)
                InputController.KeyUp((UnityEngine.KeyCode)keyCode);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
