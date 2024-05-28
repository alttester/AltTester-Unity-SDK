/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Linq;
using System.Reflection;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltCallComponentMethodForObjectCommand : AltReflectionMethodsCommand<AltCallComponentMethodForObjectParams, object>
    {
        public AltCallComponentMethodForObjectCommand(AltCallComponentMethodForObjectParams cmdParams) : base(cmdParams)
        {

        }

        public override object Execute()
        {
            if (CommandParams.typeOfParameters != null && CommandParams.typeOfParameters.Length != 0 && CommandParams.parameters.Length != CommandParams.typeOfParameters.Length)
            {
                throw new InvalidParameterTypeException("Number of parameters different than number of types of parameters");
            }

            System.Reflection.MethodInfo methodInfoToBeInvoked;
            var componentType = GetType(CommandParams.component, CommandParams.assembly);
            var methodPathSplited = CommandParams.method.Split('.');
            string methodName;
            object instance;
            if (CommandParams.altObject != null)
            {
                UnityEngine.GameObject gameObject = AltRunner.GetGameObject(CommandParams.altObject.id);
                if (componentType == typeof(UnityEngine.GameObject))
                {
                    instance = gameObject;
                    if (instance == null)
                    {
                        throw new ObjectWasNotFoundException("Object with name=" + CommandParams.altObject.name + " and id=" + CommandParams.altObject.id + " was not found");
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
                methodName = CommandParams.method;

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


            methodInfoToBeInvoked = GetMethodToBeInvoked(methodInfos);

            return InvokeMethod(methodInfoToBeInvoked, CommandParams.parameters, instance);
        }

        private MethodInfo GetMethodToBeInvoked(MethodInfo[] methodInfos)
        {
            var parameterTypes = getParameterTypes(CommandParams.typeOfParameters);

            foreach (var methodInfo in methodInfos.Where(method => method.GetParameters().Length == CommandParams.parameters.Length))
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

            var errorMessage = "No method found with " + CommandParams.parameters.Length + " parameters matching signature: " +
                CommandParams.method + "(" + CommandParams.typeOfParameters + ")";

            throw new MethodWithGivenParametersNotFoundException(errorMessage);
        }


        private Type[] getParameterTypes(string[] typeOfParameters)
        {
            if (typeOfParameters == null || typeOfParameters.Length == 0)
                return new Type[0];

            var types = new Type[typeOfParameters.Length];
            for (int i = 0; i < typeOfParameters.Length; i++)
            {
                var type = Type.GetType(typeOfParameters[i]);
                if (type == null)
                    throw new InvalidParameterTypeException("Parameter type " + typeOfParameters[i] + " not found.");
                types[i] = type;
            }
            return types;
        }
    }
}
