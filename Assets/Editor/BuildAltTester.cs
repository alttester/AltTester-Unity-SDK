/*
    Copyright(C) 2025 Altom Consulting

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

using System;
using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterUnitySDK;
using AltTester.AltTesterUnitySDK.Editor;
using AltTester.AltTesterUnitySDK.Editor.Logging;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace AltTesterTools
{
    public class BuildAltTester
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();


        [MenuItem("Build/Mac")]
        protected static void MacBuildFromCommandLine()
        {
            try
            {
                SetCommonSettings(BuildTargetGroup.Standalone);

                PlayerSettings.fullScreenMode = UnityEngine.FullScreenMode.Windowed;
                PlayerSettings.defaultScreenHeight = 1080;
                PlayerSettings.defaultScreenWidth = 1920;
                AltBuilder.AddScriptingDefineSymbol("TMP_PRESENT", BuildTargetGroup.Standalone);

                logger.Debug("Starting Mac build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                var buildPlayerOptions = GetBuildPlayerOptions("sampleGame", BuildTarget.StandaloneOSX);
                buildGame(buildPlayerOptions, BuildTargetGroup.Standalone);

            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }

        }



        [MenuItem("Build/Android")]
        protected static void AndroidBuildFromCommandLine()
        {
            try
            {
                SetCommonSettings(BuildTargetGroup.Android);

                PlayerSettings.Android.bundleVersionCode = int.Parse(PlayerSettings.bundleVersion);
                PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
                AltBuilder.AddScriptingDefineSymbol("TMP_PRESENT", BuildTargetGroup.Android);
#if UNITY_2018_1_OR_NEWER
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
#endif
                logger.Debug("Starting Android build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                var buildPlayerOptions = GetBuildPlayerOptions("sampleGame.apk", BuildTarget.Android, false);

                buildGame(buildPlayerOptions, BuildTargetGroup.Android);

            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }

        }

        [MenuItem("Build/iOS")]
        protected static void IosBuildFromCommandLine()
        {
            try
            {
                SetCommonSettings(BuildTargetGroup.iOS);
                PlayerSettings.iOS.appleEnableAutomaticSigning = true;
                PlayerSettings.iOS.appleDeveloperTeamID = "59ESG8ELF5";
                AltBuilder.AddScriptingDefineSymbol("TMP_PRESENT", BuildTargetGroup.iOS);

                logger.Debug("Starting IOS build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);

                var buildPlayerOptions = GetBuildPlayerOptions("sampleGame", BuildTarget.iOS, false);
                buildGame(buildPlayerOptions, BuildTargetGroup.iOS);

            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        [MenuItem("Build/WebGL")]
        protected static void WebGLBuildFromCommandLine()
        {
            try
            {
                SetCommonSettings(BuildTargetGroup.WebGL);

                PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
                PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.FullWithStacktrace;

                logger.Debug("Starting WebGL build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                var buildPlayerOptions = GetBuildPlayerOptions("build/webgl", BuildTarget.WebGL);
                AltBuilder.AddScriptingDefineSymbol("TMP_PRESENT", BuildTargetGroup.WebGL);
                AltBuilder.AddScriptingDefineSymbol("UNITY_WEBGL", BuildTargetGroup.WebGL);
                buildGame(buildPlayerOptions, BuildTargetGroup.WebGL);

                AltBuilder.RemoveScriptingDefineSymbol("UNITY_WEBGL", BuildTargetGroup.WebGL);


            }
            catch (Exception exception)
            {
                logger.Error(exception);
                EditorApplication.Exit(1);
            }
        }

        private static AltInstrumentationSettings getInstrumentationSettings()
        {
            var instrumentationSettings = new AltInstrumentationSettings();

            var host = System.Environment.GetEnvironmentVariable("ALTSERVER_HOST");
            if (!string.IsNullOrEmpty(host))
            {
                instrumentationSettings.AltServerHost = host;
            }

            var port = System.Environment.GetEnvironmentVariable("ALTSERVER_PORT");
            if (!string.IsNullOrEmpty(port))
            {
                instrumentationSettings.AltServerPort = int.Parse(port);
            }
            else
            {
                instrumentationSettings.AltServerPort = 13010;
            }
            instrumentationSettings.ResetConnectionData = true;
            instrumentationSettings.UID = UnityEngine.SystemInfo.deviceUniqueIdentifier.ToString() + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

            return instrumentationSettings;
        }
        public static string[] GetScenes()
        {
            return new string[]
                    {
                    "Assets/Examples/Scenes/Scene 1 AltDriverTestScene.unity",
                    "Assets/Examples/Scenes/Scene 2 Draggable Panel.unity",
                    "Assets/Examples/Scenes/Scene 3 Drag And Drop.unity",
                    "Assets/Examples/Scenes/Scene 4 No Cameras.unity",
                    "Assets/Examples/Scenes/Scene 5 Keyboard Input.unity",
                    "Assets/Examples/Scenes/Scene6.unity",
                    "Assets/Examples/Scenes/Scene 7 Drag And Drop NIS.unity",
                    "Assets/Examples/Scenes/Scene 8 Draggable Panel NIP.unity",
                    "Assets/Examples/Scenes/scene 9 NIS.unity",
                    "Assets/Examples/Scenes/Scene 10 Sample NIS.unity",
                    "Assets/Examples/Scenes/Scene 7 New Input System Actions.unity",
                    "Assets/Examples/Scenes/Scene 11 ScrollView Scene.unity",
                    "Assets/Examples/Scenes/Sceme 12 2D Objects.unity",
                    "Assets/Examples/Scenes/DragDemo.unity"
                    };
        }
        private static void buildGame(BuildPlayerOptions buildPlayerOptions, BuildTargetGroup targetGroup)
        {
            var instrumentationSettings = getInstrumentationSettings();

            AltBuilder.InsertAltInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
            AltBuilder.RemoveAltTesterFromScriptingDefineSymbols(targetGroup);
            HandleResults(results);
        }

        private static void HandleResults(BuildReport results)
        {
#if UNITY_2017
            if (results.Equals(""))
            {
                logger.Info("Build succeeded!");
                EditorApplication.Exit(0);

            }
            else
                {
                    logger.Error("Build failed!");
                    EditorApplication.Exit(1);
                }

#else
            if (results.summary.totalErrors == 0)
            {
                logger.Info("Build succeeded!");
            }
            else
            {
                logger.Error("Total Errors: " + results.summary.totalErrors);
                logger.Error("Build failed! " + results.steps + "\n Result: " + results.summary.result + "\n Stripping info: " + results.strippingInfo);
                EditorApplication.Exit(1);
            }

#endif

            logger.Info("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            EditorApplication.Exit(0);
        }
        public static void SetCommonSettings(BuildTargetGroup targetGroup)
        {
            string versionNumber = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            PlayerSettings.companyName = "Altom";
            PlayerSettings.productName = "sampleGame";
            PlayerSettings.bundleVersion = versionNumber;
            PlayerSettings.SetApplicationIdentifier(targetGroup, "com.altom.sampleGame");
            PlayerSettings.SetApiCompatibilityLevel(targetGroup, ApiCompatibilityLevel.NET_4_6);
            AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(targetGroup);

        }
        public static BuildPlayerOptions GetBuildPlayerOptions(string locationPathName, BuildTarget target, bool autorun = true)
        {
            return new BuildPlayerOptions
            {
                scenes = GetScenes(),

                locationPathName = locationPathName,
                target = target,
                options = BuildOptions.Development | BuildOptions.IncludeTestAssemblies | (autorun ? BuildOptions.AutoRunPlayer : BuildOptions.None)
            };
        }
    }
}
