/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetScreenshotPNGCommand : AltBaseScreenshotCommand<AltGetPNGScreenshotParams, string>
    {
        public AltGetScreenshotPNGCommand(ICommandHandler handler, AltGetPNGScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            AltRunner._altRunner.StartCoroutine(SendPNGScreenshotCoroutine());
            return "Ok";
        }
    }
}
