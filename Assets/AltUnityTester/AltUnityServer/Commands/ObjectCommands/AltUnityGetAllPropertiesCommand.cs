using System;
using System.Linq;
using Altom.AltUnityDriver;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllPropertiesCommand : AltUnityReflectionMethodsCommand
    {
        string id;
        AltUnityComponent component;
        AltUnityPropertiesSelections altUnityPropertiesSelections;

        public AltUnityGetAllPropertiesCommand(params string[] parameters) : base(parameters, 5)
        {
            this.id = parameters[2];
            this.component = JsonConvert.DeserializeObject<AltUnityComponent>(parameters[3]);
            this.altUnityPropertiesSelections = (AltUnityPropertiesSelections)Enum.Parse(typeof(AltUnityPropertiesSelections), parameters[4], true);
        }

        public override string Execute()
        {
            LogMessage("getAllProperties");
            UnityEngine.GameObject altObject;
            altObject = id.Equals("null") ? null : AltUnityRunner.GetGameObject(System.Convert.ToInt32(id));
            System.Type type = GetType(component.componentName, component.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var propertyInfos = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            switch (altUnityPropertiesSelections)
            {
                case AltUnityPropertiesSelections.CLASSPROPERTIES:
                    propertyInfos = type.GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
                case AltUnityPropertiesSelections.INHERITEDPROPERTIES:
                    var allProperties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    var classProperties = type.GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    propertyInfos = allProperties.Except(classProperties).ToArray();
                    break;
                case AltUnityPropertiesSelections.ALLPROPERTIES:
                    propertyInfos = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
            }

            System.Collections.Generic.List<AltUnityProperty> listProperties = new System.Collections.Generic.List<AltUnityProperty>();

            foreach (var propertyInfo in propertyInfos)
            {
                try
                {
                    var value = propertyInfo.GetValue(altObjectComponent, null);
                    AltUnityType altUnityType = AltUnityType.OBJECT;
                    if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.Equals(typeof(System.String)))
                    {
                        altUnityType = AltUnityType.PRIMITIVE;
                    }
                    else if (propertyInfo.PropertyType.IsArray)
                    {
                        altUnityType = AltUnityType.ARRAY;
                    }
                    listProperties.Add(new AltUnityProperty(propertyInfo.Name,
                        value == null ? "null" : value.ToString(), altUnityType));
                }
                catch (Exception e)
                {
                    LogMessage(e.Message);
                }

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listProperties);
        }
    }
}
