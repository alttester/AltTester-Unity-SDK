using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetComponentPropertyCommand : AltUnityReflectionMethodsCommand
    {
        private readonly AltUnityObject altUnityObject;
        private readonly AltUnityObjectProperty altProperty;
        readonly int maxDepth;
        public AltUnityGetComponentPropertyCommand(params string[] parameters) : base(parameters, 5)
        {
            this.altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(parameters[2]);
            altProperty = JsonConvert.DeserializeObject<AltUnityObjectProperty>(parameters[3]);
            this.maxDepth = JsonConvert.DeserializeObject<int>(parameters[4]);
        }

        public override string Execute()
        {
            System.Type type = GetType(altProperty.Component, altProperty.Assembly);
            string response = GetValueForMember(altUnityObject, altProperty.Property.Split('.'), type, maxDepth);
            return response;
        }
    }
}
