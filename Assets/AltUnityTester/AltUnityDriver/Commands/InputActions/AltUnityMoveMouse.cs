namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMoveMouse : AltBaseCommand
    {
        AltUnityMoveMouseParams cmdParams;
        public AltUnityMoveMouse(IDriverCommunication commHandler, AltUnityVector2 location, float duration) : base(commHandler)
        {
            cmdParams = new AltUnityMoveMouseParams(location, duration);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);
        }
    }
}