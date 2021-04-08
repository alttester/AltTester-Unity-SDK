namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObjects : AltUnityBaseFindObjects
    {
        readonly By by;
        readonly string value;
        readonly By cameraBy;
        string cameraPath;
        readonly bool enabled;

        public AltUnityFindObjects(SocketSettings socketSettings, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
        {
            this.by = by;
            this.value = value;
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
        public System.Collections.Generic.List<AltUnityObject> Execute()
        {
            string path = SetPath(by, value);
            cameraPath = SetPath(cameraBy, cameraPath);
            SendCommand("findObjects", path, cameraBy.ToString(), cameraPath, enabled.ToString());
            return ReceiveListOfAltUnityObjects();
        }
    }
}