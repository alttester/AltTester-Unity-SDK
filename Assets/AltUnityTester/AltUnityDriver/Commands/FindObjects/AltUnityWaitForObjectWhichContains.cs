using System;
using System.Threading;
using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityWaitForObjectWhichContains : AltUnityBaseFindObjects
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        private AltUnityFindObjectWhichContains findObject;
        private readonly string path;
        private readonly double timeout;
        private readonly double interval;

        public AltUnityWaitForObjectWhichContains(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraValue, bool enabled, double timeout, double interval) : base(commHandler)
        {
            path = SetPath(by, value);
            if (timeout <= 0) throw new ArgumentOutOfRangeException("timeout");
            if (interval <= 0) throw new ArgumentOutOfRangeException("interval");
            this.timeout = timeout;
            this.interval = interval;
            findObject = new AltUnityFindObjectWhichContains(CommHandler, by, value, cameraBy, cameraValue, enabled);
        }
        public AltUnityObject Execute()
        {
            double time = 0;
            AltUnityObject altElement = null;

            logger.Debug("Waiting for element " + path + " to be present.");
            while (time < timeout)
            {
                try
                {
                    altElement = findObject.Execute();
                    break;
                }
                catch (NotFoundException)
                {
                    Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                    time += interval;
                }
            }
            if (altElement != null)
                return altElement;
            throw new WaitTimeOutException("Element " + path + " not loaded after " + timeout + " seconds");
        }
    }
}