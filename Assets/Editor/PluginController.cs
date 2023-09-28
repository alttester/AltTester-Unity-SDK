using UnityEditor;
namespace AltTesterTools
{
    public class PluginController
    {

        public static void SetDLLSettings()
        {
            foreach (var plugin in PluginImporter.GetAllImporters())
            {
                if (plugin.assetPath.Contains("SDKLibrary.dll"))
                {
                    if (plugin.assetPath.Contains("Unity2019OldInput"))
                    {
                        plugin.DefineConstraints = new string[]{
                    "UNITY_2019_1_OR_NEWER",
                    "!UNITY_2021_2_OR_NEWER",
                    "!ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };
                    }
                    if (plugin.assetPath.Contains("Unity2019NewInput"))
                    {
                        plugin.DefineConstraints = new string[]{
                    "UNITY_2019_1_OR_NEWER",
                    "!UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "!ENABLE_LEGACY_INPUT_MANAGER"
                };
                    }
                    if (plugin.assetPath.Contains("Unity2019Both"))
                    {
                        plugin.DefineConstraints = new string[]{
                    "UNITY_2019_1_OR_NEWER",
                    "!UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };

                    }
                    if (plugin.assetPath.Contains("Unity2021OldInput"))
                    {
                        plugin.DefineConstraints = new string[]{
                    "UNITY_2021_2_OR_NEWER",
                    "!ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };
                    }
                    if (plugin.assetPath.Contains("Unity2021NewInput"))
                    {
                        plugin.DefineConstraints = new string[]{
                    "UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "!ENABLE_LEGACY_INPUT_MANAGER"
                };
                    }
                    if (plugin.assetPath.Contains("Unity2021Both"))
                    {
                        plugin.DefineConstraints = new string[]{
                    "UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };
                    }
                    plugin.SaveAndReimport();
                    UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(UnityEditor.BuildPipeline.GetBuildTargetGroup(UnityEditor.EditorUserBuildSettings.activeBuildTarget), "ALTTESTER");

                }
                if (plugin.assetPath.Contains("SDKLibrary.dll"))
                {
                    plugin.SetCompatibleWithAnyPlatform(false);
                    plugin.SetCompatibleWithEditor(true);
                }


            }
        }
    }
}
