using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTiltAndWait : AltBaseCommand
    {
        AltUnityTilt tiltCommand;
        AltUnityActionFinishedParams actionFinishedParams;
        readonly float duration;
        public AltUnityTiltAndWait(IDriverCommunication commHandler, AltUnityVector3 acceleration, float duration) : base(commHandler)
        {
            this.duration = duration;
            tiltCommand = new AltUnityTilt(commHandler, acceleration, duration);
            actionFinishedParams = new AltUnityActionFinishedParams();

        }
        public void Execute()
        {
            tiltCommand.Execute();
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