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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetAllMethodsCommand : AltReflectionMethodsCommand<AltGetAllMethodsParams, List<string>>
    {
        public AltGetAllMethodsCommand(AltGetAllMethodsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<string> Execute()
        {
            Type type = GetType(CommandParams.altComponent.componentName, CommandParams.altComponent.assemblyName);
            MethodInfo[] methodInfos = new MethodInfo[1];
            switch (CommandParams.methodSelection)
            {
                case AltMethodSelection.CLASSMETHODS:
                    methodInfos = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
                case AltMethodSelection.INHERITEDMETHODS:
                    var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var classMethods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    methodInfos = allMethods.Except(classMethods).ToArray();
                    break;
                case AltMethodSelection.ALLMETHODS:
                    methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
            }

            var listMethods = new List<string>();

            foreach (var methodInfo in methodInfos)
            {
                listMethods.Add(methodInfo.ToString());
            }
            return listMethods;
        }
    }
}
