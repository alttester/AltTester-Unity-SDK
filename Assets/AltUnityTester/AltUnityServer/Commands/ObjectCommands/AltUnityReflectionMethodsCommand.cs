
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
                throw new MethodNotFoundException(String.Format("Method {0} not found in {1}", altActionMethod, componentType.ToString()));
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

                    parameterValues[i] = JsonConvert.DeserializeObject(parameters[i], parameterInfos[i].ParameterType,
                       new JsonSerializerSettings
                       {
                           Culture = CultureInfo.InvariantCulture
                       });
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
            object value = GetValue(instance, memberInfo, index);

            for (int i = 1; i < fieldArray.Length; i++)
            {
                index = getArrayIndex(fieldArray[i], out propertyName);
                memberInfo = GetMemberForObjectComponent(value.GetType(), propertyName);
                value = GetValue(value, memberInfo, index);
            }
            return SerializeMemberValue(value, value.GetType(), maxDepth);
        }


        protected string SetValueForMember(AltUnityObject altUnityObject, string[] fieldArray, Type componentType, string valueString)
        {
            var instance = AltUnityRunner.GetGameObject(altUnityObject).GetComponent(componentType);
            if (instance == null)
            {
                throw new ComponentNotFoundException("Component " + componentType.Name + " not found");
            }
            setValueRecursive(valueString, fieldArray, instance);

            return "valueSet";
        }
        private object setValueRecursive(string valueAsString, string[] fieldArray, object instance, int counter = 0)
        {
            string propertyName;
            int index = getArrayIndex(fieldArray[counter], out propertyName);
            MemberInfo memberInfo = GetMemberForObjectComponent(instance.GetType(), propertyName);
            object value = GetValue(instance, memberInfo, index);
            if (counter < fieldArray.Length - 1)
            {
                counter++;
                var valueObtained = setValueRecursive(valueAsString, fieldArray, value, counter);
                if (index == -1)
                {
                    if (memberInfo.GetType().Equals(typeof(PropertyInfo)))
                    {
                        ((PropertyInfo)memberInfo).SetValue(instance, valueObtained);
                    }
                    else
                    {
                        ((FieldInfo)memberInfo).SetValue(instance, valueObtained);

                    }
                }

                return value;
            }
            else
            {
                setValue(instance, memberInfo, index, valueAsString);
                return instance;
            }
        }

        protected object GetInstance(object instance, string[] methodPathSplited, Type componentType = null)
        {
            return getInstance(instance, methodPathSplited, 0, componentType);
        }

        protected object GetValue(object instance, MemberInfo memberInfo, int index)
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
            MethodInfo methodDefinition = typeof(AltUnityReflectionMethodsCommand<TParam, TResult>).GetMethod(nameof(SetElementOfListObject), new Type[] { typeof(int), typeof(object), typeof(object), typeof(Type) });

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;

                if (index != -1)
                {
                    var type = propertyInfo.PropertyType.GetElementType() != null ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType.GetGenericArguments().Single();
                    MethodInfo method = methodDefinition.MakeGenericMethod(type);

                    object value = deserializeMemberValue(valueString, type);
                    var listValue = GetValue(instance, memberInfo, -1);

                    listValue = method.Invoke(this, new object[] { index, listValue, value, propertyInfo.PropertyType }); ;
                    propertyInfo.SetValue(instance, listValue);

                }
                else
                {
                    object value = deserializeMemberValue(valueString, propertyInfo.PropertyType);
                    propertyInfo.SetValue(instance, value);

                }
            }
            else if (memberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;

                if (index != -1)
                {
                    var type = fieldInfo.FieldType.GetElementType() != null ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType.GetGenericArguments().Single();
                    object value = deserializeMemberValue(valueString, type);
                    var listValue = GetValue(instance, memberInfo, -1);

                    MethodInfo method = methodDefinition.MakeGenericMethod(type);
                    listValue = method.Invoke(this, new object[] { index, listValue, value, fieldInfo.FieldType }); ;
                    fieldInfo.SetValue(instance, listValue);
                }
                else
                {
                    object value = deserializeMemberValue(valueString, fieldInfo.FieldType);
                    fieldInfo.SetValue(instance, value);
                }
            }
        }

        public object SetElementOfListObject<T>(int index, object enumerable, object value, Type type)
        {
            if (type.IsArray)
            {
                var arrray = enumerable as T[];
                if (arrray != null)
                {
                    arrray[index] = (T)value;
                    return arrray;
                }

            }
            var list = enumerable as IList<T>;
            if (enumerable != null)
            {
                list[index] = (T)value;
                return list;

            }
            return null;
            throw new AltUnityException(AltUnityErrors.errorPropertyNotSet);
        }



        public string SerializeMemberValue(object value, System.Type type, int maxDepth)
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
            instance = GetValue(instance, memberInfo, indexValue);
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
