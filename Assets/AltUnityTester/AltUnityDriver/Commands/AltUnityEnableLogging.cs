namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityEnableLogging : AltBaseCommand
    {
        public AltUnityEnableLogging(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public void Execute()
        {
            SendCommand("enableLogging", SocketSettings.LogFlag.ToString());
            var data = Recvall();
            if (data.Equals("Ok"))
                return;
            HandleErrors(data);
        }
    }
}