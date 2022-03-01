using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityInvalidCommand : AltUnityCommand<CommandParams, string>
    {
        private readonly Exception ex;


        public AltUnityInvalidCommand(CommandParams cmdParams, Exception ex) : base(cmdParams ?? new CommandParams(AltUnityErrors.errorInvalidCommand, null))
        {
            this.ex = ex;
        }
        public override string Execute()
        {
            throw new InvalidCommandException(ex);
        }
    }
}
