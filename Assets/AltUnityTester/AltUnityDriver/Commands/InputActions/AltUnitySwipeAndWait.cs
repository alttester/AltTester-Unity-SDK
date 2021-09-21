using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySwipeAndWait : AltBaseCommand
    {
        AltUnityActionFinishedParams actionFinishedParams;
        AltUnitySwipe multipointSwipe;
        readonly float duration;
        public AltUnitySwipeAndWait(IDriverCommunication commHandler, AltUnityVector2 start, AltUnityVector2 end, float duration) : base(commHandler)
        {
            this.duration = duration;
            multipointSwipe = new AltUnitySwipe(commHandler, start, end, duration);
            actionFinishedParams = new AltUnityActionFinishedParams();
        }
        public void Execute()
        {
            multipointSwipe.Execute();
            Thread.Sleep((int)duration * 1000);
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