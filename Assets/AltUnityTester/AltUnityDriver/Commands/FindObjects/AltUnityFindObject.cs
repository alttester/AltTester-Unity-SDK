namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityFindObject : AltUnityBaseFindObjects
    {
        By by;
        string value;
        By cameraBy;
        string cameraPath;
        bool enabled;

        public AltUnityFindObject(SocketSettings socketSettings, By by, string value, By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
        {
            this.by = by;
            this.value = value;
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
        public AltUnityObject Execute()
        {
            cameraPath = SetPath(cameraBy, cameraPath);
            if (enabled && by == By.NAME)
            {
                SendCommand("findActiveObjectByName", value, cameraBy.ToString(), cameraPath, enabled.ToString());
            }
            else
            {
                string path = SetPath(by, value);
                SendCommand("findObject", path, cameraBy.ToString(), cameraPath, enabled.ToString());
            }
            return ReceiveAltUnityObject();
        }
    }
}