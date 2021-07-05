using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMultipointSwipeAndWait : AltBaseCommand
    {
        readonly AltUnityVector2[] positions;
        readonly float duration;

        public AltUnityMultipointSwipeAndWait(SocketSettings socketSettings, AltUnityVector2[] positions, float duration) : base(socketSettings)
        {
            this.positions = positions;
            this.duration = duration;
        }

        public void Execute()
        {
            new AltUnityMultipointSwipe(SocketSettings, positions, duration).Execute();
            Thread.Sleep((int)(duration * 1000));
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