public class AltUnityWaitForCurrentSceneToBe : AltBaseCommand
{
    string sceneName;
    double timeout;
    double interval;
    public AltUnityWaitForCurrentSceneToBe(SocketSettings socketSettings, string sceneName, double timeout, double interval) : base(socketSettings)
    {
        this.sceneName = sceneName;
        this.timeout = timeout;
        this.interval = interval;
    }
    public string Execute()
    {
        double time = 0;
        string currentScene = "";
        while (time < timeout)
        {
            currentScene = new AltUnityGetCurrentScene(SocketSettings).Execute();
            if (!currentScene.Equals(sceneName))
            {
                System.Diagnostics.Debug.WriteLine("Waiting for scene to be " + sceneName + "...");
                System.Threading.Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
            }
            else
            {
                break;
            }
        }

        if (sceneName.Equals(currentScene))
            return currentScene;
        throw new Assets.AltUnityTester.AltUnityDriver.WaitTimeOutException("Scene " + sceneName + " not loaded after " + timeout + " seconds");

    }
}