namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDeleteKeyPlayerPrefCommand : AltUnityCommand
    {
        string keyName;

        public AltUnityDeleteKeyPlayerPrefCommand(params string[] parameters) : base(parameters, 3)
        {
            this.keyName = parameters[2];
        }

        public override string Execute()
        {
            LogMessage("deleteKeyPlayerPref for: " + keyName);
            string response = AltUnityErrors.errorNotFoundMessage;
            UnityEngine.PlayerPrefs.DeleteKey(keyName);
            response = "Ok";
            return response;
        }
    }
}
