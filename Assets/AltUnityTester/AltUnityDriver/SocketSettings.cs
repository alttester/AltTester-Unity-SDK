using System;
using Altom.AltUnityDriver.AltSocket;

namespace Altom.AltUnityDriver
{
    public class SocketSettings
    {
        public ISocket Socket { get; private set; }
        public string RequestSeparator { get; private set; }
        public string RequestEnding { get; private set; }

        [Obsolete]
        public bool LogFlag { get; private set; }

        [Obsolete]
        public SocketSettings(ISocket socket, string requestSeparator, string requestEnding, bool logFlag)
        {
            this.Socket = socket;
            this.RequestSeparator = requestSeparator;
            this.RequestEnding = requestEnding;
            this.LogFlag = logFlag;
        }

        public SocketSettings(ISocket socket, string requestSeparator, string requestEnding)
        {
            this.Socket = socket;
            this.RequestSeparator = requestSeparator;
            this.RequestEnding = requestEnding;
        }
    }
}