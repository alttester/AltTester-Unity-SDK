/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltGetScreenshotCommand : AltBaseScreenshotCommand<AltGetScreenshotParams, string>
    {
        public AltGetScreenshotCommand(ICommandHandler handler, AltGetScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            AltRunner._altRunner.StartCoroutine(SendTexturedScreenshotCoroutine(new UnityEngine.Vector2(CommandParams.size.x, CommandParams.size.y), CommandParams.quality));
            return "Ok";
        }
    }
}
