/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltGetScreenshotResponse
    {
        public AltVector2 scaleDifference;
        public AltVector3 textureSize;
        public byte[] compressedImage;

    }
    public class AltGetScreenshot : AltCommandReturningAltElement
    {
        AltGetScreenshotParams cmdParams;


        public AltGetScreenshot(IDriverCommunication commHandler, AltVector2 size, int screenShotQuality) : base(commHandler)
        {
            cmdParams = new AltGetScreenshotParams(size, screenShotQuality);
        }
        public virtual AltTextureInformation Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveScreenshot(cmdParams);
        }

        protected AltTextureInformation ReceiveScreenshot(CommandParams commandParams)
        {
            var data = CommHandler.Recvall<string>(commandParams);
            ValidateResponse("Ok", data);

            var imageData = CommHandler.Recvall<AltGetScreenshotResponse>(commandParams);
            return new AltTextureInformation(imageData.compressedImage, imageData.scaleDifference, imageData.textureSize);
        }
    }


    public class AltGetHighlightObjectScreenshot : AltGetScreenshot
    {

        AltHighlightObjectScreenshotParams cmdParams;

        public AltGetHighlightObjectScreenshot(IDriverCommunication commHandler, int id, AltColor color, float width, AltVector2 size, int screenShotQuality) : base(commHandler, size, screenShotQuality)
        {
            cmdParams = new AltHighlightObjectScreenshotParams(id, color, width, size, screenShotQuality);
        }

        public override AltTextureInformation Execute()
        {
            CommHandler.Send(cmdParams);
            return ReceiveScreenshot(cmdParams);
        }
    }


    public class AltGetHighlightObjectFromCoordinatesScreenshot : AltGetScreenshot
    {
        AltHighlightObjectFromCoordinatesScreenshotParams cmdParams;

        public AltGetHighlightObjectFromCoordinatesScreenshot(IDriverCommunication commHandler, AltVector2 coordinates, AltColor color, float width, AltVector2 size, int screenShotQuality) : base(commHandler, size, screenShotQuality)
        {
            cmdParams = new AltHighlightObjectFromCoordinatesScreenshotParams(coordinates, color, width, size, screenShotQuality);
        }
        public AltTextureInformation Execute(out AltObject selectedObject)
        {
            CommHandler.Send(cmdParams);

            selectedObject = ReceiveAltObject(cmdParams);
            if (selectedObject != null && selectedObject.name.Equals("Null") && selectedObject.id == 0)
            {
                selectedObject = null;
            }
            return ReceiveScreenshot(cmdParams);

        }
    }

}
