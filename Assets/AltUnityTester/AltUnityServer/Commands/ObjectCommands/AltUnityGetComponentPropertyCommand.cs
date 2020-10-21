namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetComponentPropertyCommand :AltUnityReflectionMethodsCommand 
    {
        string altObjectString;
        string propertyString;
        int maxDepth;
        public AltUnityGetComponentPropertyCommand (string altObjectString, string propertyString,int maxDepth)
        {
            this.altObjectString = altObjectString;
            this.propertyString = propertyString;
            this.maxDepth = maxDepth;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("get property " + propertyString + " for object " + altObjectString);
            string response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
            AltUnityObjectProperty altProperty = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
            AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
            System.Type type = GetType(altProperty.Component, altProperty.Assembly);
            response = GetValueForMember(altUnityObject, altProperty.Property.Split('.'), type, maxDepth);
            return response;
                
        }
    }
}
