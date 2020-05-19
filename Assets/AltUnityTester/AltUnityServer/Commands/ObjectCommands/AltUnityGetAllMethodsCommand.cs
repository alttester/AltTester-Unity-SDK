using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllMethodsCommand :AltUnityReflectionMethodsCommand 
    {
        AltUnityComponent component;
        AltUnityMethodSelection methodSelection;

        public AltUnityGetAllMethodsCommand (AltUnityComponent component,AltUnityMethodSelection methodSelection)
        {
            this.component = component;
            this.methodSelection = methodSelection;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getAllMethods");
            System.Type type = GetType(component.componentName, component.assemblyName);
            //var methodInfos = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            System.Reflection.MethodInfo[] methodInfos=new System.Reflection.MethodInfo[1];
            switch (methodSelection)
            {
                case AltUnityMethodSelection.CLASSMETHODS:
                    methodInfos = type.GetMethods(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
                case AltUnityMethodSelection.INHERITEDMETHODS:
                    var allMethods= type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static); 
                    var classMethods = type.GetMethods(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    methodInfos = allMethods.Except(classMethods).ToArray();
                    break;
                case AltUnityMethodSelection.ALLMETHODS:
                    methodInfos = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
            }

            System.Collections.Generic.List<string> listMethods = new System.Collections.Generic.List<string>();

            foreach (var methodInfo in methodInfos)
            {
                listMethods.Add(methodInfo.ToString());
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listMethods);
        }
    }
}
