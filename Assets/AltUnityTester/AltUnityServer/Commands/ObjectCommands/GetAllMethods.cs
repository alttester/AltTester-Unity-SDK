namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetAllMethods:ReflectionMethods
    {
        AltUnityComponent component;

        public GetAllMethods(AltUnityComponent component)
        {
            this.component = component;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("getAllMethods");
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
