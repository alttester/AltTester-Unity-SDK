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

using System.Collections.Generic;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Logging;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltGetAllComponentsCommand : AltCommand<AltGetAllComponentsParams, List<AltComponent>>
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public AltGetAllComponentsCommand(AltGetAllComponentsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltComponent> Execute()
        {
            UnityEngine.GameObject altObject = AltRunner.GetGameObject(CommandParams.altObjectId);
            var listComponents = new List<AltComponent>();
            foreach (var component in altObject.GetComponents<UnityEngine.Component>())
            {
                try
                {
                    var componentType = component.GetType();
                    var componentName = componentType.FullName;
                    var assemblyName = componentType.Assembly.GetName().Name;
                    listComponents.Add(new AltComponent(componentName, assemblyName));
                }
                catch (System.NullReferenceException e)
                {
                    logger.Warn(e);
                }
            }

            return listComponents;
        }
    }
}
