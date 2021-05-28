using System;
using System.Threading;
using Altom.AltUnityDriver.Logging;
using NLog;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityWaitForObjectWithText : AltUnityBaseFindObjects
    {
        readonly Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        private AltUnityFindObject findObject;
        private readonly string path;


        private readonly string text;

        private readonly double timeout;
        private readonly double interval;

        public AltUnityWaitForObjectWithText(IDriverCommunication commHandler, By by, string value, string text, By cameraBy, string cameraPath, bool enabled, double timeout, double interval) : base(commHandler)
        {
            this.findObject = new AltUnityFindObject(CommHandler, by, value, cameraBy, cameraPath, enabled);

            path = SetPath(by, value);
            if (timeout <= 0) throw new ArgumentOutOfRangeException("timeout");
            if (interval <= 0) throw new ArgumentOutOfRangeException("interval");
            this.timeout = timeout;
            this.interval = interval;
            this.text = text;
        }
        public AltUnityObject Execute()
        {
            double time = 0;
            AltUnityObject altElement = null;
            while (time < timeout)
            {
                try
                {
                    altElement = findObject.Execute();
                    if (altElement.GetText().Equals(text))
                        break;

                    else
                    {
                        logger.Debug("Waiting for element " + path + " to have text " + text);
                        Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                        time += interval;
                    }
                }
                catch (NotFoundException)
                {
                    logger.Debug("Waiting for element " + path + " to be present.");
                    Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                    time += interval;
                }

            }
            if (altElement != null && altElement.GetText().Equals(text))
            {
                return altElement;
            }
            throw new WaitTimeOutException("Element with text: " + text + " not loaded after " + timeout + " seconds");
        }
    }
}