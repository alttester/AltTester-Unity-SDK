using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCommandReturningAltElement : AltBaseCommand
    {
        public AltUnityCommandReturningAltElement(IDriverCommunication commHandler) : base(commHandler)
        {
        }

        protected AltUnityObject ReceiveAltUnityObject(CommandParams cmdParams)
        {
            var altElement = CommHandler.Recvall<AltUnityObject>(cmdParams);
            if (altElement != null) altElement.CommHandler = CommHandler;

            return altElement;
        }
        protected List<AltUnityObject> ReceiveListOfAltUnityObjects(CommandParams cmdParams)
        {
            var altElements = CommHandler.Recvall<List<AltUnityObject>>(cmdParams);

            foreach (var altElement in altElements)
            {
                altElement.CommHandler = CommHandler;
            }

            return altElements;
        }
    }
}