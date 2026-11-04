/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;

namespace AltTester.AltTesterSDK.Driver.Communication
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
