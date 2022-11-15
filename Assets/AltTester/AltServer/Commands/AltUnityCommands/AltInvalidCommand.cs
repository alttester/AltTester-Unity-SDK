using System;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;

namespace AltTester.Commands
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
