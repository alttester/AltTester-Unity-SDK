namespace Altom.AltUnityDriver
{
    public struct AltUnityProperty
    {
        public string name;
        public string value;
        public AltUnityType type;

        public AltUnityProperty(string name, string value, AltUnityType type)
        {
            this.name = name;
            this.value = value;
            this.type = type;
        }
    }
}