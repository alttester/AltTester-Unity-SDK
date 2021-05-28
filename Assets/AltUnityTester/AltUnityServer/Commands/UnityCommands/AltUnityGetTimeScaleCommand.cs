using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
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
