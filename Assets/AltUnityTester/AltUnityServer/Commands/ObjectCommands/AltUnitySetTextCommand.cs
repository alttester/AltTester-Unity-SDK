namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetTextCommand : AltUnityReflectionMethodsCommand 
    {
        static readonly AltUnityObjectProperty[] TextProperties =
        {
            new AltUnityObjectProperty("UnityEngine.UI.Text", "text"),
            new AltUnityObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltUnityObjectProperty("TMPro.TMP_Text", "text", "Unity.TextMeshPro"),
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
                    System.Type type = GetType(property.Component, property.Assembly);
                    response = SetValueForMember(altUnityObject,property.Property.Split('.'),type,inputText);
                    if (!response.Contains("error:"))
                        return Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(targetObject));
                }
                catch(Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException)
                {
                    response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException)
                {
                    response = AltUnityRunner._altUnityRunner.errorComponentNotFoundMessage;
                }
                catch (Assets.AltUnityTester.AltUnityDriver.AssemblyNotFoundException)
                {
                    response = AltUnityRunner._altUnityRunner.errorAssemblyNotFoundMessage;
                }
            }

            return response;
        }
    }
}