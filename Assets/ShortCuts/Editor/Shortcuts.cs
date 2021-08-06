using Altom.Editor;

namespace altunitytester.Assets.ShortCuts.Editor
{
    public class Shortcuts
    {
        [UnityEditor.MenuItem("AltUnity Tools/Add AltUnityTesterSymbol _a", false, 80)]
        public static void AddAUTSymbol()
        {
            var scriptingDefineSymbolsForGroup = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!scriptingDefineSymbolsForGroup.Contains("ALTUNITYTESTER"))
                scriptingDefineSymbolsForGroup += ";ALTUNITYTESTER";
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup, scriptingDefineSymbolsForGroup);
        }

        [UnityEditor.MenuItem("AltUnity Tools/Deselect All Tests _d", false, 80)]
        public static void DeselectAllTests()
        {
            foreach (var test in AltUnityTesterEditor.EditorConfiguration.MyTests)
            {
                test.Selected = false;
            }
        }

    }
}