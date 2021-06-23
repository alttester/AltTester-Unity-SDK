using System;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPressKey : AltBaseCommand
    {
        AltUnityPressKeyboardKeyParams cmdParams;
        public AltUnityPressKey(IDriverCommunication commHandler, AltUnityKeyCode keyCode, float power, float duration) : base(commHandler)
        {
            cmdParams = new AltUnityPressKeyboardKeyParams(keyCode, power, duration);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);
        }
    }
}