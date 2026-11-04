/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterUnitySDK.Commands
{
    public static class AltObjectIdExtensions
    {
        /// <summary>
        /// Returns the instance id of an object as the protocol reports it.
        /// </summary>
        /// <remarks>
        /// Unity 6000.4 added <c>Object.GetEntityId</c> and deprecated <c>Object.GetInstanceID</c>;
        /// 6000.5 made that deprecation an error. <c>EntityId</c>'s implicit conversion to
        /// <c>int</c> is deprecated as well, so the value is read through the supported
        /// <c>EntityId.ToULong</c> and narrowed here rather than converted implicitly.
        /// The narrowing keeps the id 32-bit, which is what <c>AltObject</c> carries.
        /// </remarks>
        public static int GetObjectInstanceId(this UnityEngine.Object obj)
        {
#if UNITY_6000_4_OR_NEWER
            return unchecked((int)UnityEngine.EntityId.ToULong(obj.GetEntityId()));
#else
            return obj.GetInstanceID();
#endif
        }
    }
}