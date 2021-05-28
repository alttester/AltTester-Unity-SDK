namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTapCustom : AltBaseCommand
    {
        private AltUnityTapCustomParams cmdParams;

        public AltUnityTapCustom(IDriverCommunication commHandler, float x, float y, int count, float interval) : base(commHandler)
        {
            cmdParams = new AltUnityTapCustomParams(x, y, count, interval);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);
        }
    }
}