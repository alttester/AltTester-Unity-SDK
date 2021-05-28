namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySwipe : AltBaseCommand
    {
        AltUnityMultipointSwipeParams cmdParams;
        public AltUnitySwipe(IDriverCommunication commHandler, AltUnityVector2 start, AltUnityVector2 end, float duration) : base(commHandler)
        {
            cmdParams = new AltUnityMultipointSwipeParams(start, end, duration);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);
        }
    }
}