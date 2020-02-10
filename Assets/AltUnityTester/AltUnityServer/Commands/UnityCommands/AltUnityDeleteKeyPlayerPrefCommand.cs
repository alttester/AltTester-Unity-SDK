namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDeleteKeyPlayerPrefCommand :  AltUnityCommand
    {
        string keyName;

        public AltUnityDeleteKeyPlayerPrefCommand(string keyName)
        {
            this.keyName = keyName;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("deleteKeyPlayerPref for: " + keyName);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.PlayerPrefs.DeleteKey(keyName);
            response = "Ok";
            return response;
        }
    }
}
