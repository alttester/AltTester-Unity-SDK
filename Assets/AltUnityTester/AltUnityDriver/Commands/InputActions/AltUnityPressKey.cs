namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKey : AltBaseCommand
    {
        AltUnityPressKeyboardKeyParams cmdParams;
        public AltUnityPressKey(IDriverCommunication commHandler, AltUnityKeyCode keyCode, float power, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityPressKeyboardKeyParams(keyCode, power, duration, wait);
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