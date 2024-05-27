/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Logging;

namespace AltTester.AltTesterUnitySDK.Commands
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
            altObject = AltRunner.GetGameObject(CommandParams.altObjectId);

            Type type = GetType(CommandParams.altComponent.componentName, CommandParams.altComponent.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            System.Reflection.FieldInfo[] fieldInfos = null;


            switch (CommandParams.altFieldsSelections)
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
                    AltType altType = AltType.OBJECT;
                    if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType.Equals(typeof(string)))
                    {
                        altType = AltType.PRIMITIVE;
                    }
                    else if (fieldInfo.FieldType.IsArray)
                    {
                        altType = AltType.ARRAY;
                    }
                    listFields.Add(new AltProperty(fieldInfo.Name,
                        value == null ? "null" : value.ToString(), altType));

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
