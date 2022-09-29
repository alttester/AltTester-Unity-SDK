using System;
using System.Collections.Generic;
using System.Linq;
using Altom.AltDriver;
using Altom.AltDriver.Commands;
using Altom.AltTester.Logging;

namespace Altom.AltTester.Commands
{
    class AltGetAllFieldsCommand : AltReflectionMethodsCommand<AltGetAllFieldsParams, List<AltProperty>>
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public AltGetAllFieldsCommand(AltGetAllFieldsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltProperty> Execute()
        {

            UnityEngine.GameObject altObject;
            altObject = AltRunner.GetGameObject(CommandParams.altUnityObjectId);

            Type type = GetType(CommandParams.altUnityComponent.componentName, CommandParams.altUnityComponent.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            System.Reflection.FieldInfo[] fieldInfos = null;


            switch (CommandParams.altUnityFieldsSelections)
            {
                case AltFieldsSelections.CLASSFIELDS:
                    fieldInfos = type.GetFields(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
                case AltFieldsSelections.INHERITEDFIELDS:
                    var allFields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    var classFields = type.GetFields(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    fieldInfos = allFields.Except(classFields).ToArray();
                    break;
                case AltFieldsSelections.ALLFIELDS:
                    fieldInfos = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                    break;
            }

            var listFields = new List<AltProperty>();
            foreach (var fieldInfo in fieldInfos)
            {
                try
                {
                    var value = fieldInfo.GetValue(altObjectComponent);
                    AltType altUnityType = AltType.OBJECT;
                    if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.Equals(typeof(string)))
                    {
                        altUnityType = AltType.PRIMITIVE;
                    }
                    else if (fieldInfo.FieldType.IsArray)
                    {
                        altUnityType = AltType.ARRAY;
                    }
                    listFields.Add(new AltProperty(fieldInfo.Name,
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
