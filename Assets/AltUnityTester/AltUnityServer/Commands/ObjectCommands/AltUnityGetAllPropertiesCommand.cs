using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Logging;
using Newtonsoft.Json;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetAllPropertiesCommand : AltUnityReflectionMethodsCommand<AltUnityGetAllPropertiesParams, List<AltUnityProperty>>
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public AltUnityGetAllPropertiesCommand(AltUnityGetAllPropertiesParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltUnityProperty> Execute()
        {
            UnityEngine.GameObject altObject;
            altObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObjectId);
            Type type = GetType(CommandParams.altUnityComponent.componentName, CommandParams.altUnityComponent.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            switch (CommandParams.altUnityPropertiesSelections)
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
            return listProperties;
        }
    }
}
