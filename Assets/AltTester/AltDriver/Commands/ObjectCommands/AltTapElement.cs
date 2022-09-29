namespace Altom.AltDriver.Commands
{
    public class AltTapElement : AltCommandReturningAltElement
    {
        AltTapElementParams cmdParams;

        public AltTapElement(IDriverCommunication commHandler, AltObject altUnityObject, int count, float interval, bool wait) : base(commHandler)
        {
            cmdParams = new AltTapElementParams(
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