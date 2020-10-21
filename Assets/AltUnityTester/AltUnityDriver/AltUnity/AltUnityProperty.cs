public struct AltUnityProperty
{
    public string name;
    public string value;
    public bool isPrimitive;

    public AltUnityProperty(string name, string value,bool isPrimitive)
    {
        this.name = name;
        this.value = value;
        this.isPrimitive = isPrimitive;
    }
}