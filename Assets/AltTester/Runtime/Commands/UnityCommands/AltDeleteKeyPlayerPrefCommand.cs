using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltDeleteKeyPlayerPrefCommand : AltCommand<AltDeleteKeyPlayerPrefParams, string>
    {
        public AltDeleteKeyPlayerPrefCommand(AltDeleteKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            UnityEngine.PlayerPrefs.DeleteKey(CommandParams.keyName);
            return "Ok";
        }
    }
}
