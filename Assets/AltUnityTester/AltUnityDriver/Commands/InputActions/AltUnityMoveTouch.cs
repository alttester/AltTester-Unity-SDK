namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityMoveTouch : AltBaseCommand
    {
        AltUnityMoveTouchParams cmdParams;

        public AltUnityMoveTouch(IDriverCommunication commHandler, int fingerId, AltUnityVector2 coordinates) : base(commHandler)
        {
            this.cmdParams = new AltUnityMoveTouchParams(fingerId, coordinates);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}