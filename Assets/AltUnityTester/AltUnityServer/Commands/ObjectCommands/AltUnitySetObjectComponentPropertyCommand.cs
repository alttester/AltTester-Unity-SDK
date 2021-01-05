using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetObjectComponentPropertyCommand : AltUnityReflectionMethodsCommand
    {
        string altObjectString;
        string propertyString;
        string valueString;

        public AltUnitySetObjectComponentPropertyCommand(params string[] parameters) : base(parameters, 5)
        {
            this.altObjectString = parameters[2];
            this.propertyString = parameters[3];
            this.valueString = parameters[4];
        }

        public override string Execute()
        {
            LogMessage("set property " + propertyString + " to value: " + valueString + " for object " + altObjectString);
            string response = AltUnityErrors.errorPropertyNotFoundMessage;
            AltUnityObjectProperty altProperty =
                Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
            AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
            System.Type type = GetType(altProperty.Component, altProperty.Assembly);
            response = SetValueForMember(altUnityObject, altProperty.Property.Split('.'), type, valueString);
            return response;
        }
    }
}
