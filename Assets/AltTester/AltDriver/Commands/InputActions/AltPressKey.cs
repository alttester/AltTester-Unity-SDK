namespace Altom.AltDriver.Commands
{
    public class AltPressKey : AltBaseCommand
    {
        AltPressKeyboardKeyParams cmdParams;
        public AltPressKey(IDriverCommunication commHandler, AltKeyCode keyCode, float power, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltPressKeyboardKeyParams(keyCode, power, duration, wait);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            if (cmdParams.wait)
            {
                data = CommHandler.Recvall<string>(cmdParams);
                ValidateResponse("Finished", data);
            }
        }
    }
}