/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltSetTimeScaleCommand : AltCommand<AltSetTimeScaleParams, string>
    {
        public AltSetTimeScaleCommand(AltSetTimeScaleParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            UnityEngine.Time.timeScale = CommandParams.timeScale;
            return "Ok";
        }
    }
}
