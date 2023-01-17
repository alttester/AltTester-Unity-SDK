namespace AltTester.AltDriver
{
    public struct AltVector2
    {
        public float x;
        public float y;

        public AltVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is AltVector2))
                return false;
            var other = (AltVector2)obj;
            return
                other.x == this.x &&
                other.y == this.y;
        }
        public static AltVector2 operator +(AltVector2 v1, AltVector2 v2)
        {
            return new AltVector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static AltVector2 operator *(AltVector2 v1, AltVector2 v2)
        {
            return new AltVector2(v1.x * v2.x, v1.y * v2.y);
        }
        public static AltVector2 operator *(AltVector2 v1, float number)
        {
            return new AltVector2(v1.x * number, v1.y * number);
        }
        public static AltVector2 operator /(AltVector2 v1, AltVector2 v2)
        {
            return new AltVector2(v1.x / v2.x, v1.y / v2.y);
        }
        public static AltVector2 operator /(AltVector2 v1, float number)
        {
            return new AltVector2(v1.x / number, v1.y / number);
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
    public struct AltVector3
    {
        public float x;
        public float y;
        public float z;

        public AltVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public AltVector3(float x, float y) : this(x, y, 0)
        {
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AltVector3))
                return false;
            var other = (AltVector3)obj;
            return
                other.x == this.x &&
                other.y == this.y &&
                other.z == this.z;
        }

        public static AltVector3 operator +(AltVector3 v1, AltVector3 v2)
        {
            return new AltVector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static AltVector3 operator *(AltVector3 v1, AltVector3 v2)
        {
            return new AltVector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
        public static AltVector3 operator *(AltVector3 v1, float number)
        {
            return new AltVector3(v1.x * number, v1.y * number, v1.z * number);
        }
        public static AltVector3 operator /(AltVector3 v1, AltVector3 v2)
        {
            return new AltVector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }
        public static AltVector3 operator /(AltVector3 v1, float number)
        {
            return new AltVector3(v1.x / number, v1.y / number, v1.z / number);
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
