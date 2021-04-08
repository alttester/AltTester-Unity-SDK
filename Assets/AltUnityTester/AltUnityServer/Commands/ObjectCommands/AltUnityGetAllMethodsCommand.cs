using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllMethodsCommand : AltUnityReflectionMethodsCommand
    {
        AltUnityComponent component;
        readonly AltUnityMethodSelection methodSelection;

        public AltUnityGetAllMethodsCommand(params string[] parameters) : base(parameters, 4)
        {
            this.component = JsonConvert.DeserializeObject<AltUnityComponent>(parameters[2]);
            this.methodSelection = (AltUnityMethodSelection)Enum.Parse(typeof(AltUnityMethodSelection), parameters[3]);
        }

        public override string Execute()
        {
            Type type = GetType(component.componentName, component.assemblyName);
            MethodInfo[] methodInfos = new MethodInfo[1];
            switch (methodSelection)
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
            return JsonConvert.SerializeObject(listMethods);
        }
    }
}
