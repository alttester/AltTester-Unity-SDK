using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllFields : AltBaseCommand
    {
        AltUnityGetAllFieldsParams cmdParams;
        public AltUnityGetAllFields(IDriverCommunication commHandler, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityFieldsSelections altUnityFieldsSelections = AltUnityFieldsSelections.ALLFIELDS) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllFieldsParams(altUnityObject.id, altUnityComponent, altUnityFieldsSelections);
        }
        public List<AltUnityProperty> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<AltUnityProperty>>(cmdParams);
        }
    }
}