using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllFieldsCommand :AltUnityReflectionMethodsCommand 
    {
        string id;
        AltUnityComponent component;
        AltUnityFieldsSelections altUnityFieldsSelections;

        public AltUnityGetAllFieldsCommand(string id, AltUnityComponent component, AltUnityFieldsSelections altUnityFieldsSelections)
        {
            this.id = id;
            this.component = component;
            this.altUnityFieldsSelections = altUnityFieldsSelections;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("getAllFields");
            UnityEngine.GameObject altObject;
            altObject = id.Equals("null") ? null : AltUnityRunner.GetGameObject(System.Convert.ToInt32(id));
            System.Type type = GetType(component.componentName, component.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            System.Reflection.FieldInfo[] fieldInfos=null;

            switch (altUnityFieldsSelections)
            {
                case AltUnityFieldsSelections.CLASSFIELDS:
                    fieldInfos = type.GetFields(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
                case AltUnityFieldsSelections.INHERITEDFIELDS:
                    var allFields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    var classFields = type.GetFields(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    fieldInfos = allFields.Except(classFields).ToArray();
                    break;
                case AltUnityFieldsSelections.ALLFIELDS:
                    fieldInfos = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
            }

            System.Collections.Generic.List<AltUnityField> listFields = new System.Collections.Generic.List<AltUnityField>();

            foreach (var fieldInfo in fieldInfos)
            {
                try
                {
                    var value = fieldInfo.GetValue(altObjectComponent);
                    listFields.Add(new AltUnityField(fieldInfo.Name,
                        value == null ? "null" : value.ToString(),fieldInfo.FieldType.IsPrimitive));

                }
                catch (System.Exception e)
                {
                    AltUnityRunner._altUnityRunner.LogMessage(e.Message);
                }

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listFields);
        }
    }
}
