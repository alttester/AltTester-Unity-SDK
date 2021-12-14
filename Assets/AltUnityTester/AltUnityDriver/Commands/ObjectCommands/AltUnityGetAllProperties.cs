using System.Collections.Generic;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllProperties : AltBaseCommand
    {
        AltUnityGetAllPropertiesParams cmdParams;

        public AltUnityGetAllProperties(IDriverCommunication commHandler, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityPropertiesSelections altUnityPropertiesSelections = AltUnityPropertiesSelections.ALLPROPERTIES) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllPropertiesParams(altUnityObject.id, altUnityComponent, altUnityPropertiesSelections);

        }
        public List<AltUnityProperty> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<AltUnityProperty>>(cmdParams);
        }
    }
}