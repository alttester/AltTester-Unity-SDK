/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterUnitySDK.Editor;
using UnityEditor.Build;

namespace alttester.Assets.ShortCuts.Editor
{
    public class Shortcuts
    {
#if ALTTESTER_DEVELOPMENT
        [UnityEditor.MenuItem("AltTester®/Shortcuts/Add AltTesterSymbol &#]", false, 80)]
        public static void AddAUTSymbol()
        {
#if UNITY_6000_0_OR_NEWER
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTTESTER"))
                scriptingDefineSymbolsForGroup += ";ALTTESTER";
            UnityEditor.PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, scriptingDefineSymbolsForGroup);
#else
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTTESTER"))
                scriptingDefineSymbolsForGroup += ";ALTTESTER";
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup, scriptingDefineSymbolsForGroup);
#endif
        }

        [UnityEditor.MenuItem("AltTester®/Shortcuts/Deselect All Tests &#[", false, 80)]
        public static void DeselectAllTests()
        {
            foreach (var test in AltTesterEditorWindow.EditorConfiguration.MyTests)
            {
                test.Selected = false;
            }
        }
#endif

    }
}
