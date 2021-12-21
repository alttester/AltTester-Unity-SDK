namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetComponentProperty<T> : AltBaseCommand
    {
        AltUnityGetObjectComponentPropertyParams cmdParams;
        public AltUnityGetComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, string assemblyName, int maxDepth, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityGetObjectComponentPropertyParams(altUnityObject, componentName, propertyName, assemblyName, maxDepth);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<T>(cmdParams);
        }
    }
}