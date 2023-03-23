using AltTester.AltTesterUnitySdk.Driver.Commands;


namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltSetComponentPropertyCommand : AltReflectionMethodsCommand<AltSetObjectComponentPropertyParams, string>
    {
        public AltSetComponentPropertyCommand(AltSetObjectComponentPropertyParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            System.Type type = GetType(CommandParams.component, CommandParams.assembly);
            return SetValueForMember(CommandParams.altObject, CommandParams.property.Split('.'), type, CommandParams.value);
        }
    }
}
