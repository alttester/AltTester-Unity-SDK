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
            System.Reflection.MemberInfo memberInfo;
            var propertySplited = CommandParams.property.Split('.');
            string propertyName;
            string response;
            if (CommandParams.altUnityObject != null)
                response = GetValueForMember(CommandParams.altUnityObject, propertySplited, type, CommandParams.maxDepth);
            else
            {
                var instance = GetInstance(null, propertySplited, type);
                if (propertySplited.Length > 1)
                    propertyName = propertySplited[propertySplited.Length - 1];
                else
                    propertyName = CommandParams.property;

                if (instance == null)
                    memberInfo = GetMemberForObjectComponent(type, propertyName);
                else
                    memberInfo = GetMemberForObjectComponent(instance.GetType(), propertyName);

                object value = GetValue(instance, memberInfo, -1);
                response = SerializeMemberValue(value, CommandParams.maxDepth);
            }
            return response;
        }
    }
}
