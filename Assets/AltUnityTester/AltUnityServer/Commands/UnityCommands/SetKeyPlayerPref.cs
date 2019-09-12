namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class SetKeyPlayerPref: Command
    {
        PLayerPrefKeyType type;
        string keyName;
        string value;

        public SetKeyPlayerPref(PLayerPrefKeyType type, string keyName, string value)
        {
            this.type = type;
            this.keyName = keyName;
            this.value = value;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("setKeyPlayerPref for: " + keyName);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
                switch (type)
                {
                    case PLayerPrefKeyType.String:
                        UnityEngine.Debug.Log("Set Option string ");
                        UnityEngine.PlayerPrefs.SetString(keyName, value);
                        break;
                    case PLayerPrefKeyType.Float:
                        UnityEngine.Debug.Log("Set Option Float ");
                        UnityEngine.PlayerPrefs.SetFloat(keyName, float.Parse(value));
                        break;
                    case PLayerPrefKeyType.Int:
                        UnityEngine.Debug.Log("Set Option Int ");
                        UnityEngine.PlayerPrefs.SetInt(keyName, int.Parse(value));
                        break;
                }
                response = "Ok";
            return response;
        }
    }
}
