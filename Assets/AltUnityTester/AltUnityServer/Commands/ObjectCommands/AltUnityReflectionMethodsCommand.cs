
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.Server.Logging;
using Assets.AltUnityTester.AltUnityServer.Commands;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer
{
    class AltUnityReflectionMethodsCommand<TParam, TResult> : AltUnityCommand<TParam, TResult> where TParam : CommandParams
    {
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        protected AltUnityReflectionMethodsCommand(TParam cmdParams) : base(cmdParams) { }

        public Type GetType(string typeName, string assemblyName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
                return type;
            if (string.IsNullOrEmpty(assemblyName))
            {
                if (typeName.Contains("."))
                {
                    assemblyName = typeName.Substring(0, typeName.LastIndexOf('.'));

                    var assembly = Assembly.Load(assemblyName);
                    if (assembly == null)
                        throw new ComponentNotFoundException("Component not found");
                    type = assembly.GetType(typeName);
                    if (type == null)
                        throw new ComponentNotFoundException("Component not found");
                    return type;
                }

                throw new ComponentNotFoundException("Component not found");
            }
            else
            {
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    if (assembly.GetType(typeName) == null)
                        throw new ComponentNotFoundException("Component not found");
                    return assembly.GetType(typeName);
                }
                catch (System.IO.FileNotFoundException)
                {
                    throw new AssemblyNotFoundException("Assembly not found");
                }
            }
        }

        public override TResult Execute()
        {
            throw new NotImplementedException();
        }

        protected MemberInfo GetMemberForObjectComponent(Type Type, string propertyName)
        {
            PropertyInfo propertyInfo = Type.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            FieldInfo fieldInfo = Type.GetField(propertyName,
                 BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (propertyInfo != null)
                return propertyInfo;
            if (fieldInfo != null)
                return fieldInfo;
            throw new PropertyNotFoundException("Property " + propertyName + " not found");
        }

        protected MethodInfo[] GetMethodInfoWithSpecificName(System.Type componentType, string altActionMethod)
        {
            MethodInfo[] methodInfos = componentType.GetMethods().Where(method => method.Name.Equals(altActionMethod)).ToArray();

            if (methodInfos.Length == 0)
            {
                throw new MethodNotFoundException("Method not found");
            }
            return methodInfos;
        }

        protected object InvokeMethod(MethodInfo methodInfo, string[] parameters, object component)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();

            if (parameterInfos.Length != parameters.Length)
                throw new TargetParameterCountException();

            object[] parameterValues = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                try
                {
                    if (parameterInfos[i].ParameterType == typeof(string))
                    {
                        parameterValues[i] = parameters[i];
                    }
                    else
                    {
                        parameterValues[i] = JsonConvert.DeserializeObject(parameters[i], parameterInfos[i].ParameterType,
                           new JsonSerializerSettings
                           {
                               Culture = CultureInfo.InvariantCulture
                           });
                    }
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    throw new FailedToParseArgumentsException(string.Format("Could not parse parameter '{0}' to type {1}", parameters[i], parameterInfos[i].ParameterType));
                }
            }

            return methodInfo.Invoke(component, parameterValues);
        }

        protected string GetValueForMember(AltUnityObject altUnityObject, string[] fieldArray, Type componentType, int maxDepth)
        {
            string propertyName;
            int index = getArrayIndex(fieldArray[0], out propertyName);
            MemberInfo memberInfo = GetMemberForObjectComponent(componentType, propertyName);
            var instance = AltUnityRunner.GetGameObject(altUnityObject).GetComponent(componentType);
            if (instance == null)
            {
                throw new ComponentNotFoundException("Component " + componentType.Name + " not found");
            }
            object value = getValue(instance, memberInfo, index);

            for (int i = 1; i < fieldArray.Length; i++)
            {
                index = getArrayIndex(fieldArray[i], out propertyName);
                memberInfo = GetMemberForObjectComponent(value.GetType(), propertyName);
                value = getValue(value, memberInfo, index);
            }
            return serializeMemberValue(value, value.GetType(), maxDepth);
        }


        protected string SetValueForMember(AltUnityObject altUnityObject, string[] fieldArray, Type componentType, string valueString)
        {
            string propertyName;
            int index = getArrayIndex(fieldArray[0], out propertyName);
            MemberInfo memberInfo = GetMemberForObjectComponent(componentType, propertyName);
            var instance = AltUnityRunner.GetGameObject(altUnityObject).GetComponent(componentType);
            if (instance == null)
            {
                throw new ComponentNotFoundException("Component " + componentType.Name + " not found");
            }
            if (fieldArray.Length > 1)
            {
                object value = getValue(instance, memberInfo, index);

                for (int i = 1; i < fieldArray.Length - 1; i++)
                {
                    index = getArrayIndex(fieldArray[i], out propertyName);
                    memberInfo = GetMemberForObjectComponent(value.GetType(), propertyName);
                    value = getValue(value, memberInfo, index);
                }

                index = getArrayIndex(fieldArray[fieldArray.Length - 1], out propertyName);
                memberInfo = GetMemberForObjectComponent(value.GetType(), propertyName);
                setValue(value, memberInfo, index, valueString);
            }
            else
            {
                setValue(instance, memberInfo, index, valueString);
            }
            return "valueSet";
        }

        protected object GetInstance(object instance, string[] methodPathSplited, Type componentType = null)
        {
            return getInstance(instance, methodPathSplited, 0, componentType);
        }

        private object getValue(object instance, MemberInfo memberInfo, int index)
        {
            object value = null;
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                value = ((PropertyInfo)memberInfo).GetValue(instance, null);
            }
            else if (memberInfo.MemberType == MemberTypes.Field)
            {
                value = ((FieldInfo)memberInfo).GetValue(instance);
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
                throw new AltUnityException(AltUnityErrors.errorIndexOutOfRange);
            }
        }

        private int getArrayIndex(string arrayProperty, out string propertyName)
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

        private void setValue(object instance, MemberInfo memberInfo, int index, string valueString)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                object value = deserializeMemberValue(valueString, propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, value);
            }
            else if (memberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
                object value = deserializeMemberValue(valueString, fieldInfo.FieldType);
                fieldInfo.SetValue(instance, value);
            }
        }



        private string serializeMemberValue(object value, System.Type type, int maxDepth)
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
            catch (Exception e)
            {
                logger.Trace(e);
                response = value.ToString();
            }
            return response;
        }

        private object deserializeMemberValue(string valueString, System.Type type)
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
                logger.Trace(e);
                value = null;
            }
            return value;
        }

        private object getInstance(object instance, string[] methodPathSplited, int index, Type componentType = null)
        {
            if (methodPathSplited.Length - 1 <= index)
                return instance;
            string propertyName;
            int indexValue = getArrayIndex(methodPathSplited[index], out propertyName);

            Type type = instance == null ? componentType : instance.GetType();//Checking for static fields

            MemberInfo memberInfo = GetMemberForObjectComponent(type, propertyName);
            instance = getValue(instance, memberInfo, indexValue);
            if (instance == null)
            {
                string path = "";
                for (int i = 0; i < index; i++)
                {
                    path += methodPathSplited[i] + ".";
                }
                throw new Altom.AltUnityDriver.NullReferenceException(path + propertyName + "is not assigned");
            }
            index++;
            return getInstance(instance, methodPathSplited, index);
        }

    }
}
