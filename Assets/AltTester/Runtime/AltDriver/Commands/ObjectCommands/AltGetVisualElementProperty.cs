/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltGetVisualElementProperty<T> : AltBaseCommand
    {
        AltGetVisualElementPropertyParams cmdParams;
        public AltGetVisualElementProperty(IDriverCommunication commHandler, string propertyName, AltObject altObject) : base(commHandler)
        {
            cmdParams = new AltGetVisualElementPropertyParams(altObject, propertyName);
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
