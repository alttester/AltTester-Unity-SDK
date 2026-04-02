/*
    Copyright(C) 2026 Altom Consulting

*/

using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltGetServerVersionCommand : AltCommand<AltGetServerVersionParams, string>
    {
        public AltGetServerVersionCommand(AltGetServerVersionParams cmdParams) : base(cmdParams) { }
        public override string Execute()
        {
            return AltRunner.VERSION.Split("-")[0];
        }
    }
}
