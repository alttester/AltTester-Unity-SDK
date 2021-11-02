using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetTimeScaleCommand : AltUnityCommand<AltUnityGetTimeScaleParams, float>
    {
        public AltUnityGetTimeScaleCommand(AltUnityGetTimeScaleParams cmdParams) : base(cmdParams)
        { }
        public override float Execute()
        {
            return UnityEngine.Time.timeScale;
        }
    }
}
