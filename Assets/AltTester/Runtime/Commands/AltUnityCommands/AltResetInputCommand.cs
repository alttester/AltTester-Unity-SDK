/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltResetInputCommand : AltCommand<AltResetInputParams, string>
    {
        public AltResetInputCommand(AltResetInputParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            InputController.ResetInput();
            return "Ok";
        }
    }
}
