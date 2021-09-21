using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMoveMouseAndWait : AltBaseCommand
    {
        AltUnityMoveMouse moveMouse;
        AltUnityActionFinishedParams actionFinishedParams;
        private readonly float duration;
        public AltUnityMoveMouseAndWait(IDriverCommunication commHandler, AltUnityVector2 location, float duration) : base(commHandler)
        {
            this.duration = duration;
            moveMouse = new AltUnityMoveMouse(commHandler, location, duration);
            actionFinishedParams = new AltUnityActionFinishedParams();
        }
        public void Execute()
        {
            moveMouse.Execute();

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