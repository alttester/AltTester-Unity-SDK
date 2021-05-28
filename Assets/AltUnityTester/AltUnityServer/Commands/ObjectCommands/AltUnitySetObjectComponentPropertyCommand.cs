using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;


namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetObjectComponentPropertyCommand : AltUnityReflectionMethodsCommand<AltUnitySetObjectComponentPropertyParams, string>
    {


        public AltUnitySetObjectComponentPropertyCommand(AltUnitySetObjectComponentPropertyParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            System.Type type = GetType(CommandParams.component, CommandParams.assembly);
            string response = SetValueForMember(CommandParams.altUnityObject, CommandParams.property.Split('.'), type, CommandParams.value);

            return response;
        }
    }
}
