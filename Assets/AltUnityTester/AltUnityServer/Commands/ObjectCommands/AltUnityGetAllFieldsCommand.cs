using System;
using System.Collections.Generic;
using System.Linq;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Logging;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetAllFieldsCommand : AltUnityReflectionMethodsCommand<AltUnityGetAllFieldsParams, List<AltUnityProperty>>
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public AltUnityGetAllFieldsCommand(AltUnityGetAllFieldsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltUnityProperty> Execute()
        {

            UnityEngine.GameObject altObject;
            altObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObjectId);

            Type type = GetType(CommandParams.altUnityComponent.componentName, CommandParams.altUnityComponent.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            System.Reflection.FieldInfo[] fieldInfos = null;


            switch (CommandParams.altUnityFieldsSelections)
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
            return listFields;
        }
    }
}
