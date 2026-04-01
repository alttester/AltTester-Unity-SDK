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

#if UNITY_IOS
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class AltProxyFinderPostProcess
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        Debug.Log("OnPostProcessBuild: " + buildTarget);

        string modulemapContent = "framework module UnityFramework {\n"
            + "    umbrella header \"UnityFramework.h\"\n"
            + "    export *\n"
            + "    module * { export * }\n"
            + "    module UnityInterface {\n"
            + "        header \"UnityInterface.h\"\n"
            + "        export *\n"
            + "    }\n"
            + "}\n";

        if (buildTarget == BuildTarget.iOS)
        {
            var projectPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

            var project = new PBXProject();
            project.ReadFromFile(projectPath);

            var unityFrameworkGuid = project.GetUnityFrameworkTargetGuid();

            // Modulemap
            project.AddBuildProperty(unityFrameworkGuid, "DEFINES_MODULE", "YES");

            // Required to load all Objective-C classes from static libraries (e.g. libNativeInputDialog.a)
            project.AddBuildProperty(unityFrameworkGuid, "OTHER_LDFLAGS", "-ObjC");

            // Explicitly link libNativeInputDialog.a to UnityFramework so that IOS_ShowNativeInput
            // is accessible at runtime. Unity links .a plugins to Unity-iPhone by default; symbols
            // in the main executable are not exported to the embedded UnityFramework dynamic library,
            // which causes a "missing symbol called" dyld crash when the symbol is invoked.
            string nativeDialogLibGuid = project.FindFileGuidByProjectPath(
                "Libraries/AltTester/Runtime/Plugins/iOS/libNativeInputDialog.a");
            if (!string.IsNullOrEmpty(nativeDialogLibGuid))
            {
                project.AddFileToBuild(unityFrameworkGuid, nativeDialogLibGuid);
            }
            else
            {
                Debug.LogWarning("AltTester: Could not find libNativeInputDialog.a in Xcode project. " +
                    "IOS_ShowNativeInput may be unavailable at runtime.");
            }

            // Explicitly link libAltProxyFinder.a to UnityFramework so that _getProxy
            // is accessible at runtime.
            string proxyFinderLibGuid = project.FindFileGuidByProjectPath(
                "Libraries/AltTester/Runtime/AltDriver/Proxy/Plugins/iOS/AltProxyFinder/libAltProxyFinder.a");
            if (!string.IsNullOrEmpty(proxyFinderLibGuid))
            {
                project.AddFileToBuild(unityFrameworkGuid, proxyFinderLibGuid);
            }
            else
            {
                Debug.LogWarning("AltTester: Could not find libAltProxyFinder.a in Xcode project. " +
                    "_getProxy may be unavailable at runtime.");
            }

            var moduleFile = buildPath + "/UnityFramework/UnityFramework.modulemap";
            if (!File.Exists(moduleFile))
            {
                StreamWriter writer = new StreamWriter(moduleFile, false);
                writer.Write(modulemapContent);
                writer.Close();

                project.AddFile(moduleFile, "UnityFramework/UnityFramework.modulemap");
                project.AddBuildProperty(unityFrameworkGuid, "MODULEMAP_FILE", "$(SRCROOT)/UnityFramework/UnityFramework.modulemap");
            }

            // Headers
            string unityInterfaceGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityInterface.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unityInterfaceGuid);

            string unityForwardDeclsGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityForwardDecls.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unityForwardDeclsGuid);

            string unityRenderingGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityRendering.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unityRenderingGuid);

            string unitySharedDeclsGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnitySharedDecls.h");
            project.AddPublicHeaderToBuild(unityFrameworkGuid, unitySharedDeclsGuid);

            // Save project
            project.WriteToFile(projectPath);

            // Allow arbitrary loads in Info.plist so the instrumented app can connect to AltTester
            // Server on iOS 16+. Without this, ATS blocks the WebSocket connection to non-HTTPS
            // endpoints (e.g. ws://127.0.0.1:13000) on iOS 16+ devices.
            var plistPath = buildPath + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            var atsKey = "NSAppTransportSecurity";
            if (!plist.root.values.ContainsKey(atsKey))
            {
                plist.root.CreateDict(atsKey);
            }
            plist.root[atsKey].AsDict().SetBoolean("NSAllowsArbitraryLoads", true);
            plist.WriteToFile(plistPath);
            Debug.Log("AltTester: Set NSAllowsArbitraryLoads=YES in Info.plist to allow connection on iOS 16+.");
        }

        Debug.Log("OnPostProcessBuild: Complete");
    }
}
#endif
