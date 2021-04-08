using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetTextCommand : AltUnityReflectionMethodsCommand
    {
        static readonly AltUnityObjectProperty[] textProperties =
        {
            new AltUnityObjectProperty("UnityEngine.UI.Text", "text"),
            new AltUnityObjectProperty("UnityEngine.UI.InputField", "text"),
            new AltUnityObjectProperty("TMPro.TMP_Text", "text", "Unity.TextMeshPro"),
            new AltUnityObjectProperty("TMPro.TMP_InputField", "text", "Unity.TextMeshPro")
        };
        readonly AltUnityObject altUnityObject;

        public AltUnityGetTextCommand(params string[] parameters) : base(parameters, 3)
        {
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[2]);
        }

        public override string Execute()
        {
            var response = AltUnityErrors.errorPropertyNotFoundMessage;

            foreach (var property in textProperties)
            {
                try
                {
                    System.Type type = GetType(property.Component, property.Assembly);
                    response = GetValueForMember(altUnityObject, property.Property.Split('.'), type, 2);
                    if (!response.Contains("error:"))
                        break;
                }
                catch (PropertyNotFoundException)
                {
                    response = AltUnityErrors.errorPropertyNotFoundMessage;
                }
                catch (ComponentNotFoundException)
                {
                    response = AltUnityErrors.errorComponentNotFoundMessage;
                }
            }

            return response;
        }
    }
}