namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetPNGScreenshot : AltBaseCommand
    {
        string path;
        public AltUnityGetPNGScreenshot(SocketSettings socketSettings, string path) : base(socketSettings)
        {
            this.path = path;
        }
        public void Execute()
        {
            SendCommand("getPNGScreenshot");
            var message = Recvall();
            string screenshotData = "";
            if (message == "Ok")
            {
                screenshotData = Recvall();
            }
            System.IO.File.WriteAllBytes(path, System.Convert.FromBase64String(screenshotData));
        }
    }
}