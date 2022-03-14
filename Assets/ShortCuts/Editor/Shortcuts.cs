using Altom.AltUnityTesterEditor;

namespace altunitytester.Assets.ShortCuts.Editor
{
    public class Shortcuts
    {
        [UnityEditor.MenuItem("AltUnity Tools/Add AltUnityTesterSymbol &#]", false, 80)]
        public static void AddAUTSymbol()
        {
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTUNITYTESTER"))
                scriptingDefineSymbolsForGroup += ";ALTUNITYTESTER";
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup, scriptingDefineSymbolsForGroup);
        }

        [UnityEditor.MenuItem("AltUnity Tools/Deselect All Tests &#[", false, 80)]
        public static void DeselectAllTests()
        {
            foreach (var test in AltUnityTesterEditorWindow.EditorConfiguration.MyTests)
            {
                test.Selected = false;
            }
        }

    }
}