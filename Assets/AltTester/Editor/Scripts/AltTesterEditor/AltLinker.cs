/*
    Copyright(C) 2026 Altom Consulting
*/

using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.UnityLinker;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Editor
{
    /// <summary>
    /// Keeps the code AltTester® reaches through reflection from being stripped in instrumented builds: the AltTester®
    /// assemblies and NLog, the TextMeshPro members used by the get/set text commands, and the System.Core expression
    /// interpreter. Builds without ALTTESTER get an empty link.xml, so they don't carry this code.
    /// </summary>
    internal class AltLinker : IUnityLinkerProcessor
    {
        // The AltTester® assemblies are listed under both names, all with ignoreIfMissing: the released package ships
        // precompiled modules, and the source project (sdk-package) builds them from assembly definitions. Without
        // them, Managed Stripping Level Low and higher removes the constructors of the command parameter classes and
        // AltAxis, which are created from JSON through reflection.
        private const string instrumentedBuildLinkXml =
@"<linker>
    <assembly fullname=""AltTesterCommandsModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTesterCommonModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTesterCommunicationModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTesterDriverModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTesterInputModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTesterLoggingModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTesterProxyModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTesterUIModule"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTester.AltTesterUnitySDK"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""AltTester.AltTesterUnitySDK.Driver"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""NLog"" preserve=""all"" ignoreIfMissing=""1"" />
    <assembly fullname=""System.Core"">
        <type fullname=""System.Linq.Expressions.Interpreter.LightLambda"" preserve=""all"" />
    </assembly>
    <assembly fullname=""Unity.TextMeshPro"" ignoreIfMissing=""1"">
        <type fullname=""TMPro.TMP_Text"">
            <property name=""text"" />
        </type>
        <type fullname=""TMPro.TMP_InputField"">
            <property name=""text"" />
            <property name=""onValueChanged"" />
            <property name=""onSubmit"" />
            <property name=""onEndEdit"" />
        </type>
    </assembly>
</linker>
";

        public int callbackOrder => 0;

        public string GenerateAdditionalLinkXmlFile(BuildReport report, UnityLinkerBuildPipelineData data)
        {
            var linkXmlPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Temp", "AltTesterLink.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(linkXmlPath));
            File.WriteAllText(linkXmlPath, AltBuildInstrumentation.IsInstrumentedBuild(data.target) ? instrumentedBuildLinkXml : "<linker />\n");
            return linkXmlPath;
        }
    }
}
