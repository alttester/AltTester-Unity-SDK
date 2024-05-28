/*
    Copyright(C) 2024 Altom Consulting

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

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Driver.Communication
{

    public class LiveUpdateDriver : BaseDriver
    {
        public LiveUpdateDriver(string path) : base(path)
        {
        }
        public void Start()
        {
            this.WsClient.Send("Start");
            this.isRunning = true;
        }

        public void Stop()
        {
            this.WsClient.Send("Stop");
            this.isRunning = false;
        }

        public void UpdateFrameRate(int frameRate)
        {
            this.WsClient.Send(string.Format("FrameRate:{0}", frameRate));
        }

        public void UpdateQuality(int quality)
        {
            this.WsClient.Send(string.Format("Quality:{0}", quality));
        }
    }
}
