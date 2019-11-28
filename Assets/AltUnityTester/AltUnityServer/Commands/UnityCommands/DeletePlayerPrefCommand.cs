
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class DeletePlayerPrefCommand :  Command
    {

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("deletePlayerPref");
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.PlayerPrefs.DeleteAll();
            response = "Ok";
            return response;
        }
    }
}
