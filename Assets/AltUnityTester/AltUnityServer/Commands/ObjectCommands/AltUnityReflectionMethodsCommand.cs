using System;
using System.Linq;
using System.Collections.Generic;
using Assets.AltUnityTester.AltUnityServer.Commands;
using System.Text.RegularExpressions;
using System.Reflection;

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

                throw new AltUnityDriver.ComponentNotFoundException("Component not found");
            }
            else
            {
                try
                {
                    var assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly.GetType(typeName) == null)
                        throw new AltUnityDriver.ComponentNotFoundException("Component not found");
                    return assembly.GetType(typeName);
                }
                catch (System.IO.FileNotFoundException)
                {
                    throw new Assets.AltUnityTester.AltUnityDriver.AssemblyNotFoundException("Assembly not found");
                }
            }
        }
        protected System.Reflection.MemberInfo GetMemberForObjectComponent(System.Type Type, string propertyName)
        {
            System.Reflection.PropertyInfo propertyInfo = Type.GetProperty(propertyName,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);

            System.Reflection.FieldInfo fieldInfo = Type.GetField(propertyName,
                 System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
            if (propertyInfo != null)
                return propertyInfo;
            if (fieldInfo != null)
                return fieldInfo;
            throw new AltUnityDriver.PropertyNotFoundException("Property " + propertyName + " not found");
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

        protected string GetValueForMember(AltUnityObject altUnityObject, string[] fieldArray, Type componentType, int maxDepth)
        {
            string propertyName;
            int index = GetArrayIndex(fieldArray[0], out propertyName);
            System.Reflection.MemberInfo memberInfo = GetMemberForObjectComponent(componentType, propertyName);
            var instance = AltUnityRunner.GetGameObject(altUnityObject).GetComponent(componentType);
            if (instance == null)
            {
                throw new AltUnityDriver.ComponentNotFoundException("Component " + componentType.Name + " not found");
            }
            object value = GetValue(instance, memberInfo, index);

            for (int i = 1; i < fieldArray.Length; i++)
            {
                index = GetArrayIndex(fieldArray[i], out propertyName);
                memberInfo = GetMemberForObjectComponent(value.GetType(), propertyName);
                value = GetValue(value, memberInfo, index);
            }
            return SerializeMemberValue(value, value.GetType(), maxDepth);
        }
        private object GetValue(object instance, System.Reflection.MemberInfo memberInfo, int index)
        {
            object value = null;
            if (memberInfo.MemberType == System.Reflection.MemberTypes.Property)
            {
                value = ((System.Reflection.PropertyInfo)memberInfo).GetValue(instance, null);
            }
            else if (memberInfo.MemberType == System.Reflection.MemberTypes.Field)
            {
                value = ((System.Reflection.FieldInfo)memberInfo).GetValue(instance);
            }
            if (index == -1)
            {
                return value;
            }
            else
            {
                System.Collections.IEnumerable enumerable = value as System.Collections.IEnumerable;
                if (enumerable != null)
                {
                    int i = 0;
                    foreach (object element in enumerable)
                    {
                        if (i == index)
                        {
                            return element;
                        }
                        i++;
                    }
                }
                throw new AltUnityDriver.AltUnityException(AltUnityRunner._altUnityRunner.errorIndexOutOfRange);

            }
        }
        private int GetArrayIndex(string arrayProperty, out string propertyName)
        {
            if (Regex.IsMatch(arrayProperty, @".*\[[0-9]\]*"))
            {
                var arrayPropertySplited = arrayProperty.Split('[');
                propertyName = arrayPropertySplited[0];
                return int.Parse(arrayPropertySplited[1].Split(']')[0]);
            }
            else
            {
                propertyName = arrayProperty;
                return -1;
            }
        }

        protected string SetValueForMember(AltUnityObject altUnityObject, string[] fieldArray, Type componentType, string valueString)
        {

            string propertyName;
            int index = GetArrayIndex(fieldArray[0], out propertyName);
            System.Reflection.MemberInfo memberInfo = GetMemberForObjectComponent(componentType, propertyName);
            var instance = AltUnityRunner.GetGameObject(altUnityObject).GetComponent(componentType);
            if (instance == null)
            {
                throw new AltUnityDriver.ComponentNotFoundException("Component " + componentType.Name + " not found");
            }
            if (fieldArray.Length > 1)
            {
                object value = GetValue(instance, memberInfo, index);

                for (int i = 1; i < fieldArray.Length - 1; i++)
                {
                    index = GetArrayIndex(fieldArray[i], out propertyName);
                    memberInfo = GetMemberForObjectComponent(value.GetType(), propertyName);
                    value = GetValue(value, memberInfo, index);

                }

                index = GetArrayIndex(fieldArray[fieldArray.Length - 1], out propertyName);
                memberInfo = GetMemberForObjectComponent(value.GetType(), propertyName);
                SetValue(value, memberInfo, index, valueString);

            }
            else
            {
                SetValue(instance, memberInfo, index, valueString);
            }
            return "valueSet";

        }

        private void SetValue(object instance, MemberInfo memberInfo, int index, string valueString)
        {
            if (memberInfo.MemberType == System.Reflection.MemberTypes.Property)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                object value = DeserializeMemberValue(valueString, propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, value);
            }
            else if (memberInfo.MemberType == System.Reflection.MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
                object value = DeserializeMemberValue(valueString, fieldInfo.FieldType);
                fieldInfo.SetValue(instance, value);
            }
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

        private string SerializeMemberValue(object value, System.Type type, int maxDepth)
        {
            string response;
            if (type == typeof(string))
                return value.ToString();
            try
            {
                using (var strWriter = new System.IO.StringWriter())
                {
                    using (var jsonWriter = new CustomJsonTextWriter(strWriter))
                    {
                        Func<bool> include = () => jsonWriter.CurrentDepth <= maxDepth;
                        var resolver = new AltUnityContractResolver(include);
                        var serializer = new Newtonsoft.Json.JsonSerializer { ContractResolver = resolver, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
                        serializer.Serialize(jsonWriter, value);
                    }
                    return strWriter.ToString();
                }
            }
            catch (System.Exception e)
            {
                AltUnityRunner._altUnityRunner.LogMessage(e.Message);
                UnityEngine.Debug.LogError(e);
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
            catch (Newtonsoft.Json.JsonException e)
            {
                AltUnityRunner._altUnityRunner.LogMessage(e.Message);
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
