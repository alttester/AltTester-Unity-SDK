using Altom.AltUnityDriver;

namespace Altom.AltUnityTester
{
    public static class AltUnityVectorExtensions
    {
        public static UnityEngine.Vector2 ToUnity(this AltUnityVector2 vector2)
        {
            return new UnityEngine.Vector2(vector2.x, vector2.y);
        }

        public static UnityEngine.Vector3 ToUnity(this AltUnityVector3 vector3)
        {
            return new UnityEngine.Vector3(vector3.x, vector3.y, vector3.z);
        }
    }

}