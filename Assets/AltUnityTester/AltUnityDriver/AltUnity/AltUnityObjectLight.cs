using Altom.AltUnityDriver.Commands;
using System;

namespace Altom.AltUnityDriver
{
    public class AltUnityObjectLight
    {
        public string name;
        public int id;
        public bool enabled;
        public int idCamera;
        [Obsolete("Use transformParentid instead.")]
        public int parentId;
        public int transformParentId;
        public int transformId;

        public AltUnityObjectLight(string name, int id = 0, bool enabled = true, int idCamera = 0, int parentId = 0, int transformParentId = 0, int transformId = 0)
        {
            this.name = name;
            this.id = id;
            this.enabled = enabled;
            this.idCamera = idCamera;
            this.parentId = parentId;
            this.transformParentId = (transformParentId != 0) ? transformParentId : this.parentId;
            this.transformId = transformId;
        }
    }
}