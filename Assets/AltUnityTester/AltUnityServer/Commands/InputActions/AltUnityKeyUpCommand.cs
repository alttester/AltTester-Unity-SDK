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

#if ALTUNITYTESTER
            Input.KeyUp((UnityEngine.KeyCode)CommandParams.keyCode);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
