using System;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

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
