using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Altom.AltUnityDriver;
using Altom.Server.Logging;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllPropertiesCommand : AltUnityReflectionMethodsCommand
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        readonly int id;
        AltUnityComponent component;
        readonly AltUnityPropertiesSelections altUnityPropertiesSelections;

        public AltUnityGetAllPropertiesCommand(params string[] parameters) : base(parameters, 5)
        {
            this.id = JsonConvert.DeserializeObject<int>(parameters[2]);
            this.component = JsonConvert.DeserializeObject<AltUnityComponent>(parameters[3]);
            this.altUnityPropertiesSelections = (AltUnityPropertiesSelections)Enum.Parse(typeof(AltUnityPropertiesSelections), parameters[4]);
        }

        public override string Execute()
        {
            UnityEngine.GameObject altObject;
            altObject = AltUnityRunner.GetGameObject(id);
            Type type = GetType(component.componentName, component.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            switch (altUnityPropertiesSelections)
            {
                case AltUnityPropertiesSelections.CLASSPROPERTIES:
                    propertyInfos = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
                case AltUnityPropertiesSelections.INHERITEDPROPERTIES:
                    var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var classProperties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    propertyInfos = allProperties.Except(classProperties).ToArray();
                    break;
                case AltUnityPropertiesSelections.ALLPROPERTIES:
                    propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
            }

            var listProperties = new List<AltUnityProperty>();

            foreach (var propertyInfo in propertyInfos)
            {
                try
                {
                    var value = propertyInfo.GetValue(altObjectComponent, null);
                    AltUnityType altUnityType = AltUnityType.OBJECT;
                    if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.Equals(typeof(string)))
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
                    logger.Trace(e);
                }

            }
            return JsonConvert.SerializeObject(listProperties);
        }
    }
}
