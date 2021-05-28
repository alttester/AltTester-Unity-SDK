namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetComponentProperty : AltBaseCommand
    {
        AltUnitySetObjectComponentPropertyParams cmdParams;

        public AltUnitySetComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, string value, string assemblyName, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnitySetObjectComponentPropertyParams(altUnityObject, componentName, propertyName, assemblyName, value);
        }
        public string Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<string>(cmdParams).data;
        }
    }
}