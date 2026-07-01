/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterSDK.Driver
{
    public class AltFindObjectFromObject : AltBaseFindObjects
    {
        AltFindObjectParams cmdParams;


        public AltFindObjectFromObject(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraValue, bool enabled, AltObject obj) : base(commHandler)
        {
            cameraValue = SetPath(cameraBy, cameraValue);
            string path = SetPathFromObject(obj, by, value);
            cmdParams = new AltFindObjectParams(path, cameraBy, cameraValue, enabled);
        }
        public AltObject Execute()
        {
            CommHandler.Send(cmdParams);
            var altTesterObject = CommHandler.Recvall<AltObject>(cmdParams);
            altTesterObject.CommHandler = CommHandler;
            return altTesterObject;
        }
    }
}
