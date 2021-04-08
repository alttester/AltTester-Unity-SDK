using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityCallComponentMethodForObjectCommand : AltUnityReflectionMethodsCommand
    {
        private readonly AltUnityObjectAction altUnityObjectAction;
        private readonly AltUnityObject altUnityObject;

        public AltUnityCallComponentMethodForObjectCommand(params string[] parameters) : base(parameters, 4)
        {
            this.altUnityObject = string.IsNullOrEmpty(parameters[2]) ? null : JsonConvert.DeserializeObject<AltUnityObject>(Parameters[2]);
            this.altUnityObjectAction = JsonConvert.DeserializeObject<AltUnityObjectAction>(Parameters[3]);
        }

        public override string Execute()
        {
            System.Reflection.MethodInfo methodInfoToBeInvoked;
            var componentType = GetType(altUnityObjectAction.Component, altUnityObjectAction.Assembly);
            var methodPathSplited = altUnityObjectAction.Method.Split('.');
            string methodName;
            object instance;
            if (altUnityObject != null)
            {
                UnityEngine.GameObject gameObject = AltUnityRunner.GetGameObject(altUnityObject);
                if (componentType == typeof(UnityEngine.GameObject))
                {
                    instance = gameObject;
                    if (instance == null)
                    {
                        throw new ObjectWasNotFoundException("Object with name=" + altUnityObject.name + " and id=" + altUnityObject.id + " was not found");
                    }
                }
                else
                {
                    instance = gameObject.GetComponent(componentType);
                    if (instance == null)
                        throw new ComponentNotFoundException();
                }
                instance = GetInstance(instance, methodPathSplited);

            }
            else
            {
                instance = GetInstance(null, methodPathSplited, componentType);
            }

            if (methodPathSplited.Length > 1)
            {
                methodName = methodPathSplited[methodPathSplited.Length - 1];
            }
            else
            {
                methodName = altUnityObjectAction.Method;

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
            methodInfoToBeInvoked = GetMethodToBeInvoked(methodInfos, altUnityObjectAction);

            return InvokeMethod(methodInfoToBeInvoked, altUnityObjectAction, instance);
        }


    }
}
