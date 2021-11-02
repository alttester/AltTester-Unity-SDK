using System.Text;
using Altom.AltUnityTester.Logging;

namespace Altom.AltUnityTester.Communication
{
    public class AltWebGLWebSocketHandler
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private readonly WebGLWebSocket _webSocket;
        private readonly ICommandHandler _commandHandler;

        public AltWebGLWebSocketHandler(ICommandHandler cmdHandler, WebGLWebSocket webSocket)
        {
            this._webSocket = webSocket;
            webSocket.OnMessage += this.OnMessage;

            this._commandHandler = cmdHandler;
            _commandHandler.OnSendMessage += (message) =>
             {
                 webSocket.SendText(message).ConfigureAwait(false).GetAwaiter().GetResult();
             };
        }
        private void OnMessage(byte[] data)
        {
            var message = Encoding.UTF8.GetString(data);
            this._commandHandler.OnMessage(message);
        }
    }
}