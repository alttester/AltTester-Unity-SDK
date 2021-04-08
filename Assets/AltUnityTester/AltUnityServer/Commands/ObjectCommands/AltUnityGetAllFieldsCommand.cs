using System;
using System.Collections.Generic;
using System.Linq;
using Altom.AltUnityDriver;
using Altom.Server.Logging;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllFieldsCommand : AltUnityReflectionMethodsCommand
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        readonly int id;
        private AltUnityComponent component;
        readonly AltUnityFieldsSelections altUnityFieldsSelections;

        public AltUnityGetAllFieldsCommand(params string[] parameters) : base(parameters, 5)
        {
            this.id = JsonConvert.DeserializeObject<int>(parameters[2]);
            this.component = JsonConvert.DeserializeObject<AltUnityComponent>(parameters[3]);
            this.altUnityFieldsSelections = (AltUnityFieldsSelections)Enum.Parse(typeof(AltUnityFieldsSelections), parameters[4]);
        }

        public override string Execute()
        {
            UnityEngine.GameObject altObject;
            altObject = AltUnityRunner.GetGameObject(id);
            Type type = GetType(component.componentName, component.assemblyName);
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

            var listFields = new List<AltUnityProperty>();
            foreach (var fieldInfo in fieldInfos)
            {
                try
                {
                    var value = fieldInfo.GetValue(altObjectComponent);
                    AltUnityType altUnityType = AltUnityType.OBJECT;
                    if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.Equals(typeof(string)))
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
                catch (Exception e)
                {
                    logger.Error(e);
                }

            }
            return JsonConvert.SerializeObject(listFields);
        }
    }
}
