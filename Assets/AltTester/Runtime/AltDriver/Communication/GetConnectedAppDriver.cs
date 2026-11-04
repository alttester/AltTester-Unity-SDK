/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Communication
{
    public class GetConnectedAppDriver : BaseDriver
    {
        public GetConnectedAppDriver(string path) : base(path)
        {
        }
        public void Send()
        {
            WsClient.Send("GetApps");
        }
    }
}
