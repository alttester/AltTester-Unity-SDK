/*
    Copyright(C) 2025 Altom Consulting
*/
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

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
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (isTMPInstalled() && !defines.Contains("TMP_PRESENT"))
        {
            var newDefines = string.IsNullOrEmpty(defines) ? "TMP_PRESENT" : defines + ";TMP_PRESENT";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
            UnityEngine.Debug.Log("|AltTester| TextMeshPro detected. 'TMP_PRESENT' define added.");
        }
    }

    private static bool isTMPInstalled()
    {
        return System.AppDomain.CurrentDomain.GetAssemblies()
            .Any(asm => asm.GetName().Name == "Unity.TextMeshPro");
    }
}
#endif