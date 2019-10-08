public class GetText : AltBaseCommand
{
  AltUnityObject altUnityObject;
  
  public GetText(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
  {
    this.altUnityObject = altUnityObject;
  }
  
  public string Execute()
  {
    string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
    Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("getText", altObject)));
    string data = Recvall();
    if (!data.Contains("error:")) return data;
    HandleErrors(data);
    return null;
  }
}