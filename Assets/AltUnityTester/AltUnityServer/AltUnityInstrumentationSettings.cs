using System;

namespace Altom.AltUnityTester
{
    public enum AltUnityInstrumentationMode
    {
        Server,
        Proxy
    }

    /// <summary>
    /// AltUnity Unity App Instrumentation settings
    /// </summary>
    [Serializable]
    public class AltUnityInstrumentationSettings
    {
        public AltUnityInstrumentationMode InstrumentationMode = AltUnityInstrumentationMode.Server;

        /// <summary>
        /// The proxy host to which the Instrumented Unity App will connect to. Used only in Proxy instrumentation mode
        /// </summary>
        public string ProxyHost = "127.0.0.1";

        /// <summary>
        /// The proxy port to which the Instrumented Unity App will connect to. Used only in Proxy instrumentation mode
        /// </summary>
        public int ProxyPort = 13000;

        /// <summary>
        /// The port AltUnity Tester is listening on for connections from driver.
        /// </summary>
        public int AltUnityTesterPort = 13000;


        /// <summary>
        /// If true, it will show where an action happens on screen ( e.g. swipe or clikc )
        /// </summary>
        public bool InputVisualizer = true;
        /// <summary>
        /// If true, it will display the `AltUnityTester` popup in Instrumented Unity App
        /// </summary>
        public bool ShowPopUp = true;

    }
}