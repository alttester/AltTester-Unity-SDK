/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AltTester.AltTesterUnitySDK.Editor.Logging;
using UnityEditor;
#if UNITY_2021_2_OR_NEWER
using UnityEditor.Build;
#endif

namespace AltTester.AltTesterUnitySDK.Editor
{
    /// <summary>
    /// Adds or removes a scripting define symbol on every build target: the Player Settings of each platform,
    /// Dedicated Server included, and on Unity 6 each build profile's scripting defines and Player Settings override.
    /// </summary>
    internal static class AltScriptingDefineSymbols
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        internal static void AddEverywhere(string symbol)
        {
            updateEverywhere(defines => defines.Contains(symbol) ? defines : defines.Append(symbol).ToArray(), $"Added {symbol} to");
        }

        internal static void RemoveEverywhere(string symbol)
        {
            updateEverywhere(defines => defines.Where(define => define != symbol).ToArray(), $"Removed {symbol} from");
        }

        private static void updateEverywhere(Func<string[], string[]> change, string action)
        {
            var updated = new List<string>();
            updatePlayerSettings(change, updated);
#if UNITY_6000_0_OR_NEWER
            updateBuildProfiles(change, updated);
#endif
            if (updated.Count > 0)
                logger.Info($"{action} the scripting define symbols of {string.Join(", ", updated)}.");
        }

        private static void updatePlayerSettings(Func<string[], string[]> change, List<string> updated)
        {
#if UNITY_2021_2_OR_NEWER
            // NamedBuildTarget, unlike BuildTargetGroup, has Dedicated Server (Server) as its own target.
            foreach (var target in namedBuildTargets())
            {
                try
                {
                    var defines = AltBuildInstrumentation.SplitDefines(PlayerSettings.GetScriptingDefineSymbols(target));
                    var newDefines = change(defines);
                    if (newDefines.SequenceEqual(defines))
                        continue;
                    PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", newDefines));
                    updated.Add(target.TargetName);
                }
                catch (Exception)
                {
                    // A target this Unity version has no Player Settings for.
                }
            }
#else
            var buildTargetGroups = typeof(BuildTargetGroup)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => !field.IsDefined(typeof(ObsoleteAttribute), false))
                .Select(field => (BuildTargetGroup)field.GetValue(null))
                .Where(targetGroup => targetGroup != BuildTargetGroup.Unknown)
                .Distinct();
            foreach (var targetGroup in buildTargetGroups)
            {
                try
                {
                    var defines = AltBuildInstrumentation.SplitDefines(PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup));
                    var newDefines = change(defines);
                    if (newDefines.SequenceEqual(defines))
                        continue;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", newDefines));
                    updated.Add(targetGroup.ToString());
                }
                catch (Exception)
                {
                    // A group this Unity version has no Player Settings for.
                }
            }
#endif
        }

#if UNITY_2021_2_OR_NEWER
        private static IEnumerable<NamedBuildTarget> namedBuildTargets()
        {
            return typeof(NamedBuildTarget)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.FieldType == typeof(NamedBuildTarget) && !field.IsDefined(typeof(ObsoleteAttribute), false))
                .Select(field => (NamedBuildTarget)field.GetValue(null))
                .Where(target => !string.IsNullOrEmpty(target.TargetName))
                .GroupBy(target => target.TargetName)
                .Select(group => group.First());
        }
#endif

#if UNITY_6000_0_OR_NEWER
        // Build profiles are read through reflection: the player settings override, which has its own scripting
        // define symbols, is only reachable through internal API (the same calls Unity's Build Profiles window makes).
        private static void updateBuildProfiles(Func<string[], string[]> change, List<string> updated)
        {
            var buildProfileType = Type.GetType("UnityEditor.Build.Profile.BuildProfile, UnityEditor.CoreModule");
            if (buildProfileType == null)
                return;
            var scriptingDefinesProperty = buildProfileType.GetProperty("scriptingDefines", BindingFlags.Public | BindingFlags.Instance);
            var playerSettingsProperty = buildProfileType.GetProperty("playerSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            var serializePlayerSettings = buildProfileType.GetMethod("SerializePlayerSettings", BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
            var activeProfile = buildProfileType.GetMethod("GetActiveBuildProfile", BindingFlags.Public | BindingFlags.Static)?.Invoke(null, null) as UnityEngine.Object;

            foreach (var guid in AssetDatabase.FindAssets("t:BuildProfile"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var profile = AssetDatabase.LoadAssetAtPath(path, buildProfileType);
                if (profile == null)
                    continue;
                var profileUpdated = false;

                if (scriptingDefinesProperty != null)
                {
                    var defines = scriptingDefinesProperty.GetValue(profile) as string[] ?? new string[0];
                    var newDefines = change(defines);
                    if (!newDefines.SequenceEqual(defines))
                    {
                        scriptingDefinesProperty.SetValue(profile, newDefines);
                        profileUpdated = true;
                        // The editor compiles with a cached copy of the active profile's defines.
                        if (profile == activeProfile)
                            requestScriptCompilation(profile);
                    }
                }

                // Loaded when the profile is, if the profile overrides Player Settings. The active profile's override
                // was already updated through PlayerSettings, so it has nothing left to change here.
                var playerSettings = playerSettingsProperty?.GetValue(profile) as PlayerSettings;
                if (playerSettings != null)
                {
                    if (serializePlayerSettings == null)
                        logger.Warn($"Could not update the Player Settings override of the build profile {path}. Check its Scripting Define Symbols.");
                    else if (updatePlayerSettingsObject(playerSettings, change, path))
                    {
                        serializePlayerSettings.Invoke(profile, null);
                        profileUpdated = true;
                    }
                }

                if (profileUpdated)
                {
                    EditorUtility.SetDirty(profile);
                    AssetDatabase.SaveAssetIfDirty(profile);
                    updated.Add($"build profile {path}");
                }
            }

            // While a profile that overrides Player Settings is active, PlayerSettings edits the override instead of
            // the project's Player Settings, so those are updated here too.
            var projectPlayerSettings = AssetDatabase.LoadAssetAtPath<PlayerSettings>("ProjectSettings/ProjectSettings.asset");
            if (projectPlayerSettings != null && updatePlayerSettingsObject(projectPlayerSettings, change, "the project Player Settings"))
            {
                EditorUtility.SetDirty(projectPlayerSettings);
                AssetDatabase.SaveAssetIfDirty(projectPlayerSettings);
                updated.Add("the project Player Settings");
            }
        }

        private static bool updatePlayerSettingsObject(PlayerSettings playerSettings, Func<string[], string[]> change, string owner)
        {
            var getDefines = typeof(PlayerSettings).GetMethod("GetScriptingDefineSymbols_Internal", BindingFlags.NonPublic | BindingFlags.Static);
            var setDefines = typeof(PlayerSettings).GetMethod("SetScriptingDefineSymbols_Internal", BindingFlags.NonPublic | BindingFlags.Static);
            if (getDefines == null || setDefines == null)
            {
                logger.Warn($"Could not update the scripting define symbols of {owner}. Check them in the Player Settings.");
                return false;
            }

            var updated = false;
            foreach (var target in namedBuildTargets())
            {
                try
                {
                    var defines = AltBuildInstrumentation.SplitDefines(getDefines.Invoke(null, new object[] { playerSettings, target.TargetName }) as string);
                    var newDefines = change(defines);
                    if (newDefines.SequenceEqual(defines))
                        continue;
                    setDefines.Invoke(null, new object[] { playerSettings, target.TargetName, string.Join(";", newDefines) });
                    updated = true;
                }
                catch (Exception)
                {
                    // A target this Unity version has no Player Settings for.
                }
            }
            return updated;
        }

        private static void requestScriptCompilation(UnityEngine.Object profile)
        {
            var moduleUtilType = Type.GetType("UnityEditor.Build.Profile.BuildProfileModuleUtil, UnityEditor.CoreModule");
            var request = moduleUtilType?.GetMethod("RequestScriptCompilation", BindingFlags.Public | BindingFlags.Static);
            if (request != null)
                request.Invoke(null, new object[] { profile });
            else
                logger.Warn("The scripts are recompiled with the active build profile's new scripting defines the next time the profile is activated.");
        }
#endif
    }
}
