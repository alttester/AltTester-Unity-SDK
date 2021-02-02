using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityCallComponentMethodForObjectCommand : AltUnityReflectionMethodsCommand
    {
        string altObjectString;
        string actionString;

        public AltUnityCallComponentMethodForObjectCommand(params string[] parameters) : base(parameters, 4)
        {
            this.altObjectString = Parameters[2];
            this.actionString = Parameters[3];
        }

        public override string Execute()
        {
            LogMessage("call action " + actionString + " for object " + altObjectString);

            System.Reflection.MethodInfo methodInfoToBeInvoked;
            AltUnityObjectAction altAction = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectAction>(actionString);
            var componentType = GetType(altAction.Component, altAction.Assembly);
            var methodPathSplited = altAction.Method.Split('.');
            string methodName;
            object instance = null;
            if (!string.IsNullOrEmpty(altObjectString))
            {
                AltUnityObject altObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altObject);
                if (componentType == typeof(UnityEngine.GameObject))
                {
                    instance = gameObject;
                    if (instance == null)
                    {
                        throw new ObjectWasNotFoundException("Object with name=" + altObject.name + " and id=" + altObject.id + " was not found");
                    }
                }
                else
                {
                    instance = gameObject.GetComponent(componentType);
                    if (instance == null)
                        throw new ComponentNotFoundException();
                }
                instance = getInstance(instance, methodPathSplited);

            }
            else
            {
                instance = getInstance(null, methodPathSplited, componentType);
            }

            if (methodPathSplited.Length > 1)
            {
                methodName = methodPathSplited[methodPathSplited.Length - 1];
            }
            else
            {
                methodName = altAction.Method;

            }
            System.Reflection.MethodInfo[] methodInfos;

            if (instance == null)
            {
                methodInfos = GetMethodInfoWithSpecificName(componentType, methodName);
            }
            else
            {
                methodInfos = GetMethodInfoWithSpecificName(instance.GetType(), methodName);
            }
            methodInfoToBeInvoked = GetMethodToBeInvoked(methodInfos, altAction);

            return InvokeMethod(methodInfoToBeInvoked, altAction, instance);
        }


    }
}
