

namespace Assets.AltUnityTester.AltUnityDriver.UnityStruct
{
    public struct AltUnityColor
    {
        public float r;
        public float g;        
        public float b;
        public float a;
        public AltUnityColor(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1;

        }
        public AltUnityColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;

        }
    }
}
