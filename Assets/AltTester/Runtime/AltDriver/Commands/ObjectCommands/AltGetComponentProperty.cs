/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltGetComponentProperty<T> : AltBaseCommand
    {
        AltGetObjectComponentPropertyParams cmdParams;
        public AltGetComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, string assemblyName, int maxDepth, AltObject altObject) : base(commHandler)
        {
            cmdParams = new AltGetObjectComponentPropertyParams(altObject, componentName, propertyName, assemblyName, maxDepth);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            T result = CommHandler.Recvall<T>(cmdParams);

            // If the result is an AltObject, ensure its CommHandler is set
            if (result is AltObject altObject)
            {
                altObject.CommHandler = CommHandler;
            }

            return result;
        }
    }
}
