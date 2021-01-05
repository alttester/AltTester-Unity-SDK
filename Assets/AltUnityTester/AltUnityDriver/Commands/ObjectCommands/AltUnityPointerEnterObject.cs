namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPointerEnterObject : AltUnityCommandReturningAltElement
    {
        AltUnityObject altUnityObject;
        public AltUnityPointerEnterObject(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.altUnityObject = altUnityObject;
        }
        public AltUnityObject Execute()
        {
            string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            SendCommand("pointerEnterObject", altObject);
            return ReceiveAltUnityObject();
        }
    }
}