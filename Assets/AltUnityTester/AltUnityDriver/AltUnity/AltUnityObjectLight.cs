namespace Altom.AltUnityDriver
{
    public class AltUnityObjectLight
    {
        public string name;
        public int id;
        public bool enabled;
        public int idCamera;
        public int parentId;
        public int transformId;

        public AltUnityObjectLight(string name, int id = 0, bool enabled = true, int idCamera = 0, int parentId = 0, int transformId = 0)
        {
            this.name = name;
            this.id = id;
            this.enabled = enabled;
            this.idCamera = idCamera;
            this.parentId = parentId;
            this.transformId = transformId;
        }
    }
}