/*
    Copyright(C) 2025 Altom Consulting
*/
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#if UNITY_6000_0_OR_NEWER
using UnityEditor.Build;
#endif

[InitializeOnLoad]
public class TMPDefineSetter
{
    static TMPDefineSetter()
    {
        UnityEngine.Debug.Log("|AltTester| Checking for TextMeshPro...");
        updateScriptingDefinesForTMP();
    }
    public static void CheckAndSetTMPDefine()
    {
        updateScriptingDefinesForTMP();
    }
    private static void updateScriptingDefinesForTMP()
    {
#if UNITY_6000_0_OR_NEWER
        var defines = PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
        if (isTMPInstalled() && !defines.Contains("TMP_PRESENT"))
        {
            var newDefines = string.IsNullOrEmpty(defines) ? "TMP_PRESENT" : defines + ";TMP_PRESENT";
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup), newDefines);
            UnityEngine.Debug.Log("|AltTester| TextMeshPro detected. 'TMP_PRESENT' define added.");
        }
        else
        if(!isTMPInstalled() && defines.Contains("TMP_PRESENT"))
        {
            var newDefines = defines.Replace("TMP_PRESENT", "").Trim(';').Replace(";;", ";");
             PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup), newDefines);
        }
#else
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (isTMPInstalled() && !defines.Contains("TMP_PRESENT"))
        {
            var newDefines = string.IsNullOrEmpty(defines) ? "TMP_PRESENT" : defines + ";TMP_PRESENT";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
            UnityEngine.Debug.Log("|AltTester| TextMeshPro detected. 'TMP_PRESENT' define added.");
        }
        else
            if (!isTMPInstalled() && defines.Contains("TMP_PRESENT"))
            {
                var newDefines = defines.Replace("TMP_PRESENT", "").Trim(';').Replace(";;", ";");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
            }
#endif

    }

    private static bool isTMPInstalled()
    {
        return System.AppDomain.CurrentDomain.GetAssemblies()
            .Any(asm => asm.GetName().Name == "Unity.TextMeshPro");
    }
}
#endif