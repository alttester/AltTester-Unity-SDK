using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetKeyPlayerPrefCommand : AltUnityCommand
    {
        PLayerPrefKeyType type;
        string value;

        public AltUnityGetKeyPlayerPrefCommand(params string[] parameters) : base(parameters, 4)
        {
            this.value = parameters[2];
            this.type = (PLayerPrefKeyType)System.Enum.Parse(typeof(PLayerPrefKeyType), parameters[3]);
        }

        public override string Execute()
        {
            LogMessage("getKeyPlayerPref for: " + value);
            string response = AltUnityErrors.errorNotFoundMessage;
            if (UnityEngine.PlayerPrefs.HasKey(value))
            {
                switch (type)
                {
                    case PLayerPrefKeyType.String:
                        LogMessage("Option string " + UnityEngine.PlayerPrefs.GetString(value));
                        response = UnityEngine.PlayerPrefs.GetString(value);
                        break;
                    case PLayerPrefKeyType.Float:
                        LogMessage("Option Float " + UnityEngine.PlayerPrefs.GetFloat(value));
                        response = UnityEngine.PlayerPrefs.GetFloat(value) + "";
                        break;
                    case PLayerPrefKeyType.Int:
                        LogMessage("Option Int " + UnityEngine.PlayerPrefs.GetInt(value));
                        response = UnityEngine.PlayerPrefs.GetInt(value) + "";
                        break;
                }
            }
            return response;
        }
    }
}
