
namespace Assets.AltUnityTester.AltUnityDriver.UnityStruct
{
    public struct AltUnityVector2
    {
        public float x;
        public float y;

        public AltUnityVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public struct AltUnityVector3
    {
        public float x;
        public float y;
        public float z;

        public AltUnityVector3(float x, float y,float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public AltUnityVector3(float x, float y) : this(x, y, 0)
        {
        }
    }
}
