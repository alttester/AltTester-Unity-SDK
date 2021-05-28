using System;

namespace Altom.AltUnityDriver.Commands
{
    [Obsolete]
    public struct AltUnityObjectAction
    {
        public string Component;
        public string Method;
        public string Parameters;
        public string TypeOfParameters;

        public AltUnityObjectAction(string component = "", string method = "", string parameters = "", string typeOfParameters = "", string assembly = "")
        {
            Component = component;
            Method = method;
            Parameters = parameters;
            TypeOfParameters = typeOfParameters;
            Assembly = assembly;
        }


        public string Assembly;
    }
}