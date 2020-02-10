using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer
{
    class AltUnityReflectionMethodsCommand :AltUnityCommand
    {
        public static System.Type GetType(string typeName, string assemblyName)
        {
            var type = System.Type.GetType(typeName);

            if (type != null)
                return type;
            if (assemblyName == null || assemblyName.Equals(""))
            {
                if (typeName.Contains("."))
                {
                    assemblyName = typeName.Substring(0, typeName.LastIndexOf('.'));
                    AltUnityRunner._altUnityRunner.LogMessage("assembly name " + assemblyName);
                    try
                    {
                        var assembly = System.Reflection.Assembly.Load(assemblyName);
                        if (assembly.GetType(typeName) == null)
                            throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException("Component not found");
                        return assembly.GetType(typeName);
                    }
                    catch (System.Exception e)
                    {
                        AltUnityRunner._altUnityRunner.LogMessage(e.Message);
                        return null;
                    }
                }

                throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException("Component not found");
            }
            else
            {
                try
                {
                    var assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly.GetType(typeName) == null)
                        throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException("Component not found");
                    return assembly.GetType(typeName);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log(e);
                    return null;
                }

            }
        }
        protected System.Reflection.MemberInfo GetMemberForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectProperty altUnityObjectProperty)
        {
            System.Reflection.MemberInfo memberInfo = null;
            System.Type componentType = null;
            componentType = GetType(altUnityObjectProperty.Component, altUnityObjectProperty.Assembly);
            System.Reflection.PropertyInfo propertyInfo = componentType.GetProperty(altUnityObjectProperty.Property);
            System.Reflection.FieldInfo fieldInfo = componentType.GetField(altUnityObjectProperty.Property);
            if (AltUnityRunner.GetGameObject(altUnityObject).GetComponent(componentType) != null)
            {
                if (propertyInfo != null)
                    return propertyInfo;
                if (fieldInfo != null)
                    return fieldInfo;
                throw new Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException("Property not found");

            }
            throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException("Component not found");
        }


        private System.Reflection.MethodInfo GetMethodForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectAction altUnityObjectAction)
        {
            System.Type componentType = null;
            componentType = GetType(altUnityObjectAction.Component, altUnityObjectAction.Assembly);
            System.Reflection.MethodInfo methodInfo = componentType.GetMethod(altUnityObjectAction.Method);
            return methodInfo;
        }

        protected System.Reflection.MethodInfo[] GetMethodInfoWithSpecificName(System.Type componentType, string altActionMethod)
        {
            System.Reflection.MethodInfo[] methodInfos = componentType.GetMethods();
            return methodInfos.Where(method => method.Name.Equals(altActionMethod)).ToArray();
        }

        protected System.Reflection.MethodInfo GetMethodToBeInvoked(System.Reflection.MethodInfo[] methodInfos, AltUnityObjectAction altUnityObjectAction)
        {
            var parameter = altUnityObjectAction.Parameters.Split('?');
            var typeOfParametes = altUnityObjectAction.TypeOfParameters.Split('?');
            methodInfos = methodInfos.Where(method => method.GetParameters().Length == parameter.Length).ToArray();
            if (methodInfos.Length == 1)
                return methodInfos[0];
            foreach (var methodInfo in methodInfos)
            {
                try
                {
                    for (int counter = 0; counter < typeOfParametes.Length; counter++)
                    {
                        System.Type type = System.Type.GetType(typeOfParametes[counter]);
                        if (methodInfo.GetParameters()[counter].ParameterType != type)
                            throw new System.Exception("Missmatch in parameter type");
                    }
                    return methodInfo;

                }
                catch (System.Exception)
                {

                }

            }

            var errorMessage = "No method found with this signature: " + altUnityObjectAction.Method + "(";
            errorMessage = typeOfParametes.Aggregate(errorMessage, (current, typeOfParamete) => current + (typeOfParamete + ","));

            errorMessage = errorMessage.Remove(errorMessage.Length - 1);
            errorMessage += ")";
            throw new System.Exception(errorMessage);
        }
        protected static string InvokeMethod(System.Reflection.MethodInfo methodInfo, AltUnityObjectAction altAction, object component, string response)
        {
            if (methodInfo == null) return response;
            if (altAction.Parameters == "")
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject(methodInfo.Invoke(component, null));
            }
            else
            {
                System.Reflection.ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                string[] parameterStrings = altAction.Parameters.Split('?');
                if (parameterInfos.Length != parameterStrings.Length)
                    throw new System.Reflection.TargetParameterCountException();
                object[] parameters = new object[parameterInfos.Length];
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    if (parameterInfos[i].ParameterType == typeof(string))
                        parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(parameterStrings[i]),
                            parameterInfos[i].ParameterType);
                    else
                    {
                        parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(parameterStrings[i], parameterInfos[i].ParameterType);
                    }
                }

                response = Newtonsoft.Json.JsonConvert.SerializeObject(methodInfo.Invoke(component, parameters));
            }
            return response;
        }

        protected string GetValueForMember(System.Reflection.MemberInfo memberInfo, UnityEngine.GameObject testableObject, AltUnityObjectProperty altProperty)
        {
            string response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
            if (memberInfo != null)
            {
                if (memberInfo.MemberType == System.Reflection.MemberTypes.Property)
                {
                    System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)memberInfo;
                    object value = propertyInfo.GetValue(testableObject.GetComponent(GetType(altProperty.Component, altProperty.Assembly)), null);
                    response = SerializeMemberValue(value, propertyInfo.PropertyType);
                }
                if (memberInfo.MemberType == System.Reflection.MemberTypes.Field)
                {
                    System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)memberInfo;
                    object value = fieldInfo.GetValue(testableObject.GetComponent(GetType(altProperty.Component, altProperty.Assembly)));
                    response = SerializeMemberValue(value, fieldInfo.FieldType);
                }
            }
            return response;
        }

        protected string SetValueForMember(System.Reflection.MemberInfo memberInfo, string valueString, UnityEngine.GameObject testableObject, AltUnityObjectProperty altProperty)
        {
            string response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
            if (memberInfo != null)
            {
                if (memberInfo.MemberType == System.Reflection.MemberTypes.Property)
                {
                    System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)memberInfo;
                    try
                    {
                        object value = DeserializeMemberValue(valueString, propertyInfo.PropertyType);
                        if (value != null)
                        {
                            propertyInfo.SetValue(testableObject.GetComponent(altProperty.Component), value, null);
                            response = "valueSet";
                        }
                        else
                            response = AltUnityRunner._altUnityRunner.errorPropertyNotSet;
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.Log(e);
                        response = AltUnityRunner._altUnityRunner.errorPropertyNotSet;
                    }
                }
                if (memberInfo.MemberType == System.Reflection.MemberTypes.Field)
                {
                    System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)memberInfo;
                    try
                    {
                        object value = DeserializeMemberValue(valueString, fieldInfo.FieldType);
                        if (value != null)
                        {
                            fieldInfo.SetValue(testableObject.GetComponent(altProperty.Component), value);
                            response = "valueSet";
                        }
                        else
                            response = AltUnityRunner._altUnityRunner.errorPropertyNotSet;
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.Log(e);
                        response = AltUnityRunner._altUnityRunner.errorPropertyNotSet;
                    }
                }
            }
            return response;
        }

        private string SerializeMemberValue(object value, System.Type type)
        {
            string response;
            if (type == typeof(string))
                return value.ToString();
            try
            {
                response = Newtonsoft.Json.JsonConvert.SerializeObject(value, type, AltUnityRunner._altUnityRunner._jsonSettings);
            }
            catch (Newtonsoft.Json.JsonException)
            {
                response = value.ToString();
            }
            return response;
        }

        private object DeserializeMemberValue(string valueString, System.Type type)
        {
            object value;
            if (type == typeof(string))
                valueString = Newtonsoft.Json.JsonConvert.SerializeObject(valueString);
            try
            {
                value = Newtonsoft.Json.JsonConvert.DeserializeObject(valueString, type);
            }
            catch (Newtonsoft.Json.JsonException)
            {
                value = null;
            }
            return value;
        }

        public override string Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
