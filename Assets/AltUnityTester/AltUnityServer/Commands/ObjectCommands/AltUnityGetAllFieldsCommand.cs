namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllFieldsCommand :AltUnityReflectionMethodsCommand 
    {
        string id;
        AltUnityComponent component;

        public AltUnityGetAllFieldsCommand(string id, AltUnityComponent component)
        {
            this.id = id;
            this.component = component;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getAllFields");
            UnityEngine.GameObject altObject;
            altObject = id.Equals("null") ? null : AltUnityRunner.GetGameObject(System.Convert.ToInt32(id));
            System.Type type = GetType(component.componentName, component.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var fieldInfos = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            System.Collections.Generic.List<AltUnityField> listFields = new System.Collections.Generic.List<AltUnityField>();

            foreach (var fieldInfo in fieldInfos)
            {
                    var value = fieldInfo.GetValue(altObjectComponent);
                    listFields.Add(new AltUnityField(fieldInfo.Name,
                        value == null ? "null" : value.ToString()));
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listFields);
        }
    }
}
