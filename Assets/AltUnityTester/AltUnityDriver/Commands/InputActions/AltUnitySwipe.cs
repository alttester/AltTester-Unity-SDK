namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySwipe : AltBaseCommand
    {
        AltUnitySwipeParams cmdParams;
        public AltUnitySwipe(IDriverCommunication commHandler, AltUnityVector2 start, AltUnityVector2 end, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnitySwipeParams(start, end, duration, wait);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            if (cmdParams.wait)
            {
                data = CommHandler.Recvall<string>(cmdParams);
                ValidateResponse("Finished", data);
            }
        }
    }
}