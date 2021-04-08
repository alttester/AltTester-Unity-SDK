using Assets.AltUnityTester.AltUnityServer.AltSocket;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetServerVersionCommandBackwardsCompatible
    {
        public AltUnityGetServerVersionCommandBackwardsCompatible() { }

        public string Execute()
        {
            return AltUnityRunner.VERSION;
        }

        public void SendResponse(AltClientSocketHandler handler)
        {
            AltUnityRunner._responseQueue.ScheduleResponse(delegate
            {
                var response = Execute();
                handler.SendResponseBackwardsCompatible(response);
            });
        }
    }
}

