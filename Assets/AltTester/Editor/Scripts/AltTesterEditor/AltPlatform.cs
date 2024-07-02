/*
    Copyright(C) 2024 Altom Consulting
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
        WebGL
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
