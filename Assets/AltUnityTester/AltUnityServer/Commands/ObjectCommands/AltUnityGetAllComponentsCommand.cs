using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllComponentsCommand : AltUnityCommand
    {
        string objectID;

        public AltUnityGetAllComponentsCommand(params string[] parameters) : base(parameters, 3)
        {
            this.objectID = parameters[2];
        }

        public override string Execute()
        {
            LogMessage("GetAllComponents");
            UnityEngine.GameObject altObject = AltUnityRunner.GetGameObject(System.Convert.ToInt32(objectID));
            System.Collections.Generic.List<AltUnityComponent> listComponents = new System.Collections.Generic.List<AltUnityComponent>();
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
                    if (e.Source != null)
                        UnityEngine.Debug.LogError("NullReferenceException source: " + e.Source);
                    else
                        UnityEngine.Debug.LogError("NullReferenceException unknown source");
                }

            }

            var response = Newtonsoft.Json.JsonConvert.SerializeObject(listComponents);
            return response;
        }
    }
}
