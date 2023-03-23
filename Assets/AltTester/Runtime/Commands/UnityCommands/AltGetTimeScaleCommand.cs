using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltGetTimeScaleCommand : AltCommand<AltGetTimeScaleParams, float>
    {
        public AltGetTimeScaleCommand(AltGetTimeScaleParams cmdParams) : base(cmdParams)
        { }
        public override float Execute()
        {
            return UnityEngine.Time.timeScale;
        }
    }
}
