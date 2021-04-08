using System;

namespace Altom.AltUnityDriver.Commands
{
    [Obsolete]
    public class AltUnityEnableLogging : AltBaseCommand
    {
        public AltUnityEnableLogging(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public void Execute()
        {
            SendCommand("enableLogging", SocketSettings.LogFlag.ToString());
            var data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}