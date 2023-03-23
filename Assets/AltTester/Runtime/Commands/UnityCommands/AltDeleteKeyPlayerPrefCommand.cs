using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
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
