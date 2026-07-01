/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver
{
    public struct AltComponent
    {
        public string componentName;
        public string assemblyName;

        public AltComponent(string componentName, string assemblyName)
        {
            this.componentName = componentName;
            this.assemblyName = assemblyName;
        }
    }
}
