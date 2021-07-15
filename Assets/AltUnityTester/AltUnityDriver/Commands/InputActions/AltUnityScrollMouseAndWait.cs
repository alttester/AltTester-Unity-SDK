using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityScrollMouseAndWait : AltBaseCommand
    {
        AltUnityActionFinishedParams actionFinishedParams;
        AltUnityScrollMouse scrollMouse;
        readonly float duration;
        public AltUnityScrollMouseAndWait(IDriverCommunication commHandler, float speed, float duration) : base(commHandler)
        {
            this.duration = duration;
            scrollMouse = new AltUnityScrollMouse(commHandler, speed, duration);
            actionFinishedParams = new AltUnityActionFinishedParams();
        }
        public void Execute()
        {
            scrollMouse.Execute();
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