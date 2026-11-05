/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Linq;
using AltTester.AltTesterUnitySDK.Editor.Logging;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace AltTester.AltTesterUnitySDK.Editor
{
    /// <summary>
    /// Leaves the AltTester native plugins (.jar, .xcframework, .jslib, ...) out of builds without ALTTESTER.
    /// The precompiled AltTester assemblies are left out by their define constraints, which don't apply to native plugins.
    /// </summary>
    internal class AltNativePluginFilter : IPreprocessBuildWithReport
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();
        private static readonly string[] altTesterRuntimeFolders = { "/AltTester/Runtime/", "/com.alttester.sdk/Runtime/" };

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // The source version of the SDK compiles its runtime into every build and that code calls the native
            // plugins, so they can only be left out when the runtime comes as precompiled assemblies.
            bool include = AltBuildInstrumentation.IsInstrumentedBuild(report.summary.platform) || !isRuntimePrecompiled();
            foreach (var importer in PluginImporter.GetAllImporters().Where(isAltTesterNativePlugin))
            {
                importer.SetIncludeInBuildDelegate(_ => include);
            }
            if (!include)
                logger.Info("ALTTESTER is not defined for this build, AltTester native plugins are left out.");
        }

        private static bool isAltTesterNativePlugin(PluginImporter importer)
        {
            var path = importer.assetPath;
            return !path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                && altTesterRuntimeFolders.Any(folder => path.IndexOf(folder, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static bool isRuntimePrecompiled()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => assembly.GetName().Name == "AltTesterDriverModule");
        }
    }
}
