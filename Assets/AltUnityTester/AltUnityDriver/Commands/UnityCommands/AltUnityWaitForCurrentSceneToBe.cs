using System.Threading;
using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityWaitForCurrentSceneToBe : AltBaseCommand
    {
        readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        string sceneName;
        double timeout;
        double interval;
        public AltUnityWaitForCurrentSceneToBe(IDriverCommunication commHandler, string sceneName, double timeout, double interval) : base(commHandler)
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
                currentScene = new AltUnityGetCurrentScene(CommHandler).Execute();
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