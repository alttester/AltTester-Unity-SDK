using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltKeysUpCommand : AltCommand<AltKeysUpParams, string>
    {
        public AltKeysUpCommand(AltKeysUpParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {

#if ALTTESTER
            foreach (var keyCode in CommandParams.keyCodes)
                InputController.KeyUp((UnityEngine.KeyCode)keyCode);
            return "Ok";
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }
    }
}
