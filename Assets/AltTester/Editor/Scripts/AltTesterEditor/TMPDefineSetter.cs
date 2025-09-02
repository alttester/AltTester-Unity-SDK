#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;

[InitializeOnLoad]
public class TMPDefineSetter
{
    static TMPDefineSetter()
    {
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (isTMPInstalled() && !defines.Contains("TMP_PRESENT"))
        {
            var newDefines = string.IsNullOrEmpty(defines) ? "TMP_PRESENT" : defines + ";TMP_PRESENT";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
        }
    }

    private static bool isTMPInstalled()
    {
        return System.AppDomain.CurrentDomain.GetAssemblies()
            .Any(asm => asm.GetName().Name == "Unity.TextMeshPro");
    }
}
#endif