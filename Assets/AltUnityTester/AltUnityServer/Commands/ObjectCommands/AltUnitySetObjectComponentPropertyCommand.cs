namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetObjectComponentPropertyCommand :AltUnityReflectionMethodsCommand 
    {
        string altObjectString;
        string propertyString;
        string valueString;

        public AltUnitySetObjectComponentPropertyCommand (string altObjectString, string propertyString, string valueString)
        {
            this.altObjectString = altObjectString;
            this.propertyString = propertyString;
            this.valueString = valueString;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("set property " + propertyString + " to value: " + valueString + " for object " + altObjectString);
            string response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
            AltUnityObjectProperty altProperty =
                Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
            AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
            System.Type type = GetType(altProperty.Component, altProperty.Assembly);
            response = SetValueForMember(altUnityObject, altProperty.Property.Split('.'), type, valueString);
            return response;
        }
    }
}
