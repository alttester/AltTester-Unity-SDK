namespace Altom.AltUnityDriver
{
    public struct AltUnityTextureInformation
    {
        public byte[] imageData;
        public AltUnityVector2 scaleDifference;
        public AltUnityVector3 textureSize;

        public AltUnityTextureInformation(byte[] imageData, AltUnityVector2 scaleDifference, AltUnityVector3 textureSize)
        {
            this.imageData = imageData;
            this.scaleDifference = scaleDifference;
            this.textureSize = textureSize;
        }
    }
}