namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityDropObject : AltUnityCommandReturningAltElement
    {
        AltUnityVector2 position;
        AltUnityObject altUnityObject;
        public AltUnityDropObject(SocketSettings socketSettings, AltUnityVector2 position, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.position = position;
            this.altUnityObject = altUnityObject;
        }
        public AltUnityObject Execute()
        {
            var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            var positionString = PositionToJson(position);

            SendCommand("dropObject", positionString, altObject);
            return ReceiveAltUnityObject();
        }
    }
}