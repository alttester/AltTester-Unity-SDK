namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObjectAtCoordinates : AltUnityCommandReturningAltElement
    {
        AltUnityFindObjectAtCoordinatesParams cmdParams;

        public AltUnityFindObjectAtCoordinates(IDriverCommunication commHandler, AltUnityVector2 coordinates) : base(commHandler)
        {
            cmdParams = new AltUnityFindObjectAtCoordinatesParams(coordinates);
        }

        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}