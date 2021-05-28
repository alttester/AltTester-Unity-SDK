using System;

namespace Assets.AltUnityTester.AltUnityServer.Communication
{
    public interface ICommunication
    {
        bool IsConnected { get; }
        /// <summary>
        /// Returns weather the server is listening
        /// TODO: Find a solution when communication is in cliend mode, connecting to a proxy
        /// </summary>
        /// <value></value>
        bool IsListening { get; }
        void Start();
        void Stop();
    }

    public interface ICommandHandler
    {
        void Send(string data);

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