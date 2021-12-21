using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetComponentPropertyCommand : AltUnityReflectionMethodsCommand<AltUnityGetObjectComponentPropertyParams, object>
    {
        public AltUnityGetComponentPropertyCommand(AltUnityGetObjectComponentPropertyParams cmdParams) : base(cmdParams)
        {
        }

        public override object Execute()
        {
            System.Type type = GetType(CommandParams.component, CommandParams.assembly);
            System.Reflection.MemberInfo memberInfo;
            var propertySplited = CommandParams.property.Split('.');
            string propertyName;
            object response;
            if (CommandParams.altUnityObject != null)
                response = GetValueForMember(CommandParams.altUnityObject, propertySplited, type);
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

                response = GetValue(instance, memberInfo, -1);
            }
            return response;
        }
    }
}
