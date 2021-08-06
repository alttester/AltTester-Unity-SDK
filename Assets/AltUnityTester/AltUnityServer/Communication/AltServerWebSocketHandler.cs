namespace Assets.AltUnityTester.AltUnityServer.Communication
{
    public class AltServerWebSocketHandler : WebSocketBehavior
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private readonly CommandHandler _commandHandler;

        public AltServerWebSocketHandler()
        {
            logger.Debug("Client connected.");
            _commandHandler = new CommandHandler(this.Send);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            _commandHandler.OnMessage(e.Data);
        }
    }
}