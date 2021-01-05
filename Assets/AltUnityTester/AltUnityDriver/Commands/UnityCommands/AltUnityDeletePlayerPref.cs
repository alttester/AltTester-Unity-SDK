namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityDeletePlayerPref : AltBaseCommand
    {
        public AltUnityDeletePlayerPref(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public void Execute()
        {
            SendCommand("deletePlayerPref");
            var data = Recvall();
            if (data.Equals("Ok"))
                return;
            HandleErrors(data);
        }
    }
}