/*
    Copyright(C) 2023 Altom Consulting

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
