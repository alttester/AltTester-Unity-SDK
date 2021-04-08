using System;

namespace Altom.AltUnityDriver
{
    public class AltUnityObjectLight
    {
        public string name;
        public int id;
        public bool enabled;
        public int transformParentId;
        public int transformId;

        public AltUnityObjectLight(string name, int id = 0, bool enabled = true, int transformParentId = 0, int transformId = 0)
        {
            this.name = name;
            this.id = id;
            this.enabled = enabled;
            this.transformParentId = transformParentId;
            this.transformId = transformId;
        }
    }
}