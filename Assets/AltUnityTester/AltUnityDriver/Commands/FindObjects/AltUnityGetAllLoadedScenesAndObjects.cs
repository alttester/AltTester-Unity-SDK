using System.Collections.Generic;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllLoadedScenesAndObjects : AltUnityBaseFindObjects
    {

        private AltUnityGetAllLoadedScenesAndObjectsParams cmdParams;
        public AltUnityGetAllLoadedScenesAndObjects(IDriverCommunication commHandler, bool enabled) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllLoadedScenesAndObjectsParams("//*", By.NAME, "", enabled);
        }
        public List<AltUnityObjectLight> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<AltUnityObjectLight>>(cmdParams);

        }
    }
}