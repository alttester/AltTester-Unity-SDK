using System;
using System.Threading;
using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityWaitForObject : AltUnityBaseFindObjects
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        AltUnityFindObject findObject;
        string path;
        double timeout;
        double interval;

        public AltUnityWaitForObject(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraValue, bool enabled, double timeout, double interval) : base(commHandler)
        {
            findObject = new AltUnityFindObject(CommHandler, by, value, cameraBy, cameraValue, enabled);
            path = SetPath(by, value);
            this.timeout = timeout;
            this.interval = interval;
            if (timeout <= 0) throw new ArgumentOutOfRangeException("timeout");
            if (interval <= 0) throw new ArgumentOutOfRangeException("interval");
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