using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityGetServerVersionCommand : AltUnityCommand<AltUnityGetServerVersionParams, string>
    {
        public AltUnityGetServerVersionCommand(AltUnityGetServerVersionParams cmdParams) : base(cmdParams) { }
        public override string Execute()
        {
            return AltUnityRunner.VERSION;
        }
    }
}
