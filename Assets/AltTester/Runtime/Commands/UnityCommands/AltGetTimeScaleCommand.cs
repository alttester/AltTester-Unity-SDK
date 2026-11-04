/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
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
