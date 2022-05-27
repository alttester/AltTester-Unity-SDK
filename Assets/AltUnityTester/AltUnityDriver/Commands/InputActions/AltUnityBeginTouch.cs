namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityBeginTouch : AltBaseCommand
    {
        AltUnityBeginTouchParams cmdParams;

        public AltUnityBeginTouch(IDriverCommunication commHandler, AltUnityVector2 coordinates) : base(commHandler)
        {
            this.cmdParams = new AltUnityBeginTouchParams(coordinates);
        }
        public int Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<int>(cmdParams);  //finger id
            //TODO: add handling for "Finished"
        }
    }
}