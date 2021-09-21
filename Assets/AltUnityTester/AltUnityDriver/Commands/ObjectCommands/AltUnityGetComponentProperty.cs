namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetComponentProperty : AltBaseCommand
    {
        AltUnityGetObjectComponentPropertyParams cmdParams;
        public AltUnityGetComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, string assemblyName, int maxDepth, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityGetObjectComponentPropertyParams(altUnityObject, componentName, propertyName, assemblyName, maxDepth);
        }
        public string Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<string>(cmdParams).data;
        }
    }
}