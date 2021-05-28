using System.Threading;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKeyAndWait : AltBaseCommand
    {
        AltUnityActionFinishedParams actionFinishedParams;
        AltUnityPressKey pressKey;
        readonly float duration;
        public AltUnityPressKeyAndWait(IDriverCommunication commHandler, AltUnityKeyCode keyCode, float power, float duration) : base(commHandler)
        {
            this.duration = duration;
            pressKey = new AltUnityPressKey(commHandler, keyCode, power, duration);
            actionFinishedParams = new AltUnityActionFinishedParams();
        }
        public void Execute()
        {
            pressKey.Execute();
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