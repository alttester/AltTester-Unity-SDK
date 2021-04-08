using System;
using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetKeyPlayerPrefCommand : AltUnityCommand
    {
        readonly PLayerPrefKeyType type;
        readonly string value;

        public AltUnityGetKeyPlayerPrefCommand(params string[] parameters) : base(parameters, 4)
        {
            this.value = parameters[2];
            type = (PLayerPrefKeyType)Enum.Parse(typeof(PLayerPrefKeyType), parameters[3]);
        }

        public override string Execute()
        {
            string response = AltUnityErrors.errorNotFoundMessage;
            if (UnityEngine.PlayerPrefs.HasKey(value))
            {
                switch (type)
                {
                    case PLayerPrefKeyType.String:
                        response = UnityEngine.PlayerPrefs.GetString(value);
                        break;
                    case PLayerPrefKeyType.Float:
                        response = UnityEngine.PlayerPrefs.GetFloat(value) + "";
                        break;
                    case PLayerPrefKeyType.Int:
                        response = UnityEngine.PlayerPrefs.GetInt(value) + "";
                        break;
                }
            }
            return response;
        }
    }
}
