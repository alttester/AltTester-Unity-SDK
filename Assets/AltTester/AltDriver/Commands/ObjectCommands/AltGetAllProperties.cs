using System.Collections.Generic;

namespace Altom.AltDriver.Commands
{
    public class AltGetAllProperties : AltBaseCommand
    {
        AltGetAllPropertiesParams cmdParams;

        public AltGetAllProperties(IDriverCommunication commHandler, AltComponent altUnityComponent, AltObject altUnityObject, AltPropertiesSelections altUnityPropertiesSelections = AltPropertiesSelections.ALLPROPERTIES) : base(commHandler)
        {
            cmdParams = new AltGetAllPropertiesParams(altUnityObject.id, altUnityComponent, altUnityPropertiesSelections);

        }
        public List<AltProperty> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<AltProperty>>(cmdParams);
        }
    }
}