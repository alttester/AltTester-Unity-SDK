using System;
using System.Linq;
using System.Collections.Generic;

namespace Assets.AltUnityTester.AltUnityServer
{
    class AltUnityReflectionMethodsCommand : AltUnityCommand
    {
        public static System.Type GetType(string typeName, string assemblyName)
        {
            var type = System.Type.GetType(typeName);
            if (type != null)
                return type;

            if (string.IsNullOrEmpty(assemblyName))
            {
                if (typeName.Contains("."))
                {
                    assemblyName = typeName.Substring(0, typeName.LastIndexOf('.'));
                    AltUnityRunner._altUnityRunner.LogMessage("assembly name " + assemblyName);

                    var assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly == null)
                        throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException("Component not found");
                    type = assembly.GetType(typeName);
                    if (type == null)
                        throw new Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException("Component not found");
                    return type;
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
                catch (System.IO.FileNotFoundException)
                {
                    throw new Assets.AltUnityTester.AltUnityDriver.AssemblyNotFoundException("Assembly not found");
                }
            }
        }
        protected System.Reflection.MemberInfo GetMemberForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectProperty altUnityObjectProperty)
        {
            System.Type componentType;
            componentType = GetType(altUnityObjectProperty.Component, altUnityObjectProperty.Assembly);
            System.Reflection.PropertyInfo propertyInfo = componentType.GetProperty(
                altUnityObjectProperty.Property,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);

            System.Reflection.FieldInfo fieldInfo = componentType.GetField(
                altUnityObjectProperty.Property,
                 System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
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

        protected System.Reflection.MethodInfo[] GetMethodInfoWithSpecificName(System.Type componentType, string altActionMethod)
        {
            System.Reflection.MethodInfo[] methodInfos = componentType.GetMethods().Where(method => method.Name.Equals(altActionMethod)).ToArray();

            if (methodInfos.Length == 0)
            {
                throw new Assets.AltUnityTester.AltUnityDriver.MethodNotFoundException("Method not found");
            }
            return methodInfos;
        }


        protected System.Reflection.MethodInfo GetMethodToBeInvoked(System.Reflection.MethodInfo[] methodInfos, AltUnityObjectAction altUnityObjectAction)
        {
            Type[] parameterTypes = getParameterTypes(altUnityObjectAction);
            var parameters = altUnityObjectAction.Parameters.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var methodInfo in methodInfos.Where(method => method.GetParameters().Length == parameters.Length))
            {
                var methodParameters = methodInfo.GetParameters();
                bool methodSignatureMatches = true;
                for (int counter = 0; counter < parameterTypes.Length && counter < methodParameters.Length; counter++)
                {
                    if (methodParameters[counter].ParameterType != parameterTypes[counter])
                        methodSignatureMatches = false;
                }
                if (methodSignatureMatches)
                    return methodInfo;
            }

            var errorMessage = "No method found with " + parameters.Length + " parameters matching signature: " +
                altUnityObjectAction.Method + "(" + altUnityObjectAction.TypeOfParameters + ")";

            throw new Assets.AltUnityTester.AltUnityDriver.MethodWithGivenParametersNotFoundException(errorMessage);
        }
        protected string InvokeMethod(System.Reflection.MethodInfo methodInfo, AltUnityObjectAction altAction, object component)
        {
            if (altAction.Parameters == string.Empty)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(methodInfo.Invoke(component, null));
            }

            System.Reflection.ParameterInfo[] parameterInfos = methodInfo.GetParameters();

            string[] parameterStrings = altAction.Parameters.Split('?');
            if (parameterInfos.Length != parameterStrings.Length)
                throw new System.Reflection.TargetParameterCountException();

            object[] parameters = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                try
                {
                    if (parameterInfos[i].ParameterType == typeof(string))
                    {
                        parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject
                        (
                           Newtonsoft.Json.JsonConvert.SerializeObject(parameterStrings[i]),
                           parameterInfos[i].ParameterType
                        );
                    }
                    else
                    {
                        parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(parameterStrings[i], parameterInfos[i].ParameterType);
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    throw new Assets.AltUnityTester.AltUnityDriver.FailedToParseArgumentsException();
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(methodInfo.Invoke(component, parameters));
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

        private Type[] getParameterTypes(AltUnityObjectAction altUnityObjectAction)
        {
            if (string.IsNullOrEmpty(altUnityObjectAction.TypeOfParameters))
                return new Type[0];
            var parameterTypes = altUnityObjectAction.TypeOfParameters.Split('?');
            var types = new Type[parameterTypes.Length];
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                var type = Type.GetType(parameterTypes[i]);
                if (type == null)
                    throw new Assets.AltUnityTester.AltUnityDriver.InvalidParameterTypeException($"Parameter type {parameterTypes[i]} not found.");
                types[i] = type;
            }
            return types;
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
