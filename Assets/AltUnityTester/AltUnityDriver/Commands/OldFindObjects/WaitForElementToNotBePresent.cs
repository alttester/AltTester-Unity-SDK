public class WaitForElementToNotBePresentDriver : AltBaseCommand
{
    string name;
    string cameraName;
    bool enabled;
    double timeout;
    double interval;
    public WaitForElementToNotBePresentDriver(SocketSettings socketSettings, string name, string cameraName, bool enabled, double timeout, double interval) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
        this.timeout = timeout;
        this.interval = interval;
    }
    public void Execute()
    {
        double time = 0;
        bool found = false;
        AltUnityObject altElement = null;
        while (time <= timeout)
        {
            found = false;
            try
            {
                altElement = new FindElementDriver(SocketSettings, name, cameraName, enabled).Execute();
                found = true;
                System.Threading.Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
                UnityEngine.Debug.Log("Waiting for element " + name + " to not be present");
            }
            catch (System.Exception)
            {
                break;
            }

        }
        if (found)
            throw new Assets.AltUnityTester.AltUnityDriver.WaitTimeOutException("Element " + name + " still found after " + timeout + " seconds");
    }
}