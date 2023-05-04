using System;

namespace AltTester.AltTesterUnitySDK
{
    /// <summary>
    /// Settings for AltTester Unity App instrumentation.
    /// </summary>
    [Serializable]
    public class AltInstrumentationSettings
    {
        /// <summary>
        /// Gets or sets the host that the instrumented Unity App will connect to.
        /// </summary>
        public string AltServerHost = "127.0.0.1";

        /// <summary>
        /// Gets or sets the port that the instrumented Unity App will connect to.
        /// </summary>
        public int AltServerPort = 13000;

        /// <summary>
        /// Gets or sets the name of the app that the instrumented Unity App will use as a unique identifier.
        /// </summary>
        public string AppName = "__default__";

        /// <summary>
        /// Gets or sets a value indicating whether to show where an action happens on the screen (e.g. swipe or click).
        /// </summary>
        public bool InputVisualizer = true;

        /// <summary>
        /// Gets or sets a value indicating whether to display the AltTester popup in the instrumented Unity App.
        /// </summary>
        public bool ShowPopUp = true;
    }
}
