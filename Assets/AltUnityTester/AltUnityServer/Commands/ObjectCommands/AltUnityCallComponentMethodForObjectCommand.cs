
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityCallComponentMethodForObjectCommand : AltUnityReflectionMethodsCommand 
    {
        string altObjectString;
        string actionString;

        public AltUnityCallComponentMethodForObjectCommand (string altObjectString, string actionString)
        {
            this.altObjectString = altObjectString;
            this.actionString = actionString;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("call action " + actionString + " for object " + altObjectString);
            string response = AltUnityRunner._altUnityRunner.errorMethodNotFoundMessage;
            System.Reflection.MethodInfo methodInfoToBeInvoked;
            AltUnityObjectAction altAction = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectAction>(actionString);
            var componentType = GetType(altAction.Component, altAction.Assembly);

            System.Reflection.MethodInfo[] methodInfos = GetMethodInfoWithSpecificName(componentType, altAction.Method);
            if (methodInfos.Length == 1)
                methodInfoToBeInvoked = methodInfos[0];
            else
            {
                methodInfoToBeInvoked = GetMethodToBeInvoked(methodInfos, altAction);
            }



            if (string.IsNullOrEmpty(altObjectString))
            {
                response = InvokeMethod(methodInfoToBeInvoked, altAction, null, response);
            }
            else
            {
                AltUnityObject altObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altObject);
                if (componentType == typeof(UnityEngine.GameObject))
                {
                    response = InvokeMethod(methodInfoToBeInvoked, altAction, gameObject, response);
                }
                else
                if (gameObject.GetComponent(componentType) != null)
                {
                    UnityEngine.Component component = gameObject.GetComponent(componentType);
                    response = InvokeMethod(methodInfoToBeInvoked, altAction, component, response);
                }
            }
            return response;
                
        }
    }
}
