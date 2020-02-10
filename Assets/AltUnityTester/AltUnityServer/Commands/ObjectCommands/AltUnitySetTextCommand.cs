namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetTextCommand : AltUnityReflectionMethodsCommand 
    {
        static readonly AltUnityObjectProperty[] TextProperties =
        {
            new AltUnityObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltUnityObjectProperty("TMPro.TMP_InputField", "text", "Unity.TextMeshPro")
        };

        AltUnityObject altUnityObject;
        string inputText;

        public AltUnitySetTextCommand(AltUnityObject altUnityObject, string text)
        {
            this.altUnityObject = altUnityObject;
            this.inputText = text;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("Set text for object by name " + this.altUnityObject.name);
            var response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;

            var targetObject = AltUnityRunner.GetGameObject(altUnityObject);

            foreach (var property in TextProperties)
            {
                try
                {
                    var memberInfo = GetMemberForObjectComponent(altUnityObject, property);
                    response = SetValueForMember(memberInfo, inputText, targetObject, property);
                    if (!response.Contains("error:"))
                        return Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(targetObject));
                }
                catch(Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException e)
                {
                    response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException e)
                {
                    response = AltUnityRunner._altUnityRunner.errorComponentNotFoundMessage;
                }
            }

            return response;
        }
    }
}