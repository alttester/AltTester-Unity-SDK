/*
    Copyright(C) 2026 Altom Consulting
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
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
        Switch
#endif
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
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                case AltPlatform.Switch:
                    return BuildTargetGroup.Switch;
#endif
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
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                case BuildTargetGroup.Switch:
                    return AltPlatform.Switch;
#endif
                default:
                    return AltPlatform.Editor;
            }

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
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                case AltPlatform.Switch:
                    return new BuildTarget[] { BuildTarget.Switch };
#endif
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
#if NINTENDO_ENABLED && ALTTESTER_NONGPL
                case AltPlatform.Switch:
                    return BuildTarget.Switch;
#endif
                default:
                    throw new NotImplementedException();

            }
        }

    }
}
