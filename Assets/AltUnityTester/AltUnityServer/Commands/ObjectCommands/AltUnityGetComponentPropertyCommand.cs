using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetComponentPropertyCommand : AltUnityReflectionMethodsCommand<AltUnityGetObjectComponentPropertyParams, string>
    {
        public AltUnityGetComponentPropertyCommand(AltUnityGetObjectComponentPropertyParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            System.Type type = GetType(CommandParams.component, CommandParams.assembly);
            string response = GetValueForMember(CommandParams.altUnityObject, CommandParams.property.Split('.'), type, CommandParams.maxDepth);
            return response;
        }
    }
}
