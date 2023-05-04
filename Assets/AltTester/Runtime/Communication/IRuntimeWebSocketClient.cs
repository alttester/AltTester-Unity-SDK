using System;

namespace AltTester.AltTesterUnitySDK.Communication
{
    public delegate void CommunicationHandler();
    public delegate void CommunicationErrorHandler(string message, Exception error);
    public delegate void CommunicationMessageHandler(string message);

    public interface IRuntimeWebSocketClient
    {
        CommunicationHandler OnConnect { get; set; }
        CommunicationHandler OnDisconnect { get; set; }
        CommunicationErrorHandler OnError { get; set; }
        CommunicationMessageHandler OnMessage { get; set; }
        bool IsConnected { get; }
        void Connect();
        void Close();
        void Send(string message);
        void Send(byte[] message);
    }

    public class RuntimeWebSocketClientException : Exception
    {
        public RuntimeWebSocketClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
