using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCommandReturningAltElement : AltBaseCommand
    {
        public AltUnityCommandReturningAltElement(SocketSettings socketSettings) : base(socketSettings)
        {
        }

        protected AltUnityObject ReceiveAltUnityObject()
        {
            string data = Recvall();

            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
            altElement.socketSettings = SocketSettings;
            return altElement;
        }
        protected List<AltUnityObject> ReceiveListOfAltUnityObjects()
        {
            string data = Recvall();

            var altElements = JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
            foreach (var altElement in altElements)
            {
                altElement.socketSettings = SocketSettings;
            }

            return altElements;
        }
    }
}