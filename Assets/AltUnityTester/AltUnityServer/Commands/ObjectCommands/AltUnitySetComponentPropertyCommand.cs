using Altom.AltUnityDriver.Commands;


namespace Altom.AltUnityTester.Commands
{
    class AltUnitySetComponentPropertyCommand : AltUnityReflectionMethodsCommand<AltUnitySetObjectComponentPropertyParams, string>
    {
        public AltUnitySetComponentPropertyCommand(AltUnitySetObjectComponentPropertyParams cmdParams) : base(cmdParams)
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
