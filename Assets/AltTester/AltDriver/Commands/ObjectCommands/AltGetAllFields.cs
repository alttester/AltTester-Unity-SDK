using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltDriver.Commands
{
    public class AltGetAllFields : AltBaseCommand
    {
        AltGetAllFieldsParams cmdParams;
        public AltGetAllFields(IDriverCommunication commHandler, AltComponent altUnityComponent, AltObject altUnityObject, AltFieldsSelections altUnityFieldsSelections = AltFieldsSelections.ALLFIELDS) : base(commHandler)
        {
            cmdParams = new AltGetAllFieldsParams(altUnityObject.id, altUnityComponent, altUnityFieldsSelections);
        }
        public List<AltProperty> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<AltProperty>>(cmdParams);
        }
    }
}