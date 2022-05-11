using System;
using Altom.AltUnityTester;
using Altom.AltUnityTesterEditor;
using Altom.AltUnityTesterEditor.Logging;
using UnityEditor;

namespace Altom.AltUnityTesterTools
{
    public class BuildAltUnityTester
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        [MenuItem("Build/Android")]
        protected static void AndroidBuildFromCommandLine()
        {
            try
            {
                string versionNumber = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

                PlayerSettings.companyName = "Altom";
                PlayerSettings.productName = "sampleGame";
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "fi.altom.altunitytester");
                PlayerSettings.bundleVersion = versionNumber;
                PlayerSettings.Android.bundleVersionCode = int.Parse(versionNumber);
                PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
#if UNITY_2018_1_OR_NEWER
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
#endif
                AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.Android);
                var instrumentationSettings = getInstrumentationSettings();


                logger.Debug("Starting Android build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = GetScene(),

                    locationPathName = "sampleGame.apk",
                    target = BuildTarget.Android,
                    options = BuildOptions.Development | BuildOptions.IncludeTestAssemblies | BuildOptions.AutoRunPlayer
                };



                AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

                var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
                AltUnityBuilder.RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup.Android);


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
                    "Assets/AltUnityTester/Examples/Scenes/Scene 1 AltUnityDriverTestScene.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 2 Draggable Panel.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 3 Drag And Drop.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 4 No Cameras.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 5 Keyboard Input.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene6.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 7 Drag And Drop NIS.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 8 Draggable Panel NIP.unity",
                    "Assets/AltUnityTester/Examples/Scenes/scene 9 NIS.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 10 Sample NIS.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity"
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
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "fi.altom.altunitytester");
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

                AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.iOS);
                var instrumentationSettings = getInstrumentationSettings();
                AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

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
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.WebGL, "fi.altom.altunitytester");
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

                AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.WebGL);


                var instrumentationSettings = getInstrumentationSettings();
                AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

                var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
                AltUnityBuilder.RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup.WebGL);


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

        private static AltUnityInstrumentationSettings getInstrumentationSettings()
        {
            if (AltUnityTesterEditorWindow.EditorConfiguration == null)
            {
                var instrumentationSettings = new AltUnityInstrumentationSettings();
                var altUnityTesterPort = System.Environment.GetEnvironmentVariable("ALTUNITYTESTER_PORT");

                if (!string.IsNullOrEmpty(altUnityTesterPort)) //server mode
                {
                    instrumentationSettings.AltUnityTesterPort = int.Parse(altUnityTesterPort);
                    return instrumentationSettings;
                }

                var proxyHost = System.Environment.GetEnvironmentVariable("PROXY_HOST");

                if (!string.IsNullOrEmpty(proxyHost)) //proxy mode
                {
                    instrumentationSettings.InstrumentationMode = AltUnityInstrumentationMode.Proxy;
                    instrumentationSettings.ProxyHost = proxyHost;
                }
                var proxyPort = System.Environment.GetEnvironmentVariable("PROXY_PORT");
                if (!string.IsNullOrEmpty(proxyPort))//proxy mode
                {
                    instrumentationSettings.InstrumentationMode = AltUnityInstrumentationMode.Proxy;
                    instrumentationSettings.ProxyPort = int.Parse(proxyPort);
                }

                return instrumentationSettings;
            }

            return AltUnityTesterEditorWindow.EditorConfiguration.GetInstrumentationSettings();
        }
    }
}