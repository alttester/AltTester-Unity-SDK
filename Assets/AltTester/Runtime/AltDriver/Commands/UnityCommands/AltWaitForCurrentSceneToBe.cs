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

using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Logging;

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class AltWaitForCurrentSceneToBe : AltBaseCommand
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        string sceneName;
        double timeout;
        double interval;
        public AltWaitForCurrentSceneToBe(IDriverCommunication commHandler, string sceneName, double timeout, double interval) : base(commHandler)
        {
            this.sceneName = sceneName;
            this.timeout = timeout;
            this.interval = interval;
        }
        public void Execute()
        {
            double time = 0;
            string currentScene = "";
            while (time < timeout)
            {
                currentScene = new AltGetCurrentScene(CommHandler).Execute();
                if (currentScene.Equals(sceneName))
                {
                    return;
                }

                logger.Debug("Waiting for scene to be " + sceneName + "...");
                Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
            }

            if (sceneName.Equals(currentScene))
                return;
            throw new WaitTimeOutException("Scene " + sceneName + " not loaded after " + timeout + " seconds");

        }
    }
}
