using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMultipointSwipeAndWait : AltBaseCommand
    {
        AltUnityMultipointSwipe multipointSwipe;
        AltUnityActionFinishedParams actionFinishedParams;
        private readonly float duration;

        public AltUnityMultipointSwipeAndWait(IDriverCommunication commHandler, AltUnityVector2[] positions, float duration) : base(commHandler)
        {
            this.duration = duration;
            multipointSwipe = new AltUnityMultipointSwipe(commHandler, positions, duration);
            actionFinishedParams = new AltUnityActionFinishedParams();
        }

        public void Execute()
        {
            multipointSwipe.Execute();
            Thread.Sleep((int)(duration * 1000));
            string data;
            do
            {
                CommHandler.Send(actionFinishedParams);
                data = CommHandler.Recvall<string>(actionFinishedParams).data;
            } while (data == "No");
            ValidateResponse("Yes", data);
        }
    }
}