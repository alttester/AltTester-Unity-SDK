namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetKeyPlayerPrefCommand : AltUnityCommand
    {
        PLayerPrefKeyType type;
        string keyName;
        string value;

        public AltUnitySetKeyPlayerPrefCommand (PLayerPrefKeyType type, string keyName, string value)
        {
            this.type = type;
            this.keyName = keyName;
            this.value = value;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("setKeyPlayerPref for: " + keyName);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
                switch (type)
                {
                    case PLayerPrefKeyType.String:
                    AltUnityRunner._altUnityRunner.LogMessage("Set Option string ");
                        UnityEngine.PlayerPrefs.SetString(keyName, value);
                        break;
                    case PLayerPrefKeyType.Float:
                    AltUnityRunner._altUnityRunner.LogMessage("Set Option Float ");
                        UnityEngine.PlayerPrefs.SetFloat(keyName, float.Parse(value));
                        break;
                    case PLayerPrefKeyType.Int:
                    AltUnityRunner._altUnityRunner.LogMessage("Set Option Int ");
                        UnityEngine.PlayerPrefs.SetInt(keyName, int.Parse(value));
                        break;
                }
                response = "Ok";
            return response;
        }
    }
}
