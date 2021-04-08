namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDeleteKeyPlayerPrefCommand : AltUnityCommand
    {
        readonly string keyName;

        public AltUnityDeleteKeyPlayerPrefCommand(params string[] parameters) : base(parameters, 3)
        {
            this.keyName = parameters[2];
        }

        public override string Execute()
        {
            UnityEngine.PlayerPrefs.DeleteKey(keyName);
            return "Ok";
        }
    }
}
