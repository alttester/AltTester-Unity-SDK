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
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Logging;

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltWaitForComponentProperty<T> : AltBaseFindObjects
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        AltObject altObject;
        string componentName;
        string propertyName;
        T propertyValue;
        string assambly;
        double timeout;
        double interval;

        public AltWaitForComponentProperty(IDriverCommunication commHandler, string componentName, string propertyName, T propertyValue, string assemblyName, double timeout, double interval, AltObject altObject) : base(commHandler)
        {
            this.altObject = altObject;
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
            T propertyFound = default(T);

            logger.Debug("Waiting for property " + propertyName + " to be present.");
            while (time < timeout)
            {
                try
                {
                    propertyFound = altObject.GetComponentProperty<T>(componentName, propertyName, assambly);
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
