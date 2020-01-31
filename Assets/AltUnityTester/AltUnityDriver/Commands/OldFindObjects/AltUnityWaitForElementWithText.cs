public class AltUnityWaitForElementWithText : AltBaseCommand
{
    string name;
    string cameraName;
    string text;
    bool enabled;
    double timeout;
    double interval;

    public AltUnityWaitForElementWithText(SocketSettings socketSettings, string name, string text, string cameraName, bool enabled, double timeout, double interval) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
        this.timeout = timeout;
        this.interval = interval;
        this.text = text;
    }
    public AltUnityObject Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout)
        {
            try
            {
                altElement =new AltUnityFindElement(SocketSettings, name, cameraName, enabled).Execute();
                if (altElement.GetText().Equals(text))
                    break;
                throw new System.Exception("Not the wanted text");
            }
            catch (System.Exception)
            {
                System.Threading.Thread.Sleep(System.Convert.ToInt32(interval * 1000));
                time += interval;
                System.Diagnostics.Debug.WriteLine("Waiting for element " + name + " to have text " + text);
            }
        }
        if (altElement != null && altElement.GetText().Equals(text))
        {
            return altElement;
        }
        throw new Assets.AltUnityTester.AltUnityDriver.WaitTimeOutException("Element with text: " + text + " not loaded after " + timeout + " seconds");
        
    }
}