using System.Collections.Generic;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllElementsLight : AltUnityBaseFindObjects
    {
        AltUnityFindObjectsLightParams cmdParams;
        public AltUnityGetAllElementsLight(IDriverCommunication commHandler, By cameraBy, string cameraPath, bool enabled) : base(commHandler)
        {
            cmdParams = new AltUnityFindObjectsLightParams("//*", cameraBy, SetPath(cameraBy, cameraPath), enabled);
        }
        public List<AltUnityObjectLight> Execute()
        {
            CommHandler.Send(cmdParams);

            return CommHandler.Recvall<List<AltUnityObjectLight>>(cmdParams).data;
        }
    }
}