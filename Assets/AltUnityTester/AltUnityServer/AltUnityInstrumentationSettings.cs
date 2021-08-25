using System;

namespace Altom.AltUnity.Instrumentation
{
    /// <summary>
    /// Instrument your unity app  Proxy or Server mode
    /// </summary>
    public enum AltUnityInstrumentationMode
    {
        Server = 0,
        Proxy = 1,
    }

    /// <summary>
    /// AltUnity Unity App Instrumentation settings
    /// </summary>
    [Serializable]
    public class AltUnityInstrumentationSettings
    {
        /// <summary>
        /// InstrumentationMode
        /// In Server mode the Instrumented Unity App will listen for connections from driver on defined  `ServerPort`
        /// In Proxy mode the Instrumented Unity App will connect to proxy using given `ProxyHost` and `ProxyPort` 
        /// </summary>
        public AltUnityInstrumentationMode InstrumentationMode = AltUnityInstrumentationMode.Server;

        /// <summary>
        /// The port on which the Instrumented Unity App is listening for driver. Used only in Server instrumentation mode
        /// </summary>
        public int ServerPort = 13000;

        /// <summary>
        /// The proxy host to which the Instrumented Unity App will connect to. Used only in Proxy instrumentation mode
        /// </summary>
        public string ProxyHost = "127.0.0.1";

        /// <summary>
        /// The proxy port to which the Instrumented Unity App will connect to. Used only in Proxy instrumentation mode
        /// </summary>
        public int ProxyPort = 13000;


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