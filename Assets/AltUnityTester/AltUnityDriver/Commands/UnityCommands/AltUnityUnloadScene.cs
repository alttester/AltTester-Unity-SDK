namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityUnloadScene : AltBaseCommand
    {
        AltUnityUnloadSceneParams cmdParams;
        public AltUnityUnloadScene(IDriverCommunication commHandler, string sceneName) : base(commHandler)
        {
            cmdParams = new AltUnityUnloadSceneParams(sceneName);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);

            var data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);

            data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Scene Unloaded", data);
        }
    }
}