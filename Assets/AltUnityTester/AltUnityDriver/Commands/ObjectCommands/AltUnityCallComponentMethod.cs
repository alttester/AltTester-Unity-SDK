namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCallComponentMethod<T> : AltBaseCommand
    {
        AltUnityCallComponentMethodForObjectParams cmdParams;

        public AltUnityCallComponentMethod(IDriverCommunication commHandler, string componentName, string methodName, string[] parameters, string[] typeOfParameters, string assembly, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityCallComponentMethodForObjectParams(altUnityObject, componentName, methodName, parameters, typeOfParameters, assembly);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            T data = CommHandler.Recvall<T>(cmdParams).data;
            return data;
        }
    }
}