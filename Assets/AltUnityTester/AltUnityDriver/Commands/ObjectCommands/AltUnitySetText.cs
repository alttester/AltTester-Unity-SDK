namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetText : AltUnityCommandReturningAltElement
    {
        AltUnityObject altUnityObject;
        string newText;

        public AltUnitySetText(SocketSettings socketSettings, AltUnityObject altUnityObject, string text) : base(socketSettings)
        {
            this.altUnityObject = altUnityObject;
            this.newText = text;
        }

        public AltUnityObject Execute()
        {
            var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            SendCommand("setText", altObject, newText);
            return ReceiveAltUnityObject();
        }
    }
}