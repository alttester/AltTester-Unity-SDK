using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetComponentPropertyCommand : AltUnityReflectionMethodsCommand
    {
        string altObjectString;
        string propertyString;
        int maxDepth;
        public AltUnityGetComponentPropertyCommand(params string[] parameters) : base(parameters, 5)
        {
            this.altObjectString = parameters[2];
            this.propertyString = parameters[3];
            this.maxDepth = int.Parse(parameters[4]);
        }

        public override string Execute()
        {
            LogMessage("get property " + propertyString + " for object " + altObjectString);
            string response = AltUnityErrors.errorPropertyNotFoundMessage;
            AltUnityObjectProperty altProperty = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
            AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
            System.Type type = GetType(altProperty.Component, altProperty.Assembly);
            response = GetValueForMember(altUnityObject, altProperty.Property.Split('.'), type, maxDepth);
            return response;

        }
    }
}
