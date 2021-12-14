namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMoveMouse : AltBaseCommand
    {
        AltUnityMoveMouseParams cmdParams;
        public AltUnityMoveMouse(IDriverCommunication commHandler, AltUnityVector2 coordinates, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityMoveMouseParams(coordinates, duration, wait);
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