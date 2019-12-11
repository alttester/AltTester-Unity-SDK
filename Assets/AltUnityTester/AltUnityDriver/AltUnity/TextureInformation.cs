
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public struct TextureInformation
    {
        public byte[] imageData;
        public Vector2 scaleDifference;
        public Vector3 textureSize;
        public Assets.AltUnityTester.AltUnityDriver.UnityStruct.TextureFormat textureFormat;

        public TextureInformation(byte[] imageData, Vector2 scaleDifference,Vector3 textureSize, Assets.AltUnityTester.AltUnityDriver.UnityStruct.TextureFormat textureFormat)
        {
            this.imageData = imageData;
            this.scaleDifference = scaleDifference;
            this.textureSize = textureSize;
            this.textureFormat = textureFormat;
        }
    }