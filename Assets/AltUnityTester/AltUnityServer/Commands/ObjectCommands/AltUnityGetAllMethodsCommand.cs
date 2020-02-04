namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllMethodsCommand :AltUnityReflectionMethodsCommand 
    {
        AltUnityComponent component;

        public AltUnityGetAllMethodsCommand (AltUnityComponent component)
        {
            this.component = component;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getAllMethods");
            System.Type type = GetType(component.componentName, component.assemblyName);
            var methodInfos = type.GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            System.Collections.Generic.List<string> listMethods = new System.Collections.Generic.List<string>();

            foreach (var methodInfo in methodInfos)
            {
                listMethods.Add(methodInfo.ToString());
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listMethods);
        }
    }
}
