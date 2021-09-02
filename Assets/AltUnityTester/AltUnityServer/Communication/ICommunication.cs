using System;

namespace Assets.AltUnityTester.AltUnityServer.Communication
{
    public delegate void SendMessageHandler(string message);
    public delegate void CommunicationHandler();
    public delegate void CommunicationErrorHandler(string message, Exception error);
    public interface ICommunication
    {
        CommunicationHandler OnConnect { get; set; }
        CommunicationHandler OnDisconnect { get; set; }
        CommunicationErrorHandler OnError { get; set; }
        bool IsConnected { get; }
        /// <summary>
        /// Returns weather the server is listening
        /// TODO: Find a solution when communication is in client mode, connecting to a proxy.
        /// </summary>
        /// <value></value>
        bool IsListening { get; }
        void Start();
        void Stop();
    }

    public interface ICommandHandler
    {
        SendMessageHandler OnSendMessage { get; set; }
        void Send(string data);
        void OnMessage(string data);
    }

    public class AddressInUseCommError : Exception
    {
        public AddressInUseCommError(string message) : base(message)
        {

        }
    }

    public class UnhandledStartCommError : Exception
    {
        public UnhandledStartCommError(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}