namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllElements : AltUnityBaseFindObjects
    {
        By cameraBy;
        string cameraPath;
        bool enabled;
        public AltUnityGetAllElements(SocketSettings socketSettings, By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
        {
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            cameraPath = SetPath(cameraBy, cameraPath);
            SendCommand("findObjects", "//*", cameraBy.ToString(), cameraPath, enabled.ToString());
            return ReceiveListOfAltUnityObjects();
        }
    }
}