using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityActionFinishedCommand : AltUnityCommand<AltUnityActionFinishedParams, string>
    {
        public AltUnityActionFinishedCommand(AltUnityActionFinishedParams cmdParams) : base(cmdParams) { }
        public override string Execute()
        {
#if ALTUNITYTESTER
            string response;
            if (Input.Finished)
                response = "Yes";
            else
            {
                response = "No";
            }
            return response;
#endif
            throw new NotFoundException();
        }
    }
}
