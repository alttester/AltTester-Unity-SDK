namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class DeleteKeyPlayerPrefCommand :  Command
    {
        string keyName;

        public DeleteKeyPlayerPrefCommand(string keyName)
        {
            this.keyName = keyName;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("deleteKeyPlayerPref for: " + keyName);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.PlayerPrefs.DeleteKey(keyName);
            response = "Ok";
            return response;
        }
    }
}
