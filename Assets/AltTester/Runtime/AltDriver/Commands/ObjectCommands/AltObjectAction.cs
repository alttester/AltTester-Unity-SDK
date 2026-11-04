/*
    Copyright(C) 2026 Altom Consulting
*/

using System;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    [Obsolete]
    public struct AltObjectAction
    {
        public string Component;
        public string Method;
        public string Parameters;
        public string TypeOfParameters;

        public AltObjectAction(string component = "", string method = "", string parameters = "", string typeOfParameters = "", string assembly = "")
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
