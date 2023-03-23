using AltTester.AltDriver.Commands;

namespace Altom.AltDriver.Commands
{
    public class AltSetStaticProperty : AltBaseCommand
    {
        AltSetObjectComponentPropertyParams cmdParams;
        public AltSetStaticProperty(IDriverCommunication commHandler, string componentName, string propertyName, string assemblyName, object newValue) : base(commHandler)
        {
            var strValue = Newtonsoft.Json.JsonConvert.SerializeObject(newValue);
            cmdParams = new AltSetObjectComponentPropertyParams(null, componentName, propertyName, assemblyName, strValue);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("valueSet", data);
        }
    }
}