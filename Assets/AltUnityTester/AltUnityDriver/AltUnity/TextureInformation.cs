
public struct TextureInformation
    {
        public byte[] imageData;
        public System.Numerics.Vector2 scaleDifference;
        public System.Numerics.Vector3 textureSize;
        public Assets.AltUnityTester.AltUnityDriver.UnityStruct.TextureFormat textureFormat;

        public TextureInformation(byte[] imageData, System.Numerics.Vector2 scaleDifference, System.Numerics.Vector3 textureSize, Assets.AltUnityTester.AltUnityDriver.UnityStruct.TextureFormat textureFormat)
        {
            this.imageData = imageData;
            this.scaleDifference = scaleDifference;
            this.textureSize = textureSize;
            this.textureFormat = textureFormat;
        }
    }