using System;

namespace Altom.AltUnityDriver.Commands
{

    public class AltUnityMultipointSwipe : AltBaseCommand
    {
        AltUnityMultipointSwipeChainParams cmdParams;

        public AltUnityMultipointSwipe(IDriverCommunication commHandler, AltUnityVector2[] positions, float duration) : base(commHandler)
        {
            cmdParams = new AltUnityMultipointSwipeChainParams(positions, duration);
        }

        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);
        }
    }
}
