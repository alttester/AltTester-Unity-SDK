namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTapScreen : AltUnityCommandReturningAltElement
    {
        AltUnityTapScreenParams cmdParams;
        public AltUnityTapScreen(IDriverCommunication commHandler, float x, float y) : base(commHandler)
        {
            cmdParams = new AltUnityTapScreenParams(x, y);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);

            try
            {
                return ReceiveAltUnityObject(cmdParams);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }
    }
}