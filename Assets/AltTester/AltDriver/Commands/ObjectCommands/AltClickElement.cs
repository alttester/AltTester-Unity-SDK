namespace Altom.AltDriver.Commands
{
    public class AltClickElement : AltCommandReturningAltElement
    {
        AltClickElementParams cmdParams;

        public AltClickElement(IDriverCommunication commHandler, AltObject altUnityObject, int count, float interval, bool wait) : base(commHandler)
        {
            cmdParams = new AltClickElementParams(
            altUnityObject,
             count,
             interval,
             wait);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            var element = ReceiveAltObject(cmdParams);

            if (cmdParams.wait)
            {
                var data = CommHandler.Recvall<string>(cmdParams);
                ValidateResponse("Finished", data);
            }
            return element;
        }
    }
}