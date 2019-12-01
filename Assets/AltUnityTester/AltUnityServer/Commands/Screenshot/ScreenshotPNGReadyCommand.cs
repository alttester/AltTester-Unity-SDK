using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class ScreenshotPNGReadyCommand: Command
    {
        byte[] screenshotData;

        public ScreenshotPNGReadyCommand(byte[] screenshotData)
        {
            this.screenshotData = screenshotData;
        }

        public override string Execute()
        {
        return Encoding.ASCII.GetString(screenshotData);
        }
    }

