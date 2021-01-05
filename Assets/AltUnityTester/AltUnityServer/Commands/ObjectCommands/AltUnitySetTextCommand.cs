using Altom.AltUnityDriver;
using Newtonsoft.Json;

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

        public AltUnitySetTextCommand(params string[] parameters) : base(parameters, 4)
        {
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[2]);
            this.inputText = parameters[3];
        }

        public override string Execute()
        {
            LogMessage("Set text for object by name " + this.altUnityObject.name);
            var response = AltUnityErrors.errorNotFoundMessage;

            var targetObject = AltUnityRunner.GetGameObject(altUnityObject);

            foreach (var property in TextProperties)
            {
                try
                {
                    System.Type type = GetType(property.Component, property.Assembly);
                    response = SetValueForMember(altUnityObject, property.Property.Split('.'), type, inputText);
                    if (!response.Contains("error:"))
                        return Newtonsoft.Json.JsonConvert.SerializeObject(AltUnityRunner._altUnityRunner.GameObjectToAltUnityObject(targetObject));
                }
                catch (PropertyNotFoundException)
                {
                    response = AltUnityErrors.errorPropertyNotFoundMessage;
                }
                catch (ComponentNotFoundException)
                {
                    response = AltUnityErrors.errorComponentNotFoundMessage;
                }
                catch (AssemblyNotFoundException)
                {
                    response = AltUnityErrors.errorAssemblyNotFoundMessage;
                }
            }

            return response;
        }
    }
}