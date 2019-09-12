
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class DeletePlayerPref :  Command
    {

        public override string Execute()
        {
            UnityEngine.Debug.Log("deletePlayerPref");
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.PlayerPrefs.DeleteAll();
            response = "Ok";
            return response;
        }
    }
}
