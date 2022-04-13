namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKeys : AltBaseCommand
    {
        AltUnityPressKeyboardKeysParams cmdParams;
        public AltUnityPressKeys(IDriverCommunication commHandler, AltUnityKeyCode[] keyCodes, float power, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityPressKeyboardKeysParams(keyCodes, power, duration, wait);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            if (cmdParams.wait)
            {
                foreach(AltUnityKeyCode key in cmdParams.keyCodes){
                    data = CommHandler.Recvall<string>(cmdParams);
                    ValidateResponse("Finished", data);
                }
            }
        }
    }
}