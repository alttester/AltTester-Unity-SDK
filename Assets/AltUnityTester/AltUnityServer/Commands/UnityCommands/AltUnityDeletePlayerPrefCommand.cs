
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDeletePlayerPrefCommand : AltUnityCommand
    {
        public AltUnityDeletePlayerPrefCommand(params string[] parameters) : base(parameters, 2)
        { }
        public override string Execute()
        {
            LogMessage("deletePlayerPref");
            string response = AltUnityErrors.errorNotFoundMessage;
            UnityEngine.PlayerPrefs.DeleteAll();
            response = "Ok";
            return response;
        }
    }
}
