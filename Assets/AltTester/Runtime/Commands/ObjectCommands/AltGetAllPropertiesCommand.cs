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
using System.Reflection;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Logging;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Commands
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
            altObject = AltRunner.GetGameObject(CommandParams.altObjectId);
            Type type = GetType(CommandParams.altComponent.componentName, CommandParams.altComponent.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            switch (CommandParams.altPropertiesSelections)
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
                    AltType altType = AltType.OBJECT;
                    if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.Equals(typeof(string)))
                    {
                        altType = AltType.PRIMITIVE;
                    }
                    else if (propertyInfo.PropertyType.IsArray)
                    {
                        altType = AltType.ARRAY;
                    }
                    listProperties.Add(new AltProperty(propertyInfo.Name,
                        value == null ? "null" : value.ToString(), altType));
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
