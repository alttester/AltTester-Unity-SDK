namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityClickElement : AltUnityCommandReturningAltElement
    {
        AltUnityClickElementParams cmdParams;

        public AltUnityClickElement(IDriverCommunication commHandler, AltUnityObject altUnityObject, int count, float interval, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityClickElementParams(
            altUnityObject,
             count,
             interval,
             wait);
        }
        public AltUnityObject Execute()
        {
            CommHandler.Send(cmdParams);
            var element = ReceiveAltUnityObject(cmdParams);

            if (cmdParams.wait)
            {
                var data = CommHandler.Recvall<string>(cmdParams);
                ValidateResponse("Finished", data);
            }
            return element;
        }
    }
}