
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public struct TextureInformation
    {
        public byte[] imageData;
        public AltUnityVector2 scaleDifference;
        public AltUnityVector3 textureSize;
        public Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityTextureFormat textureFormat;

        public TextureInformation(byte[] imageData, AltUnityVector2 scaleDifference,AltUnityVector3 textureSize, Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityTextureFormat textureFormat)
        {
            this.imageData = imageData;
            this.scaleDifference = scaleDifference;
            this.textureSize = textureSize;
            this.textureFormat = textureFormat;
        }
    }