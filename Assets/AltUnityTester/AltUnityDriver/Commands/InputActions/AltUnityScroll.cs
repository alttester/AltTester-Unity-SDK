namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityScroll : AltBaseCommand
    {
        AltUnityScrollParams cmdParams;
        public AltUnityScroll(IDriverCommunication commHandler, float speed, float speedHorizontal, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityScrollParams(speed, duration, wait, speedHorizontal);
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