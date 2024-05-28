/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetComponentPropertyCommand : AltReflectionMethodsCommand<AltGetObjectComponentPropertyParams, object>
    {
        public AltGetComponentPropertyCommand(AltGetObjectComponentPropertyParams cmdParams) : base(cmdParams)
        {
        }

        public override object Execute()
        {
            System.Type type = GetType(CommandParams.component, CommandParams.assembly);
            System.Reflection.MemberInfo memberInfo;
            var propertySplited = CommandParams.property.Split('.');
            string propertyName;
            object response;
            if (CommandParams.altObject != null)
                response = GetValueForMember(CommandParams.altObject, propertySplited, type);
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
