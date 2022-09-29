namespace Altom.AltDriver.Commands
{
    public class AltSetComponentProperty : AltBaseCommand
    {
        AltSetObjectComponentPropertyParams cmdParams;

        public AltSetComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, object value, string assemblyName, AltObject altUnityObject) : base(commHandler)
        {
            var strValue = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            cmdParams = new AltSetObjectComponentPropertyParams(altUnityObject, componentName, propertyName, assemblyName, strValue);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);

            ValidateResponse("valueSet", data);
        }
    }
}