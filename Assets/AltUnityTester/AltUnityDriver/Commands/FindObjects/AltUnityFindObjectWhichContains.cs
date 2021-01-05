namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObjectWhichContains : AltUnityBaseFindObjects
    {
        By by;
        string value;
        By cameraBy;
        string cameraPath;
        bool enabled;

        public AltUnityFindObjectWhichContains(SocketSettings socketSettings, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
        {
            this.by = by;
            this.value = value;
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
        public AltUnityObject Execute()
        {
            string path = SetPathContains(by, value);
            cameraPath = SetPath(cameraBy, cameraPath);
            SendCommand("findObject", path, cameraBy.ToString(), cameraPath, enabled.ToString());
            return ReceiveAltUnityObject();
        }
    }
}