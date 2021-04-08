using System;
using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetKeyPlayerPrefCommand : AltUnityCommand
    {
        readonly PLayerPrefKeyType type;
        readonly string keyName;
        readonly string value;

        public AltUnitySetKeyPlayerPrefCommand(params string[] parameters) : base(parameters, 5)
        {
            this.keyName = parameters[2];
            this.value = parameters[3];
            type = (PLayerPrefKeyType)Enum.Parse(typeof(PLayerPrefKeyType), parameters[4]);
        }

        public override string Execute()
        {
            switch (type)
            {
                case PLayerPrefKeyType.String:
                    UnityEngine.PlayerPrefs.SetString(keyName, value);
                    break;
                case PLayerPrefKeyType.Float:
                    UnityEngine.PlayerPrefs.SetFloat(keyName, float.Parse(value));
                    break;
                case PLayerPrefKeyType.Int:
                    UnityEngine.PlayerPrefs.SetInt(keyName, int.Parse(value));
                    break;
            }
            return "Ok";
        }
    }
}
