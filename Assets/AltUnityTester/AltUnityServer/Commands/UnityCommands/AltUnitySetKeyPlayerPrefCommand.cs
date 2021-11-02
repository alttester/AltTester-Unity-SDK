using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnitySetKeyPlayerPrefCommand : AltUnityCommand<AltUnitySetKeyPlayerPrefParams, string>
    {
        public AltUnitySetKeyPlayerPrefCommand(AltUnitySetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            switch (CommandParams.keyType)
            {
                case PlayerPrefKeyType.String:
                    UnityEngine.PlayerPrefs.SetString(CommandParams.keyName, CommandParams.stringValue);
                    break;
                case PlayerPrefKeyType.Float:
                    UnityEngine.PlayerPrefs.SetFloat(CommandParams.keyName, CommandParams.floatValue);
                    break;
                case PlayerPrefKeyType.Int:
                    UnityEngine.PlayerPrefs.SetInt(CommandParams.keyName, CommandParams.intValue);
                    break;
            }
            return "Ok";
        }
    }
}
