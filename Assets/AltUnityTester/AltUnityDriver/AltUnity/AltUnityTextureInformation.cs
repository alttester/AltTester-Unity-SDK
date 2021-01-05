namespace Altom.AltUnityDriver
{
    public struct AltUnityTextureInformation
    {
        public byte[] imageData;
        public AltUnityVector2 scaleDifference;
        public AltUnityVector3 textureSize;
        public AltUnityTextureFormat textureFormat;

        public AltUnityTextureInformation(byte[] imageData, AltUnityVector2 scaleDifference, AltUnityVector3 textureSize, AltUnityTextureFormat textureFormat)
        {
            this.imageData = imageData;
            this.scaleDifference = scaleDifference;
            this.textureSize = textureSize;
            this.textureFormat = textureFormat;
        }
    }
}