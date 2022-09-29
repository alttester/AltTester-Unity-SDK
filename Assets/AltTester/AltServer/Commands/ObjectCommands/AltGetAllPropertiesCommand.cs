using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Altom.AltDriver;
using Altom.AltDriver.Commands;
using Altom.AltTester.Logging;
using Newtonsoft.Json;

namespace Altom.AltTester.Commands
{
    class AltGetAllPropertiesCommand : AltReflectionMethodsCommand<AltGetAllPropertiesParams, List<AltProperty>>
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public AltGetAllPropertiesCommand(AltGetAllPropertiesParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltProperty> Execute()
        {
            UnityEngine.GameObject altObject;
            altObject = AltRunner.GetGameObject(CommandParams.altUnityObjectId);
            Type type = GetType(CommandParams.altUnityComponent.componentName, CommandParams.altUnityComponent.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            switch (CommandParams.altUnityPropertiesSelections)
            {
                case AltPropertiesSelections.CLASSPROPERTIES:
                    propertyInfos = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
                case AltPropertiesSelections.INHERITEDPROPERTIES:
                    var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    var classProperties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    propertyInfos = allProperties.Except(classProperties).ToArray();
                    break;
                case AltPropertiesSelections.ALLPROPERTIES:
                    propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
            }

            var listProperties = new List<AltProperty>();

            foreach (var propertyInfo in propertyInfos)
            {
                try
                {
                    var value = propertyInfo.GetValue(altObjectComponent, null);
                    AltType altUnityType = AltType.OBJECT;
                    if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.Equals(typeof(string)))
                    {
                        altUnityType = AltType.PRIMITIVE;
                    }
                    else if (propertyInfo.PropertyType.IsArray)
                    {
                        altUnityType = AltType.ARRAY;
                    }
                    listProperties.Add(new AltProperty(propertyInfo.Name,
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
