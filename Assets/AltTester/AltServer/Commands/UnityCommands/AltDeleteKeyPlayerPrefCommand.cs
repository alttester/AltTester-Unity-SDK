using AltTester.AltDriver.Commands;

namespace AltTester.Commands
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
