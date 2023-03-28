using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltKeysDownCommand : AltCommand<AltKeysDownParams, string>
    {
        public AltKeysDownCommand(AltKeysDownParams cmdParams) : base(cmdParams)
        {

        }

        public override string Execute()
        {

#if ALTTESTER
            var powerClamped = Mathf.Clamp(CommandParams.power, -1, 1);
            foreach (var keyCode in CommandParams.keyCodes)
                InputController.KeyDown((UnityEngine.KeyCode)keyCode, powerClamped);
            return "Ok";
#else
            throw new AltInputModuleException(AltErrors.errorInputModule);
#endif
        }
    }
}