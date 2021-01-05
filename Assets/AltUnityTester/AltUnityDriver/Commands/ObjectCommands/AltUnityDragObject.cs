namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityDragObject : AltUnityCommandReturningAltElement
    {
        AltUnityVector2 position;
        AltUnityObject altUnityObject;
        public AltUnityDragObject(SocketSettings socketSettings, AltUnityVector2 position, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.position = position;
            this.altUnityObject = altUnityObject;
        }
        public AltUnityObject Execute()
        {
            var positionJson = PositionToJson(position);
            var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);

            SendCommand("dragObject", positionJson, altObject);
            return ReceiveAltUnityObject();
        }
    }
}