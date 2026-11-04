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
