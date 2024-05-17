/*
    Copyright(C) 2023 Altom Consulting

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

using System;
using UnityEditor;

namespace AltTester.AltTesterUnitySDK.Editor.Platform
{
    public enum AltPlatform
    {
        Android,
        iOS,
        Editor,
        Standalone,
        WebGL,
    }
    public static class AltPlatformExtensions
    {

        public static BuildTargetGroup GetBuildTargetGroupFromAltPlatform(AltPlatform altPlatform)
        {
            switch (altPlatform)
            {
                case AltPlatform.Android:
                    return BuildTargetGroup.Android;
                case AltPlatform.Standalone:
                    return BuildTargetGroup.Standalone;
                case AltPlatform.iOS:
                    return BuildTargetGroup.iOS;
                case AltPlatform.WebGL:
                    return BuildTargetGroup.WebGL;

                default:
                    throw new NotImplementedException();


            }
        }
        public static AltPlatform GetAltPlatformFromBuildTargetGroup(BuildTargetGroup targetGroup)
        {
            switch (targetGroup)
            {
                case BuildTargetGroup.Standalone:
                    return AltPlatform.Standalone;
                case BuildTargetGroup.Android:
                    return AltPlatform.Android;
                case BuildTargetGroup.WebGL:
                    return AltPlatform.WebGL;
                case BuildTargetGroup.iOS:
                    return AltPlatform.iOS;

                default:
                    return AltPlatform.Editor;
            };
        }
        public static BuildTarget[] GetBuildTargetFromAltPlatform(AltPlatform altPlatform)
        {
            switch (altPlatform)
            {
                case AltPlatform.Android:
                    return new BuildTarget[] { BuildTarget.Android };
                case AltPlatform.Standalone:
                    return new BuildTarget[] { BuildTarget.StandaloneWindows, BuildTarget.StandaloneWindows64, BuildTarget.StandaloneOSX, BuildTarget.StandaloneLinux64 };
                case AltPlatform.iOS:
                    return new BuildTarget[] { BuildTarget.iOS };
                case AltPlatform.WebGL:
                    return new BuildTarget[] { BuildTarget.WebGL };

                default:
                    throw new NotImplementedException();

            }
        }

        public static BuildTarget GetBuildTargetFromAltPlatform(AltPlatform altPlatform, BuildTarget standaloneTarget = BuildTarget.NoTarget)
        {
            switch (altPlatform)
            {
                case AltPlatform.Android:
                    return BuildTarget.Android;
                case AltPlatform.Standalone:
                    return standaloneTarget;
                case AltPlatform.iOS:
                    return BuildTarget.iOS;
                case AltPlatform.WebGL:
                    return BuildTarget.WebGL;
                default:
                    throw new NotImplementedException();

            }
        }

    }
}
