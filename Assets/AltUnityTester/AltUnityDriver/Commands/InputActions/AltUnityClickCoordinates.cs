namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityClickCoordinates : AltBaseCommand
    {
        AltUnityClickCoordinatesParams cmdParams;
        public AltUnityClickCoordinates(IDriverCommunication commHandler, AltUnityVector2 coordinates, int count, float interval, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityClickCoordinatesParams(coordinates, count, interval, wait);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            string data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
            if (cmdParams.wait)
            {
                data = CommHandler.Recvall<string>(cmdParams); ;
                ValidateResponse("Finished", data);
            }
        }
    }
}