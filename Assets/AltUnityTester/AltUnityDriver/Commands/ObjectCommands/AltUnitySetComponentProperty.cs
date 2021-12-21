namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetComponentProperty : AltBaseCommand
    {
        AltUnitySetObjectComponentPropertyParams cmdParams;

        public AltUnitySetComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, object value, string assemblyName, AltUnityObject altUnityObject) : base(commHandler)
        {
            var strValue = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            cmdParams = new AltUnitySetObjectComponentPropertyParams(altUnityObject, componentName, propertyName, assemblyName, strValue);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);

            ValidateResponse("valueSet", data);
        }
    }
}