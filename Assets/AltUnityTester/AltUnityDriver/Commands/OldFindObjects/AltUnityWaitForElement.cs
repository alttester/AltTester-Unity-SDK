public class AltUnityWaitForElement : AltBaseCommand
{
    string name;
    string cameraName;
    bool enabled;
    double timeout;
    double interval;
    public AltUnityWaitForElement(SocketSettings socketSettings, string name, string cameraName, bool enabled, double timeout, double interval) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
        this.timeout = timeout;
        this.interval = interval;
    }
    public AltUnityObject Execute()
    {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout)
        {
            try
            {
                altElement = new AltUnityFindElement(SocketSettings, name, cameraName, enabled).Execute();
                break;
            }
            catch (System.Exception)
            {
                System.Threading.Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
                System.Diagnostics.Debug.WriteLine("Waiting for element " + name + "...");
            }
        }
        if (altElement != null)
        {
            return altElement;
        }
        throw new Assets.AltUnityTester.AltUnityDriver.WaitTimeOutException("Element " + name + " not loaded after " + timeout + " seconds");
    }
}