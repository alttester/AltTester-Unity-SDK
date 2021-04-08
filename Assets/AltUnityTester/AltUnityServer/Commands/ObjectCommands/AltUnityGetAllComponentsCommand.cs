using System.Collections.Generic;
using Altom.AltUnityDriver;
using Altom.Server.Logging;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllComponentsCommand : AltUnityCommand
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        readonly int objectId;

        public AltUnityGetAllComponentsCommand(params string[] parameters) : base(parameters, 3)
        {
            this.objectId = JsonConvert.DeserializeObject<int>(parameters[2]);
        }

        public override string Execute()
        {
            UnityEngine.GameObject altObject = AltUnityRunner.GetGameObject(objectId);
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

            var response = JsonConvert.SerializeObject(listComponents);
            return response;
        }
    }
}
