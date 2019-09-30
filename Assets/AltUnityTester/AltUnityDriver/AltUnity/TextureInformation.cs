public struct TextureInformation
    {
        public byte[] imageData;
        public UnityEngine.Vector2 scaleDifference;
        public UnityEngine.Vector3 textureSize;
        public UnityEngine.TextureFormat textureFormat;

        public TextureInformation(byte[] imageData, UnityEngine.Vector2 scaleDifference, UnityEngine.Vector3 textureSize, UnityEngine.TextureFormat textureFormat)
        {
            this.imageData = imageData;
            this.scaleDifference = scaleDifference;
            this.textureSize = textureSize;
            this.textureFormat = textureFormat;
        }
    }