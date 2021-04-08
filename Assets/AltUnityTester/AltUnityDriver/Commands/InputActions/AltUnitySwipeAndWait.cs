namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySwipeAndWait : AltBaseCommand
    {
        AltUnityVector2 start;
        AltUnityVector2 end;
        readonly float duration;
        public AltUnitySwipeAndWait(SocketSettings socketSettings, AltUnityVector2 start, AltUnityVector2 end, float duration) : base(socketSettings)
        {
            this.start = start;
            this.end = end;
            this.duration = duration;
        }
        public void Execute()
        {
            new AltUnitySwipe(SocketSettings, start, end, duration).Execute();
            System.Threading.Thread.Sleep((int)(duration * 1000));
            string data;
            do
            {
                SendCommand("actionFinished");
                data = Recvall();
            } while (data == "No");
            ValidateResponse("Yes", data);
        }
    }
}