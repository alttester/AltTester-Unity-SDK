namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetAllComponentsCommand : AltUnityCommand
    {
        string objectID;

        public AltUnityGetAllComponentsCommand(string objectID)
        {
            this.objectID = objectID;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("GetAllComponents");
            UnityEngine.GameObject altObject = AltUnityRunner.GetGameObject(System.Convert.ToInt32(objectID));
            System.Collections.Generic.List<AltUnityComponent> listComponents = new System.Collections.Generic.List<AltUnityComponent>();
            foreach (var component in altObject.GetComponents<UnityEngine.Component>())
            {
                var a = component.GetType();
                var componentName = a.FullName;
                var assemblyName = a.Assembly.GetName().Name;
                listComponents.Add(new AltUnityComponent(componentName, assemblyName));
            }

            var response = Newtonsoft.Json.JsonConvert.SerializeObject(listComponents);
            return response;
        }
    }
}
