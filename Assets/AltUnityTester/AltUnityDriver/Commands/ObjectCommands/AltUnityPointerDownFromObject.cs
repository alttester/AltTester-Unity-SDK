namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityPointerDownFromObject : AltUnityCommandReturningAltElement
    {
        AltUnityObject altUnityObject;
        public AltUnityPointerDownFromObject(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.altUnityObject = altUnityObject;
        }
        public AltUnityObject Execute()
        {
            string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            SendCommand("pointerDownFromObject", altObject);
            return ReceiveAltUnityObject();
        }
    }
}