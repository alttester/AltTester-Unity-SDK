using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltGetServerVersionCommand : AltCommand<AltGetServerVersionParams, string>
    {
        public AltGetServerVersionCommand(AltGetServerVersionParams cmdParams) : base(cmdParams) { }
        public override string Execute()
        {
            return AltRunner.VERSION;
        }
    }
}
