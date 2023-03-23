using AltTester.AltTesterUnitySDK.Editor;

namespace alttester.Assets.ShortCuts.Editor
{
    public class Shortcuts
    {
        [UnityEditor.MenuItem("AltTester/Add AltTesterSymbol &#]", false, 80)]
        public static void AddAUTSymbol()
        {
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTTESTER"))
                scriptingDefineSymbolsForGroup += ";ALTTESTER";
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup, scriptingDefineSymbolsForGroup);
        }

        [UnityEditor.MenuItem("AltTester/Deselect All Tests &#[", false, 80)]
        public static void DeselectAllTests()
        {
            foreach (var test in AltTesterEditorWindow.EditorConfiguration.MyTests)
            {
                test.Selected = false;
            }
        }

    }
}