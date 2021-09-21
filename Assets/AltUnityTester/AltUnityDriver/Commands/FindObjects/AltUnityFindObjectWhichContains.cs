namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObjectWhichContains : AltUnityBaseFindObjects
    {
        AltUnityFindObjectParams cmdParams;

        public AltUnityFindObjectWhichContains(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(commHandler)
        {
            cmdParams = new AltUnityFindObjectParams(SetPathContains(by, value), cameraBy, SetPath(cameraBy, cameraPath), enabled);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltUnityObject(cmdParams);
        }
    }
}