using System;
using System.Linq;
using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllFieldsCommand : AltUnityReflectionMethodsCommand
    {
        string id;
        AltUnityComponent component;
        AltUnityFieldsSelections altUnityFieldsSelections;

        public AltUnityGetAllFieldsCommand(params string[] parameters) : base(parameters, 5)
        {
            this.id = parameters[2];
            this.component = JsonConvert.DeserializeObject<AltUnityComponent>(parameters[3]);
            this.altUnityFieldsSelections = (AltUnityFieldsSelections)Enum.Parse(typeof(AltUnityFieldsSelections), parameters[4], true);
        }

        public override string Execute()
        {
            LogMessage("getAllFields");
            UnityEngine.GameObject altObject;
            altObject = id.Equals("null") ? null : AltUnityRunner.GetGameObject(System.Convert.ToInt32(id));
            System.Type type = GetType(component.componentName, component.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            System.Reflection.FieldInfo[] fieldInfos = null;

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

            System.Collections.Generic.List<AltUnityProperty> listFields = new System.Collections.Generic.List<AltUnityProperty>();
            foreach (var fieldInfo in fieldInfos)
            {
                try
                {
                    var value = fieldInfo.GetValue(altObjectComponent);
                    AltUnityType altUnityType = AltUnityType.OBJECT;
                    if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.Equals(typeof(System.String)))
                    {
                        altUnityType = AltUnityType.PRIMITIVE;
                    }
                    else if (fieldInfo.FieldType.IsArray)
                    {
                        altUnityType = AltUnityType.ARRAY;
                    }
                    listFields.Add(new AltUnityProperty(fieldInfo.Name,
                        value == null ? "null" : value.ToString(), altUnityType));

                }
                catch (System.Exception e)
                {
                    LogMessage(e.Message);
                }

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listFields);
        }
    }
}
