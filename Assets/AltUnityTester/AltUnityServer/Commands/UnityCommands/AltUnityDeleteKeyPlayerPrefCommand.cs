using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityDeleteKeyPlayerPrefCommand : AltUnityCommand<AltUnityDeleteKeyPlayerPrefParams, string>
    {
        public AltUnityDeleteKeyPlayerPrefCommand(AltUnityDeleteKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            UnityEngine.PlayerPrefs.DeleteKey(CommandParams.keyName);
            return "Ok";
        }
    }
}
