/*
    Copyright(C) 2024 Altom Consulting

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
    public class AltWaitForObjectWhichContains : AltBaseFindObjects
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        private AltFindObjectWhichContains findObject;
        private readonly string path;
        private readonly double timeout;
        private readonly double interval;

        public AltWaitForObjectWhichContains(IDriverCommunication commHandler, By by, string value, By cameraBy, string cameraValue, bool enabled, double timeout, double interval) : base(commHandler)
        {
            path = SetPath(by, value);
            if (timeout <= 0) throw new ArgumentOutOfRangeException("timeout");
            if (interval <= 0) throw new ArgumentOutOfRangeException("interval");
            this.timeout = timeout;
            this.interval = interval;
            findObject = new AltFindObjectWhichContains(CommHandler, by, value, cameraBy, cameraValue, enabled);
        }
        public AltObject Execute()
        {
            double time = 0;
            AltObject altElement = null;

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
