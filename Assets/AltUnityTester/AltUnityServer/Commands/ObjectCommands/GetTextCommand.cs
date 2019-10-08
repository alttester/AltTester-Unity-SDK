namespace Assets.AltUnityTester.AltUnityServer.Commands
{
  class GetTextCommand : ReflectionMethodsCommand
  {
    static readonly AltUnityObjectProperty[] TextProperties =
    {
      new AltUnityObjectProperty("UnityEngine.UI.Text", "text"),
      new AltUnityObjectProperty("TMPro.TMP_Text", "text", "Unity.TextMeshPro")
    };
    
    AltUnityObject altUnityObject;

    public GetTextCommand(AltUnityObject altUnityObject)
    {
      this.altUnityObject = altUnityObject;
    }

    public override string Execute()
    {
      UnityEngine.Debug.Log("Get text from object by name " + this.altUnityObject.name);
      var response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
      
      var targetObject = AltUnityRunner.GetGameObject(altUnityObject);

      foreach (var property in TextProperties)
      {
        var memberInfo = GetMemberForObjectComponent(altUnityObject, property);
        response = GetValueForMember(memberInfo, targetObject, property);
        if (!response.Contains("error:"))
          break;
      }
      
      return response;
    }
  }
}