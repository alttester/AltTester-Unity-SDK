using System;
using Altom.Server.Logging;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Assets.AltUnityTester.AltUnityServer.Communication
{
    public class AltServerWebSocketHandler : WebSocketBehavior
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private readonly ICommandHandler _commandHandler;

        public CommunicationErrorHandler OnErrorHandler;
        public CommunicationHandler OnClientConnected;
        public CommunicationHandler OnClientDisconnected;

        public AltServerWebSocketHandler(ICommandHandler cmdHandler)
        {
            this._commandHandler = cmdHandler;
            _commandHandler.OnSendMessage += this.Send;
        }

        ~AltServerWebSocketHandler()
        {
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            logger.Debug("Client " + this.ID + " connected.");
            if (OnClientConnected != null) OnClientConnected.Invoke();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            logger.Debug("Client " + this.ID + " disconnected.");
            if (OnClientDisconnected != null) OnClientDisconnected.Invoke();
            _commandHandler.OnSendMessage -= this.Send;
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            if (OnErrorHandler != null)
                OnErrorHandler.Invoke(e.Message, e.Exception);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            _commandHandler.OnMessage(e.Data);
        }
    }
}