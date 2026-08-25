/*
    Copyright(C) 2026 Altom Consulting

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
        public static int GetAltInstanceId(this UnityEngine.Object obj)
        {
#if UNITY_6000_4_OR_NEWER
            return unchecked((int)UnityEngine.EntityId.ToULong(obj.GetEntityId()));
#else
            return obj.GetInstanceID();
#endif
        }
    }
}
