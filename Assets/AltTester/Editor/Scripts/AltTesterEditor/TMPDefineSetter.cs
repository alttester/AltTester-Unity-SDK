/*
    Copyright(C) 2026 Altom Consulting

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