namespace Altom.AltDriver.Commands
{
    public class AltKeyUp : AltBaseCommand
    {
        AltKeyUpParams cmdParams;

        public AltKeyUp(IDriverCommunication commHandler, AltKeyCode keyCode) : base(commHandler)
        {
            this.cmdParams = new AltKeyUpParams(keyCode);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}