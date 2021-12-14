namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMultipointSwipe : AltBaseCommand
    {
        AltUnityMultipointSwipeParams cmdParams;

        public AltUnityMultipointSwipe(IDriverCommunication commHandler, AltUnityVector2[] positions, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityMultipointSwipeParams(positions, duration, wait);
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
