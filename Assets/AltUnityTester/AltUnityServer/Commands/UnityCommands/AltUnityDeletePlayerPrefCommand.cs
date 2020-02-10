
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDeletePlayerPrefCommand :  AltUnityCommand
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
