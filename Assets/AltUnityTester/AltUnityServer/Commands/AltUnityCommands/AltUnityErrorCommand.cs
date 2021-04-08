using System;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityErrorCommand : AltUnityCommand
    {
        private readonly string message;
        private readonly Exception ex;

        public AltUnityErrorCommand(string message, Exception ex, params string[] parameters) : base(parameters, parameters.Length)
        {
            this.message = message;
            this.ex = ex;
        }
        public override string Execute()
        {
            return message;
        }

        public override string GetLogs()
        {
            return this.ex.Message + "\n" + this.ex.StackTrace;
        }
    }
}
