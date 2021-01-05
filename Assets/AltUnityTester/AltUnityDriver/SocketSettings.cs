using Altom.AltUnityDriver.AltSocket;

namespace Altom.AltUnityDriver
{
    public class SocketSettings
    {
        public ISocket Socket { get; private set; }
        public string RequestSeparator { get; private set; }
        public string RequestEnding { get; private set; }
        public bool LogFlag { get; private set; }

        public SocketSettings(ISocket socket, string requestSeparator, string requestEnding, bool logFlag)
        {
            this.Socket = socket;
            this.RequestSeparator = requestSeparator;
            this.RequestEnding = requestEnding;
            this.LogFlag = logFlag;
        }
    }
}