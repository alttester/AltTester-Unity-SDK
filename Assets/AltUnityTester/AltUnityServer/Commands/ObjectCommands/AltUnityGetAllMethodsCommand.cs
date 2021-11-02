using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetAllMethodsCommand : AltUnityReflectionMethodsCommand<AltUnityGetAllMethodsParams, List<string>>
    {
        public AltUnityGetAllMethodsCommand(AltUnityGetAllMethodsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<string> Execute()
        {
            Type type = GetType(CommandParams.altUnityComponent.componentName, CommandParams.altUnityComponent.assemblyName);
            MethodInfo[] methodInfos = new MethodInfo[1];
            switch (CommandParams.methodSelection)
            {
                case AltUnityMethodSelection.CLASSMETHODS:
                    methodInfos = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
                case AltUnityMethodSelection.INHERITEDMETHODS:
                    var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var classMethods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    methodInfos = allMethods.Except(classMethods).ToArray();
                    break;
                case AltUnityMethodSelection.ALLMETHODS:
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
