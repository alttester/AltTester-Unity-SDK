namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityEndTouch : AltBaseCommand
    {
        int fingerId;

        public AltUnityEndTouch(SocketSettings socketSettings, int fingerId) : base(socketSettings)
        {
            this.fingerId = fingerId;
        }
        public void Execute()
        {
            SendCommand("endTouch", fingerId.ToString());
            string data = Recvall();
            ValidateResponse("Ok", data);
        }
    }
}