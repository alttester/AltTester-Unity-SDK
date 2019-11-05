namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class SetTextCommand : ReflectionMethodsCommand
    {
        static readonly AltUnityObjectProperty[] TextProperties =
        {
            new AltUnityObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltUnityObjectProperty("TMPro.TMP_InputField", "text", "Unity.TextMeshPro")
        };

        AltUnityObject altUnityObject;
        string inputText;

        public SetTextCommand(AltUnityObject altUnityObject, string text)
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
                var memberInfo = GetMemberForObjectComponent(altUnityObject, property);
                response = SetValueForMember(memberInfo, inputText, targetObject, property);
                if (!response.Contains("error:"))
                    return Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(targetObject));
            }

            return response;
        }
    }
}