using System;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
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
