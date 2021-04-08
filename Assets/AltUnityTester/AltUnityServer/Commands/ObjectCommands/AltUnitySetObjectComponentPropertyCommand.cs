using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetObjectComponentPropertyCommand : AltUnityReflectionMethodsCommand
    {
        private readonly AltUnityObject altUnityObject;
        private readonly AltUnityObjectProperty altProperty;
        readonly string valueString;

        public AltUnitySetObjectComponentPropertyCommand(params string[] parameters) : base(parameters, 5)
        {
            altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(parameters[2]);
            altProperty =
               Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(parameters[3]);
            this.valueString = parameters[4];
        }

        public override string Execute()
        {
            System.Type type = GetType(altProperty.Component, altProperty.Assembly);
            string response = SetValueForMember(altUnityObject, altProperty.Property.Split('.'), type, valueString);

            return response;
        }
    }
}
