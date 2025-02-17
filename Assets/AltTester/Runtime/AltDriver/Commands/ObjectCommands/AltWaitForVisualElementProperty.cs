/*
    Copyright(C) 2025 Altom Consulting

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
using Newtonsoft.Json.Linq;

namespace AltTester.AltTesterUnitySDK.Driver.Commands
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
        int maxDepth;

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
            JToken jTokenPropertyFound = "";
            string strPropertyValue = "";
            while (time < timeout)
            {
                logger.Debug($"Waiting for property {propertyName} to be {propertyValue}.");
                T propertyFound = altObject.GetVisualElementProperty<T>(propertyName);
                if (propertyFound == null && propertyValue == null) //avoid null reference exception
                    return propertyFound;
                if (!getPropertyAsString && propertyFound.Equals(propertyValue))
                    return propertyFound;
                strPropertyValue = propertyValue.ToString() == "" ? "null" : propertyValue.ToString();
                jTokenPropertyFound = propertyFound == null ? "null" : propertyFound as JToken;
                if (getPropertyAsString && jTokenPropertyFound.ToString().Equals(strPropertyValue))
                    return propertyFound;

                Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
            }
            throw new WaitTimeOutException($"Property {propertyName} was {jTokenPropertyFound} and was not {strPropertyValue} after {timeout} seconds");
        }
    }
}
