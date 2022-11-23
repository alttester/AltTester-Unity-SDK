namespace AltTester.AltDriver
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