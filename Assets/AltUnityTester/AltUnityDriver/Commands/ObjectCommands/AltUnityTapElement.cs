namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTapElement : AltUnityCommandReturningAltElement
    {
        private readonly AltUnityObject altUnityObject;
        private readonly int count;
        private readonly float interval;
        private readonly bool wait;

        public AltUnityTapElement(SocketSettings socketSettings, AltUnityObject altUnityObject, int count, float interval, bool wait) : base(socketSettings)
        {
            this.altUnityObject = altUnityObject;
            this.count = count;
            this.interval = interval;
            this.wait = wait;
        }

        public AltUnityObject Execute()
        {
            var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            SendCommand("tapElement", altObject, count.ToString(), interval.ToString(), wait.ToString());
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