public class AltUnityWaitForObjectWhichContains : AltUnityBaseFindObjects
{
    By by;
    string value;
    string cameraName;
    bool enabled;
    double timeout;
    double interval;

    public AltUnityWaitForObjectWhichContains (SocketSettings socketSettings, By by, string value, string cameraName, bool enabled, double timeout, double interval) : base(socketSettings)
    {
        this.by = by;
        this.value = value;
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
                altElement = new AltUnityFindObjectWhichContains(SocketSettings, by, value, cameraName, enabled).Execute();
                break;
            }
            catch (System.Exception)
            {
                System.Diagnostics.Debug.WriteLine("Waiting for element where name contains " + value + "....");
                System.Threading.Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
            }
        }
        if (altElement != null)
            return altElement;
        throw new Assets.AltUnityTester.AltUnityDriver.WaitTimeOutException("Element " + value + " not loaded after " + timeout + " seconds");
    }
    
}

