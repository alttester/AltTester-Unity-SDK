using System;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltSetKeyPlayerPrefCommand : AltCommand<AltSetKeyPlayerPrefParams, string>
    {
        public AltSetKeyPlayerPrefCommand(AltSetKeyPlayerPrefParams cmdParams) : base(cmdParams)
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