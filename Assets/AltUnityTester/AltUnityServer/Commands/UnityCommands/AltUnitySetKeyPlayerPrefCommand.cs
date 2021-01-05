using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetKeyPlayerPrefCommand : AltUnityCommand
    {
        PLayerPrefKeyType type;
        string keyName;
        string value;

        public AltUnitySetKeyPlayerPrefCommand(params string[] parameters) : base(parameters, 5)
        {
            this.keyName = parameters[2];
            this.value = parameters[3];
            this.type = (PLayerPrefKeyType)System.Enum.Parse(typeof(PLayerPrefKeyType), parameters[4]);
        }

        public override string Execute()
        {
            LogMessage("setKeyPlayerPref for: " + keyName);
            string response = AltUnityErrors.errorNotFoundMessage;
            switch (type)
            {
                case PLayerPrefKeyType.String:
                    LogMessage("Set Option string ");
                    UnityEngine.PlayerPrefs.SetString(keyName, value);
                    break;
                case PLayerPrefKeyType.Float:
                    LogMessage("Set Option Float ");
                    UnityEngine.PlayerPrefs.SetFloat(keyName, float.Parse(value));
                    break;
                case PLayerPrefKeyType.Int:
                    LogMessage("Set Option Int ");
                    UnityEngine.PlayerPrefs.SetInt(keyName, int.Parse(value));
                    break;
            }
            response = "Ok";
            return response;
        }
    }
}
