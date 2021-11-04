using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetStringKeyPlayerPrefCommand : AltUnityCommand<AltUnityGetKeyPlayerPrefParams, string>
    {
        public AltUnityGetStringKeyPlayerPrefCommand(AltUnityGetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            if (UnityEngine.PlayerPrefs.HasKey(CommandParams.keyName))
            {
                return UnityEngine.PlayerPrefs.GetString(CommandParams.keyName);
            }
            throw new NotFoundException(string.Format("PlayerPrefs key {0} not found", CommandParams.keyName));
        }
    }

    class AltUnityGetFloatKeyPlayerPrefCommand : AltUnityCommand<AltUnityGetKeyPlayerPrefParams, float>
    {
        public AltUnityGetFloatKeyPlayerPrefCommand(AltUnityGetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override float Execute()
        {
            if (UnityEngine.PlayerPrefs.HasKey(CommandParams.keyName))
            {
                return UnityEngine.PlayerPrefs.GetFloat(CommandParams.keyName);
            }
            throw new NotFoundException(string.Format("PlayerPrefs key {0} not found", CommandParams.keyName));
        }
    }

    class AltUnityGetIntKeyPlayerPrefCommand : AltUnityCommand<AltUnityGetKeyPlayerPrefParams, int>
    {
        public AltUnityGetIntKeyPlayerPrefCommand(AltUnityGetKeyPlayerPrefParams cmdParams) : base(cmdParams)
        {
        }

        public override int Execute()
        {
            if (UnityEngine.PlayerPrefs.HasKey(CommandParams.keyName))
            {
                return UnityEngine.PlayerPrefs.GetInt(CommandParams.keyName);
            }
            throw new NotFoundException(string.Format("PlayerPrefs key {0} not found", CommandParams.keyName));
        }
    }
}
