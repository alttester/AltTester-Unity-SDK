namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetScreenshotResponse
    {
        public AltUnityVector2 scaleDifference;
        public AltUnityVector3 textureSize;
        public byte[] compressedImage;

    }
    public class AltUnityGetScreenshot : AltBaseCommand
    {
        AltUnityGetScreenshotParams cmdParams;

        public AltUnityGetScreenshot(IDriverCommunication commHandler, AltUnityVector2 size, int screenShotQuality) : base(commHandler)
        {
            cmdParams = new AltUnityGetScreenshotParams(size, screenShotQuality);
        }
        public AltUnityTextureInformation Execute()
        {
            CommHandler.Send(cmdParams);

            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            var imageData = CommHandler.Recvall<AltUnityGetScreenshotResponse>(cmdParams);
            byte[] decompressedImage = DecompressScreenshot(imageData.compressedImage);
            return new AltUnityTextureInformation(decompressedImage, imageData.scaleDifference, imageData.textureSize);
        }
    }


    public class AltUnityGetHightlightObjectScreenshot : AltBaseCommand
    {

        AltUnityHightlightObjectScreenshotParams cmdParams;

        public AltUnityGetHightlightObjectScreenshot(IDriverCommunication commHandler, int id, AltUnityColor color, float width, AltUnityVector2 size, int screenShotQuality) : base(commHandler)
        {
            cmdParams = new AltUnityHightlightObjectScreenshotParams(id, color, width, size, screenShotQuality);
        }

        public AltUnityTextureInformation Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            var imageData = CommHandler.Recvall<AltUnityGetScreenshotResponse>(cmdParams);
            byte[] decompressedImage = DecompressScreenshot(imageData.compressedImage);
            return new AltUnityTextureInformation(decompressedImage, imageData.scaleDifference, imageData.textureSize);
        }
    }


    public class AltUnityGetHightlightObjectFromCoordinatesScreenshot : AltUnityCommandReturningAltElement
    {
        AltUnityHightlightObjectFromCoordinatesScreenshotParams cmdParams;


        public AltUnityGetHightlightObjectFromCoordinatesScreenshot(IDriverCommunication commHandler, AltUnityVector2 coordinates, AltUnityColor color, float width, AltUnityVector2 size, int screenShotQuality) : base(commHandler)
        {
            cmdParams = new AltUnityHightlightObjectFromCoordinatesScreenshotParams(coordinates, color, width, size, screenShotQuality);
        }
        public AltUnityTextureInformation Execute(out AltUnityObject selectedObject)
        {
            CommHandler.Send(cmdParams);
            selectedObject = ReceiveAltUnityObject(cmdParams);
            if (selectedObject != null && selectedObject.name.Equals("Null") && selectedObject.id == 0)
            {
                selectedObject = null;
            }

            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);

            var imageData = CommHandler.Recvall<AltUnityGetScreenshotResponse>(cmdParams);
            byte[] decompressedImage = DecompressScreenshot(imageData.compressedImage);
            return new AltUnityTextureInformation(decompressedImage, imageData.scaleDifference, imageData.textureSize);

        }
    }

}