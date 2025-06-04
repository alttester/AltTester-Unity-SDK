/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Runtime.CompilerServices;

namespace AltTester.AltTesterSDK.Driver
{
    public struct AltVector2
    {
        public float x;
        public float y;

        private static readonly AltVector2 zeroVector = new(0f, 0f);

        private static readonly AltVector2 oneVector = new(1f, 1f);

        private static readonly AltVector2 upVector = new(0f, 1f);

        private static readonly AltVector2 downVector = new(0f, -1f);

        private static readonly AltVector2 leftVector = new(-1f, 0f);

        private static readonly AltVector2 rightVector = new(1f, 0f);

        //
        // Summary:
        //     Shorthand for writing AltVector2(0, 0).
        public static AltVector2 zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return zeroVector;
            }
        }

        //
        // Summary:
        //     Shorthand for writing AltVector2(1, 1).
        public static AltVector2 one
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return oneVector;
            }
        }

        //
        // Summary:
        //     Shorthand for writing AltVector2(0, 1).
        public static AltVector2 up
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return upVector;
            }
        }

        //
        // Summary:
        //     Shorthand for writing AltVector2(0, -1).
        public static AltVector2 down
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return downVector;
            }
        }

        //
        // Summary:
        //     Shorthand for writing AltVector2(-1, 0).
        public static AltVector2 left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return leftVector;
            }
        }

        //
        // Summary:
        //     Shorthand for writing AltVector2(1, 0).
        public static AltVector2 right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return rightVector;
            }
        }

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
            return $"AltVector2{{ x : {x}, y : {y} }}";
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
            return $"AltVector3{{ x : {x}, y : {y}, z : {z} }}";
        }
    }
}
