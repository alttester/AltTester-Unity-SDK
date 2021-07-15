namespace Altom.AltUnityDriver
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
        public override bool Equals(object obj)
        {
            if (!(obj is AltUnityVector2))
                return false;
            var other = (AltUnityVector2)obj;
            return
                other.x == this.x &&
                other.y == this.y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
    public struct AltUnityVector3
    {
        public float x;
        public float y;
        public float z;

        public AltUnityVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public AltUnityVector3(float x, float y) : this(x, y, 0)
        {
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AltUnityVector3))
                return false;
            var other = (AltUnityVector3)obj;
            return
                other.x == this.x &&
                other.y == this.y &&
                other.z == this.z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
