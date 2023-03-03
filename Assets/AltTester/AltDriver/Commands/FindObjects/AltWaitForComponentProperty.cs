using System;
using System.Threading;
using AltTester.AltDriver.Logging;

namespace AltTester.AltDriver.Commands
{
    public class AltWaitForComponentProperty<T> : AltBaseFindObjects
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        AltObject obj;
        T propertyFound;
        string componentName;
        string propertyName;
        T propertyValue;
        string assambly;
        string path;
        double timeout;
        double interval;

        public AltWaitForComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, T propertyValue, string assemblyName, double timeout, double interval, AltObject obj) : base(commHandler)
        {

            this.componentName = componentName;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.assambly = assemblyName;
            this.timeout = timeout;
            this.interval = interval;
            if (timeout <= 0) throw new ArgumentOutOfRangeException("timeout");
            if (interval <= 0) throw new ArgumentOutOfRangeException("interval");
        }
        public T Execute()
        {
            double time = 0;

            logger.Debug("Waiting for element " + path + " to be present.");
            while (time < timeout)
            {
                try
                {
                    propertyFound = obj.GetComponentProperty<T>(componentName, propertyName, assambly);
                    if (propertyFound.Equals(propertyValue))
                        break;
                }
                catch (NotFoundException)
                {
                    Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                    time += interval;
                }
            }
            if (propertyFound != null)
                return propertyFound;
            throw new WaitTimeOutException("Property " + propertyName + " not loaded after " + timeout + " seconds");
        }
    }
}