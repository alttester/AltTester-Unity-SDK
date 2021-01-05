namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObjectsWhichContain : AltUnityBaseFindObjects
    {
        By by;
        string value;
        By cameraBy;
        string cameraPath;
        bool enabled;

        public AltUnityFindObjectsWhichContain(SocketSettings socketSettings, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
        {
            this.by = by;
            this.value = value;
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            string path = SetPathContains(by, value);
            cameraPath = SetPath(cameraBy, cameraPath);
            SendCommand("findObjects", path, cameraBy.ToString(), cameraPath, enabled.ToString());
            return ReceiveListOfAltUnityObjects();
        }
    }
}