using System;

namespace AltTester
{
    /// <summary>
    /// Unity App Instrumentation settings for AltTester
    /// </summary>
    [Serializable]
    public class AltInstrumentationSettings
    {
        /// <summary>
        /// The proxy host to which the Instrumented Unity App will connect to.
        /// </summary>
        public string AltServerHost = "127.0.0.1";

        /// <summary>
        /// The proxy port to which the Instrumented Unity App will connect to.
        /// </summary>
        public int AltServerPort = 13000;

        public string AppName = "__default__";

        /// <summary>
        /// If true, it will show where an action happens on screen (e.g. swipe or click).
        /// </summary>
        public bool InputVisualizer = true;

        /// <summary>
        /// If true, it will display the `AltTester` popup in Instrumented Unity App.
        /// </summary>
        public bool ShowPopUp = true;

    }
}