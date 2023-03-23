namespace AltTester.AltTesterUnitySdk.Communication
{
    public class BaseWebSocketHandler
    {
        protected readonly ICommandHandler _commandHandler;

        public BaseWebSocketHandler(ICommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }
    }
}
