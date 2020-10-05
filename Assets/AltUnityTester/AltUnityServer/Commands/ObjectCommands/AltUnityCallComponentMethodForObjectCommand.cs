
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityCallComponentMethodForObjectCommand : AltUnityReflectionMethodsCommand
    {
        string altObjectString;
        string actionString;

        public AltUnityCallComponentMethodForObjectCommand(string altObjectString, string actionString)
        {
            this.altObjectString = altObjectString;
            this.actionString = actionString;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("call action " + actionString + " for object " + altObjectString);

            System.Reflection.MethodInfo methodInfoToBeInvoked;
            AltUnityObjectAction altAction = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectAction>(actionString);
            var componentType = GetType(altAction.Component, altAction.Assembly);

            System.Reflection.MethodInfo[] methodInfos = GetMethodInfoWithSpecificName(componentType, altAction.Method);
            methodInfoToBeInvoked = GetMethodToBeInvoked(methodInfos, altAction);

            if (string.IsNullOrEmpty(altObjectString))
                return InvokeMethod(methodInfoToBeInvoked, altAction, null);

            AltUnityObject altObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
            UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altObject);
            if (componentType == typeof(UnityEngine.GameObject))
            {
                return InvokeMethod(methodInfoToBeInvoked, altAction, gameObject);
            }
            UnityEngine.Component component = gameObject.GetComponent(componentType);
            if (component == null)
                throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException();

            return InvokeMethod(methodInfoToBeInvoked, altAction, component);
        }
    }
}
