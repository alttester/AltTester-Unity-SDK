using System;
using Altom.AltTester;
using Altom.AltTesterEditor;
using Altom.AltTesterEditor.Logging;
using UnityEditor;

namespace Altom.AltTesterTools
{
    public class BuildAltTester
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();


        [MenuItem("Build/Mac")]
        protected static void MacBuildFromCommandLine()
        {
            try
            {
                string versionNumber = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                PlayerSettings.companyName = "Altom";
                PlayerSettings.productName = "sampleGame";
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, "com.altom.sampleGame");
                PlayerSettings.bundleVersion = versionNumber;
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
                AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(BuildTargetGroup.Standalone);
                var instrumentationSettings = getInstrumentationSettings();
                PlayerSettings.fullScreenMode = UnityEngine.FullScreenMode.Windowed;
                PlayerSettings.defaultScreenHeight = 1080;
                PlayerSettings.defaultScreenWidth = 1920;


                logger.Debug("Starting Mac build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = GetScene(),

                    locationPathName = "sampleGame",
                    target = BuildTarget.StandaloneOSX,
                    options = BuildOptions.Development | BuildOptions.IncludeTestAssemblies | BuildOptions.AutoRunPlayer
                };



                AltBuilder.InsertAltInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

                var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
                AltBuilder.RemoveAltTesterFromScriptingDefineSymbols(BuildTargetGroup.Standalone);


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
                    logger.Info("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                    EditorApplication.Exit(0);
                }

                logger.Error("Total Errors: " + results.summary.totalErrors);
                logger.Error("Build failed! " + results.steps + "\n Result: " + results.summary.result + "\n Stripping info: " + results.strippingInfo);
                EditorApplication.Exit(1);
#endif

            }
            catch (Exception exception)
            {
                logger.Error(exception);
                EditorApplication.Exit(1);
            }

        }
        [MenuItem("Build/Android")]
        protected static void AndroidBuildFromCommandLine()
        {
            try
            {
                string versionNumber = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                PlayerSettings.companyName = "Altom";
                PlayerSettings.productName = "sampleGame";
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.altom.sampleGame");
                PlayerSettings.bundleVersion = versionNumber;
                PlayerSettings.Android.bundleVersionCode = int.Parse(versionNumber);
                PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
#if UNITY_2018_1_OR_NEWER
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
#endif
                AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(BuildTargetGroup.Android);
                var instrumentationSettings = getInstrumentationSettings();


                logger.Debug("Starting Android build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = GetScene(),

                    locationPathName = "sampleGame.apk",
                    target = BuildTarget.Android,
                    options = BuildOptions.Development | BuildOptions.IncludeTestAssemblies | BuildOptions.AutoRunPlayer
                };



                AltBuilder.InsertAltInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

                var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
                AltBuilder.RemoveAltTesterFromScriptingDefineSymbols(BuildTargetGroup.Android);


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
                    // EditorApplication.Exit(1);
                }

#endif

                logger.Info("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                // EditorApplication.Exit(0);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                // EditorApplication.Exit(1);
            }

        }
        public static string[] GetScene()
        {
            return new string[]
                    {
                    "Assets/AltTester/Examples/Scenes/Scene 1 AltDriverTestScene.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 2 Draggable Panel.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 3 Drag And Drop.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 4 No Cameras.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 5 Keyboard Input.unity",
                    "Assets/AltTester/Examples/Scenes/Scene6.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 7 Drag And Drop NIS.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 8 Draggable Panel NIP.unity",
                    "Assets/AltTester/Examples/Scenes/scene 9 NIS.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 10 Sample NIS.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 7 New Input System Actions.unity",
                    "Assets/AltTester/Examples/Scenes/Scene 11 ScrollView Scene.unity"
                    };
        }

        [MenuItem("Build/iOS")]
        protected static void IosBuildFromCommandLine()
        {
            try
            {
                string versionNumber = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                PlayerSettings.companyName = "Altom";
                PlayerSettings.productName = "sampleGame";
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.altom.sampleGame");
                PlayerSettings.bundleVersion = versionNumber;
                PlayerSettings.iOS.appleEnableAutomaticSigning = true;
                PlayerSettings.iOS.appleDeveloperTeamID = "59ESG8ELF5";
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
                logger.Debug("Starting IOS build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);

                var buildPlayerOptions = new BuildPlayerOptions
                {
                    locationPathName = "sampleGame",
                    scenes = GetScene(),

                    target = BuildTarget.iOS,
                    options = BuildOptions.Development | BuildOptions.IncludeTestAssemblies | BuildOptions.AutoRunPlayer
                };

                AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(BuildTargetGroup.iOS);
                var instrumentationSettings = getInstrumentationSettings();
                AltBuilder.InsertAltInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

                var results = BuildPipeline.BuildPlayer(buildPlayerOptions);

#if UNITY_2017
            if (results.Equals(""))
            {
                logger.Info("Build succeeded!");

            }
            else
            logger.Error("Build failed!");
            EditorApplication.Exit(1);

#else
                if (results.summary.totalErrors == 0)
                {
                    logger.Info("Build succeeded!");

                }
                else
                {
                    logger.Error("Build failed!");
                    EditorApplication.Exit(1);
                }

#endif
                logger.Info("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                EditorApplication.Exit(0);

            }
            catch (Exception exception)
            {
                logger.Error(exception);
                EditorApplication.Exit(1);
            }
        }

        [MenuItem("Build/WebGL")]
        protected static void WebGLBuildFromCommandLine()
        {
            try
            {
                string versionNumber = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                PlayerSettings.companyName = "Altom";
                PlayerSettings.productName = "sampleGame";
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.WebGL, "com.altom.sampleGame");
                PlayerSettings.bundleVersion = versionNumber;
                PlayerSettings.Android.bundleVersionCode = int.Parse(versionNumber);
                PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WebGL, ApiCompatibilityLevel.NET_4_6);
                PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
                PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.FullWithoutStacktrace;

                logger.Debug("Starting WebGL build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = GetScene(),

                    locationPathName = "build/webgl",
                    target = BuildTarget.WebGL,
                    options = BuildOptions.Development | BuildOptions.IncludeTestAssemblies | BuildOptions.AutoRunPlayer
                };

                AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(BuildTargetGroup.WebGL);


                var instrumentationSettings = getInstrumentationSettings();
                AltBuilder.InsertAltInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

                var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
                AltBuilder.RemoveAltTesterFromScriptingDefineSymbols(BuildTargetGroup.WebGL);


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
            catch (Exception exception)
            {
                logger.Error(exception);
                EditorApplication.Exit(1);
            }
        }

        private static AltInstrumentationSettings getInstrumentationSettings()
        {
            if (AltTesterEditorWindow.EditorConfiguration == null)
            {
                var instrumentationSettings = new AltInstrumentationSettings();
                var altTesterPort = System.Environment.GetEnvironmentVariable("ALTTESTER_PORT");

                if (!string.IsNullOrEmpty(altTesterPort)) //server mode
                {
                    instrumentationSettings.AltTesterPort = int.Parse(altTesterPort);
                    return instrumentationSettings;
                }

                var proxyHost = System.Environment.GetEnvironmentVariable("PROXY_HOST");

                if (!string.IsNullOrEmpty(proxyHost)) //proxy mode
                {
                    instrumentationSettings.InstrumentationMode = AltInstrumentationMode.Proxy;
                    instrumentationSettings.ProxyHost = proxyHost;
                }
                var proxyPort = System.Environment.GetEnvironmentVariable("PROXY_PORT");
                if (!string.IsNullOrEmpty(proxyPort))//proxy mode
                {
                    instrumentationSettings.InstrumentationMode = AltInstrumentationMode.Proxy;
                    instrumentationSettings.ProxyPort = int.Parse(proxyPort);
                }

                return instrumentationSettings;
            }

            return AltTesterEditorWindow.EditorConfiguration.GetInstrumentationSettings();
        }
    }
}