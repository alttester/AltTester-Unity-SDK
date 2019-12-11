using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class GetScreenshot : AltBaseCommand
{
    int id;
    Assets.AltUnityTester.AltUnityDriver.UnityStruct.Color color;
    float width;
    Vector2 size;
    Vector2 coordinates;
   
    int option = 0;

    public GetScreenshot(SocketSettings socketSettings, Vector2 size) : base(socketSettings)
    {
        this.size = size;
        this.option = 1;
    }
    public GetScreenshot(SocketSettings socketSettings, int id, Assets.AltUnityTester.AltUnityDriver.UnityStruct.Color color, float width, Vector2 size) : base(socketSettings)
    {
        this.size = size;
        this.color = color;
        this.width = width;
        this.id = id;
        this.option = 2;
    }
    public GetScreenshot(SocketSettings socketSettings, Vector2 coordinates, Assets.AltUnityTester.AltUnityDriver.UnityStruct.Color color, float width, Vector2 size) : base(socketSettings)
    {
        this.coordinates = coordinates;
        this.color = color;
        this.width = width;
        this.size = size;
        this.option = 3;
    }
    public TextureInformation Execute()
    {
        switch (option)
        {
            case 2:
                return GetHighlightObjectScreenshot();
            case 3:
                return GetHighlightObjectFromCoordinatesScreenshot();
            default:
                return GetSimpleScreenshot();
        }
    }

    private TextureInformation GetSimpleScreenshot()
    {
        var sizeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(size);
        Socket.Client.Send(toBytes(CreateCommand("getScreenshot", sizeSerialized)));
        return ReceiveImage();
    }
    private TextureInformation GetHighlightObjectScreenshot()
    {
        var sizeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(size);
        var colorAndWidth = color.r + "!!" + color.g + "!!" + color.b + "!!" + color.a + "!-!" + width;
        Socket.Client.Send(toBytes(CreateCommand("hightlightObjectScreenshot", id.ToString(), colorAndWidth, sizeSerialized)));
        return ReceiveImage();
    }
    private TextureInformation GetHighlightObjectFromCoordinatesScreenshot()
    {
        var coordinatesSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(coordinates);
        var sizeSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(size);
        var colorAndWidth = color.r + "!!" + color.g + "!!" + color.b + "!!" + color.a + "!-!" + width;
        Socket.Client.Send(toBytes(CreateCommand("hightlightObjectFromCoordinatesScreenshot", coordinatesSerialized, colorAndWidth, sizeSerialized)));
        return ReceiveImage();
    }
}