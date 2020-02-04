namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetKeyPlayerPrefCommand :  AltUnityCommand
    {
        PLayerPrefKeyType type;
        string value;

        public AltUnityGetKeyPlayerPrefCommand(PLayerPrefKeyType type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getKeyPlayerPref for: " + value);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            if (UnityEngine.PlayerPrefs.HasKey(value))
            {
                switch (type)
                {
                    case PLayerPrefKeyType.String:
                        AltUnityRunner._altUnityRunner.LogMessage("Option string " + UnityEngine.PlayerPrefs.GetString(value));
                        response = UnityEngine.PlayerPrefs.GetString(value);
                        break;
                    case PLayerPrefKeyType.Float:
                        AltUnityRunner._altUnityRunner.LogMessage("Option Float " + UnityEngine.PlayerPrefs.GetFloat(value));
                        response = UnityEngine.PlayerPrefs.GetFloat(value) + "";
                        break;
                    case PLayerPrefKeyType.Int:
                        AltUnityRunner._altUnityRunner.LogMessage("Option Int " + UnityEngine.PlayerPrefs.GetInt(value));
                        response = UnityEngine.PlayerPrefs.GetInt(value) + "";
                        break;
                }
            }
            return response;
        }
    }
}
