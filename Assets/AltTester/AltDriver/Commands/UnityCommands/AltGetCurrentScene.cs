using Newtonsoft.Json;

namespace AltTester.AltDriver.Commands
{
    public class AltGetCurrentScene : AltBaseFindObjects
    {
        private readonly AltGetCurrentSceneParams cmdParams;
        public AltGetCurrentScene(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltGetCurrentSceneParams();
        }
        public string Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveAltObject(cmdParams).name;
        }
    }
}