/*
    Copyright(C) 2026 Altom Consulting
*/

using System;

namespace AltTester.AltTesterSDK.Driver
{
    /// <summary>
    /// Settings for AltTester® Unity App instrumentation.
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

        public bool ResetConnectionData = false;
        public string UID = "";

        public bool hideGreenPopup = false;

        /// <summary>
        ///  Gets or sets a value indicating whether to use secure WebSocket connection (WSS).
        /// </summary>
        public bool SecureMode = false;

        /// <summary>
        /// Gets or sets a value indicating whether to show the native popup at startup on Android and iOS platforms. This popup allows changing connection data through Appium.
        /// </summary>
        public bool ShowNativePopup = false;
    }
}
