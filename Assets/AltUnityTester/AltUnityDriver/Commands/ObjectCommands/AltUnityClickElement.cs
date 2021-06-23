namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityClickElement : AltUnityCommandReturningAltElement
    {
        AltUnityObject altUnityObject;
        private readonly int count;
        private readonly float interval;
        private readonly bool wait;

        public AltUnityClickElement(SocketSettings socketSettings, AltUnityObject altUnityObject, int count, float interval, bool wait) : base(socketSettings)
        {
            this.altUnityObject = altUnityObject;
            this.count = count;
            this.interval = interval;
            this.wait = wait;
        }
        public AltUnityObject Execute()
        {
            string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            SendCommand("clickElement", altObject, count.ToString(), interval.ToString(), this.wait.ToString());

            var element = ReceiveAltUnityObject();
            if (wait)
            {
                var data = this.Recvall();
                ValidateResponse("Finished", data);
            }
            return element;
        }
    }
}