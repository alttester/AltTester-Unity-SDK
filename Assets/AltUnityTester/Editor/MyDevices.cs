public  class MyDevices
{
    public string DeviceId { get; set; }
    public int LocalPort { get; set; }
    public int RemotePort { get; set; }
    public bool Active { get; set; }
    public Platform Platform { get; set; }
    public MyDevices(string deviceId, int localPort=13000, int remotePort=13000,bool active=false,Platform platform=Platform.Android)
    {
        DeviceId = deviceId;
        LocalPort = localPort;
        RemotePort = remotePort;
        Active = active;
        Platform = platform;
    }

   
}