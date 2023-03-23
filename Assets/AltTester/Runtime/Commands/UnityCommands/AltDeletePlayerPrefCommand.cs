using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltDeletePlayerPrefCommand : AltCommand<AltDeletePlayerPrefParams, string>
    {
        public AltDeletePlayerPrefCommand(AltDeletePlayerPrefParams cmdParams) : base(cmdParams)
        { }

        public override string Execute()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
            return "Ok";
        }
    }
}
