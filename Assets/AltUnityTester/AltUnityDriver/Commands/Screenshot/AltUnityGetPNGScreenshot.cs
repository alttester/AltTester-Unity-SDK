namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetPNGScreenshot : AltBaseCommand
    {
        string path;
        AltUnityGetPNGScreenshotParams cmdParams;
        public AltUnityGetPNGScreenshot(IDriverCommunication commHandler, string path) : base(commHandler)
        {
            this.path = path;
            this.cmdParams = new AltUnityGetPNGScreenshotParams();
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var message = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", message);
            string screenshotData = CommHandler.Recvall<string>(cmdParams);
            System.IO.File.WriteAllBytes(path, System.Convert.FromBase64String(screenshotData));
        }
    }
}