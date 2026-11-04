/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace AltTester.AltTesterUnitySDK.Editor
{
    /// <summary>
    /// Tells whether a player build is instrumented, i.e. compiled with the ALTTESTER define.
    /// Editor assemblies don't see the defines of the build being made (for example
    /// BuildPlayerOptions.extraScriptingDefines), so this is decided at build time instead of with #if ALTTESTER.
    /// </summary>
    public static class AltBuildInstrumentation
    {
        public const string AltTesterDefine = "ALTTESTER";

        private static string[] extraScriptingDefines = new string[0];
        // A Dedicated Server build has the Standalone build target, but its own Player Settings defines.
        private static bool isServerBuild;

        public static bool IsInstrumentedBuild(BuildTarget target)
        {
            return GetBuildDefines(target).Contains(AltTesterDefine);
        }

        /// <summary>
        /// Defines the player build for <paramref name="target"/> is compiled with: Player Settings (the Dedicated
        /// Server ones for a server build), the active build profile (Unity 6) and the extraScriptingDefines of the
        /// build in progress.
        /// </summary>
        public static HashSet<string> GetBuildDefines(BuildTarget target)
        {
            var defines = new HashSet<string>(SplitDefines(getPlayerSettingsDefines(target)));
            defines.UnionWith(extraScriptingDefines);
            defines.UnionWith(getActiveBuildProfileDefines());
            return defines;
        }

        public static string GetPlayerSettingsDefines(BuildTargetGroup targetGroup)
        {
#if UNITY_6000_0_OR_NEWER
            return PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup));
#else
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
#endif
        }

        public static string[] SplitDefines(string defines)
        {
            if (string.IsNullOrEmpty(defines))
                return new string[0];
            return defines.Split(';').Select(define => define.Trim()).Where(define => define.Length > 0).ToArray();
        }

        internal static void SetExtraScriptingDefines(string[] defines)
        {
            extraScriptingDefines = defines ?? new string[0];
        }

        internal static void SetServerBuild(bool serverBuild)
        {
            isServerBuild = serverBuild;
        }

        private static string getPlayerSettingsDefines(BuildTarget target)
        {
#if UNITY_2021_2_OR_NEWER
            if (isServerBuild)
                return PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Server);
#endif
            return GetPlayerSettingsDefines(BuildPipeline.GetBuildTargetGroup(target));
        }

        private static IEnumerable<string> getActiveBuildProfileDefines()
        {
            // Build profiles (Unity 6) add their own scripting defines on top of Player Settings. Read them through
            // reflection because this assembly is also compiled against Unity versions without build profiles.
            try
            {
                var buildProfileType = Type.GetType("UnityEditor.Build.Profile.BuildProfile, UnityEditor.CoreModule");
                if (buildProfileType == null)
                    return new string[0];
                var activeProfile = buildProfileType.GetMethod("GetActiveBuildProfile", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, null);
                if (activeProfile == null)
                    return new string[0];
                return buildProfileType.GetProperty("scriptingDefines", BindingFlags.Public | BindingFlags.Instance)?.GetValue(activeProfile) as string[] ?? new string[0];
            }
            catch (Exception)
            {
                return new string[0];
            }
        }
    }

#if UNITY_2021_2_OR_NEWER
    /// <summary>
    /// Captures BuildPlayerOptions.extraScriptingDefines for builds started from scripts or CI, and whether the build
    /// is a Dedicated Server build. BuildPlayerProcessor exists from Unity 2021.2; older versions only see the Player
    /// Settings defines, and their server builds use the Standalone ones.
    /// </summary>
    internal class AltBuildDefinesCollector : BuildPlayerProcessor
    {
        public override void PrepareForBuild(BuildPlayerContext buildPlayerContext)
        {
            var options = buildPlayerContext.BuildPlayerOptions;
            AltBuildInstrumentation.SetExtraScriptingDefines(options.extraScriptingDefines);
            AltBuildInstrumentation.SetServerBuild(BuildPipeline.GetBuildTargetGroup(options.target) == BuildTargetGroup.Standalone
                && options.subtarget == (int)StandaloneBuildSubtarget.Server);
        }
    }
#endif

    internal class AltBuildDefinesReset : IPostprocessBuildWithReport
    {
        // Run after every other post-process step, which may still ask whether the build is instrumented.
        public int callbackOrder => int.MaxValue;

        public void OnPostprocessBuild(BuildReport report)
        {
            AltBuildInstrumentation.SetExtraScriptingDefines(null);
            AltBuildInstrumentation.SetServerBuild(false);
        }
    }
}
