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
