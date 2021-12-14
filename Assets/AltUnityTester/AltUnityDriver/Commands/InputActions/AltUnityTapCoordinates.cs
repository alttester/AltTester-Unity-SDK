namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTapCoordinates : AltBaseCommand
    {
        AltUnityTapCoordinatesParams cmdParams;
        public AltUnityTapCoordinates(IDriverCommunication commHandler, AltUnityVector2 coordinates, int count, float interval, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityTapCoordinatesParams(coordinates, count, interval, wait);
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