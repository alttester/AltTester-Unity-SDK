public  class AltUnityMyDevices
{
    public string DeviceId { get; set; }
    public int LocalPort { get; set; }
    public int RemotePort { get; set; }
    public bool Active { get; set; }
    public AltUnityPlatform Platform { get; set; }
    public int Pid{ get; set;}
    public AltUnityMyDevices(string deviceId, int localPort=13000, int remotePort=13000,bool active=false,AltUnityPlatform platform=AltUnityPlatform.Android,int pid=0)
    {
        DeviceId = deviceId;
        LocalPort = localPort;
        RemotePort = remotePort;
        Active = active;
        Platform = platform;
        Pid=pid;
    }

   
}