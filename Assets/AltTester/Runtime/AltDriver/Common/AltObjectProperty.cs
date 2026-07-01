/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver
{
    public struct AltObjectProperty
    {
        public string Component;
        public string Property;
        public string Assembly;

        public AltObjectProperty(string component = "", string property = "") :
            this(component, property, null)
        { }

        public AltObjectProperty(string component, string property, string assembly)
        {
            Component = component;
            Property = property;
            Assembly = assembly;
        }
    }
}
