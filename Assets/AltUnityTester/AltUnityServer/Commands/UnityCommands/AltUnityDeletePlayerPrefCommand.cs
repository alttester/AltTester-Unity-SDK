using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDeletePlayerPrefCommand : AltUnityCommand<AltUnityDeletePlayerPrefParams, string>
    {
        public AltUnityDeletePlayerPrefCommand(AltUnityDeletePlayerPrefParams cmdParams) : base(cmdParams)
        { }

        public override string Execute()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
            return "Ok";
        }
    }
}
