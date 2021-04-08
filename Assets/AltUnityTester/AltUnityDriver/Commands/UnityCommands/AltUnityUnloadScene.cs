namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityUnloadScene : AltBaseCommand
    {
        readonly string sceneName;
        public AltUnityUnloadScene(SocketSettings socketSettings, string sceneName) : base(socketSettings)
        {
            this.sceneName = sceneName;
        }
        public void Execute()
        {
            SendCommand("unloadScene", sceneName);

            var data = Recvall();
            ValidateResponse("Ok", data);

            data = Recvall();
            ValidateResponse("Scene Unloaded", data);
        }
    }
}