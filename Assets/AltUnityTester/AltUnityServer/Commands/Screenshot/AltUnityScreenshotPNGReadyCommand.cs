using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class AltUnityScreenshotPNGReadyCommand : AltUnityCommand
    {
        byte[] screenshotData;

        public AltUnityScreenshotPNGReadyCommand (byte[] screenshotData)
        {
            this.screenshotData = screenshotData;
        }

        public override string Execute()
        {
        return Encoding.ASCII.GetString(screenshotData);
        }
    }

