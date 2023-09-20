using UnityEditor;

public class Test
{

    [MenuItem("Build/test")]
    public static void test()
    {
        foreach (var p in PluginImporter.GetAllImporters())
        {
            if (p.assetPath.Contains("SDKLibrary.dll"))
            {
                if (p.assetPath.Contains("Unity2019OldInput"))
                {
                    p.DefineConstraints = new string[]{
                    "UNITY_2019_1_OR_NEWER",
                    "!UNITY_2021_2_OR_NEWER",
                    "!ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };
                }
                if (p.assetPath.Contains("Unity2019NewInput"))
                {
                    p.DefineConstraints = new string[]{
                    "UNITY_2019_1_OR_NEWER",
                    "!UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "!ENABLE_LEGACY_INPUT_MANAGER"
                };
                }
                if (p.assetPath.Contains("Unity2019Both"))
                {
                    p.DefineConstraints = new string[]{
                    "UNITY_2019_1_OR_NEWER",
                    "!UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };
                    if (p.assetPath.Contains("Unity2021OldInput"))
                    {
                        p.DefineConstraints = new string[]{
                    "UNITY_2021_2_OR_NEWER",
                    "!ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };
                    }
                }
                if (p.assetPath.Contains("Unity2021NewInput"))
                {
                    p.DefineConstraints = new string[]{
                    "UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "!ENABLE_LEGACY_INPUT_MANAGER"
                };
                }
                if (p.assetPath.Contains("Unity2021Both"))
                {
                    p.DefineConstraints = new string[]{
                    "UNITY_2021_2_OR_NEWER",
                    "ENABLE_INPUT_SYSTEM",
                    "ENABLE_LEGACY_INPUT_MANAGER"
                };
                }
                p.SaveAndReimport();
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(UnityEditor.BuildPipeline.GetBuildTargetGroup(UnityEditor.EditorUserBuildSettings.activeBuildTarget), "ALTTESTER");

            }
        }
    }
}
