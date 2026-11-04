/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltInvalidCommand : AltCommand<CommandParams, string>
    {
        private readonly Exception ex;

        public AltInvalidCommand(CommandParams cmdParams, Exception ex) : base(cmdParams ?? new CommandParams(AltErrors.errorInvalidCommand, null))
        {
            this.ex = ex;
        }

        public override string Execute()
        {
            throw new InvalidCommandException(ex);
        }
    }
}
