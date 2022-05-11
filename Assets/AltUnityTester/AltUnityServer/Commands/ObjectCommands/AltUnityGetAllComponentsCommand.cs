using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.AltUnityTester.Logging;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityGetAllComponentsCommand : AltUnityCommand<AltUnityGetAllComponentsParams, List<AltUnityComponent>>
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public AltUnityGetAllComponentsCommand(AltUnityGetAllComponentsParams cmdParams) : base(cmdParams)
        {
        }

        public override List<AltUnityComponent> Execute()
        {
            UnityEngine.GameObject altObject = AltUnityRunner.GetGameObject(CommandParams.altUnityObjectId);
            var listComponents = new List<AltUnityComponent>();
            foreach (var component in altObject.GetComponents<UnityEngine.Component>())
            {
                try
                {
                    var a = component.GetType();
                    var componentName = a.FullName;
                    var assemblyName = a.Assembly.GetName().Name;
                    listComponents.Add(new AltUnityComponent(componentName, assemblyName));
                }
                catch (System.NullReferenceException e)
                {
                    logger.Error(e);
                }
            }

            return listComponents;
        }
    }
}
