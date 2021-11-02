using WebSocketSharp;

namespace Altom.AltUnityTester.Communication
{
    public class AltClientWebSocketHandler
    {
        private readonly WebSocket _webSocket;
        private readonly ICommandHandler _commandHandler;

        public AltClientWebSocketHandler(WebSocket webSocket, ICommandHandler commandHandler)
        {
            this._webSocket = webSocket;
            webSocket.OnMessage += this.OnMessage;

            this._commandHandler = commandHandler;
            this._commandHandler.OnSendMessage += webSocket.Send;
        }
        private void OnMessage(object sender, MessageEventArgs message)
        {
            this._commandHandler.OnMessage(message.Data);
        }
    }
}