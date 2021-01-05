namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetScreenshot : AltUnityCommandReturningAltElement
    {
        int id;
        AltUnityColor color;
        float width;
        AltUnityVector2 size;
        AltUnityVector2 coordinates;
        int screenShotQuality;


        int option = 0;

        public AltUnityGetScreenshot(SocketSettings socketSettings, AltUnityVector2 size, int screenShotQuality) : base(socketSettings)
        {
            this.size = size;
            this.screenShotQuality = screenShotQuality;
            this.option = 1;
        }
        public AltUnityGetScreenshot(SocketSettings socketSettings, int id, AltUnityColor color, float width, AltUnityVector2 size, int screenShotQuality) : base(socketSettings)
        {
            this.size = size;
            this.color = color;
            this.width = width;
            this.id = id;
            this.screenShotQuality = screenShotQuality;
            this.option = 2;
        }
        public AltUnityGetScreenshot(SocketSettings socketSettings, AltUnityVector2 coordinates, AltUnityColor color, float width, AltUnityVector2 size, int screenShotQuality) : base(socketSettings)
        {
            this.coordinates = coordinates;
            this.color = color;
            this.width = width;
            this.size = size;
            this.screenShotQuality = screenShotQuality;
            this.option = 3;
        }
        public AltUnityTextureInformation Execute(out AltUnityObject selectedObject)
        {
            selectedObject = null;
            switch (option)
            {
                case 2:
                    return GetHighlightObjectScreenshot();
                case 3:
                    return GetHighlightObjectFromCoordinatesScreenshot(out selectedObject);
                default:
                    return GetSimpleScreenshot();
            }
        }
        public AltUnityTextureInformation Execute()
        {
            AltUnityObject selectedObject = null;
            switch (option)
            {
                case 2:
                    return GetHighlightObjectScreenshot();
                case 3:
                    return GetHighlightObjectFromCoordinatesScreenshot(out selectedObject);
                default:
                    return GetSimpleScreenshot();
            }
        }

        private AltUnityTextureInformation GetSimpleScreenshot()
        {
            var sizeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(size);
            SendCommand("getScreenshot", sizeSerialized, screenShotQuality.ToString());
            return ReceiveImage();
        }
        private AltUnityTextureInformation GetHighlightObjectScreenshot()
        {
            var sizeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(size);
            var colorAndWidth = color.r + "!!" + color.g + "!!" + color.b + "!!" + color.a + "!-!" + width;
            SendCommand("hightlightObjectScreenshot", id.ToString(), colorAndWidth, sizeSerialized, screenShotQuality.ToString());
            return ReceiveImage();
        }
        private AltUnityTextureInformation GetHighlightObjectFromCoordinatesScreenshot(out AltUnityObject selectedObject)
        {
            var coordinatesSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(coordinates);
            var sizeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(size);
            var colorAndWidth = color.r + "!!" + color.g + "!!" + color.b + "!!" + color.a + "!-!" + width;
            SendCommand("hightlightObjectFromCoordinatesScreenshot", coordinatesSerialized, colorAndWidth, sizeSerialized, screenShotQuality.ToString());
            selectedObject = ReceiveAltUnityObject();
            if (selectedObject.name.Equals("Null") && selectedObject.id == 0)
            {
                selectedObject = null;
            }
            return ReceiveImage();
        }
    }
}