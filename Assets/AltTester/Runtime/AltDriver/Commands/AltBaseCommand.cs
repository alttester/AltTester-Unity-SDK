using System;
using AltTester.AltDriver.Logging;

namespace AltTester.AltDriver.Commands
{
    public class AltBaseCommand
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();

        protected IDriverCommunication CommHandler;

        public AltBaseCommand(IDriverCommunication commHandler)
        {
            this.CommHandler = commHandler;
        }

        protected void ValidateResponse(string expected, string received)
        {
            if (!expected.Equals(received, StringComparison.InvariantCulture))
            {
                throw new AltInvalidServerResponse(expected, received);
            }
        }

    }
}