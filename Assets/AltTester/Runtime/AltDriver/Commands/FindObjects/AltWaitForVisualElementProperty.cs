/*
    Copyright(C) 2026 Altom Consulting
*/

using System;
using System.Threading;
using AltTester.AltTesterSDK.Driver.Logging;
using Newtonsoft.Json.Linq;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltWaitForVisualElementProperty<T> : AltBaseFindObjects
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        AltObject altObject;
        string propertyName;
        T propertyValue;
        bool getPropertyAsString;
        double timeout;
        double interval;

        public AltWaitForVisualElementProperty(IDriverCommunication commHandler, string propertyName, T propertyValue, double timeout, double interval, bool getPropertyAsString, AltObject altObject) : base(commHandler)
        {
            this.altObject = altObject;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.timeout = timeout;
            this.interval = interval;
            this.getPropertyAsString = getPropertyAsString;
            if (timeout <= 0) throw new ArgumentOutOfRangeException("timeout");
            if (interval <= 0) throw new ArgumentOutOfRangeException("interval");
        }
        public T Execute()
        {
            double time = 0;
            string strPropertyValue = "";
            string propertyFoundString = "";
            while (time < timeout)
            {
                logger.Debug($"Waiting for property {propertyName} to be {propertyValue}.");
                T propertyFound = altObject.GetVisualElementProperty<T>(propertyName);
                if (propertyFound == null && propertyValue == null) //avoid null reference exception
                    return propertyFound;
                if (!getPropertyAsString && propertyFound.Equals(propertyValue))
                    return propertyFound;
                strPropertyValue = propertyValue.ToString() == "" ? "null" : propertyValue.ToString();
                JToken jTokenPropertyFound = propertyFound == null ? "null" : propertyFound as JToken;
                propertyFoundString = propertyFound.ToString();
                if (getPropertyAsString && jTokenPropertyFound.ToString().Equals(strPropertyValue))
                    return propertyFound;

                Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
            }
            throw new WaitTimeOutException($"Property {propertyName} was {propertyFoundString} and was not {strPropertyValue} after {timeout} seconds");
        }
    }
}
