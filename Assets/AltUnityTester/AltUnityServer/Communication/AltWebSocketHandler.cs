using Altom.Server.Logging;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Assets.AltUnityTester.AltUnityServer.Communication
{
    public class AltClientWebSocketHandler
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private readonly WebSocket _webSocket;
        private readonly CommandHandler _commandHandler;

        public AltClientWebSocketHandler(WebSocket webSocket)
        {
            this._webSocket = webSocket;
            webSocket.OnMessage += this.OnMessage;
            webSocket.OnError += (sender, args) =>
            {
                logger.Error(args.Message);
                if (args.Exception != null)
                    logger.Error(args.Exception);
            };
            this._commandHandler = new CommandHandler(webSocket.Send);
        }
        private void OnMessage(object sender, MessageEventArgs message)
        {
            this._commandHandler.OnMessage(message.Data);
        }
    }

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