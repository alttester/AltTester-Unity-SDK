using Altom.AltUnityDriver.Commands;
using System;

namespace Altom.AltUnityDriver
{
    public class AltUnityObject
    {
        public string name;
        public int id;
        public int x;
        public int y;
        public int z;
        public int mobileY;
        public string type;
        public bool enabled;
        public float worldX;
        public float worldY;
        public float worldZ;
        public int idCamera;
        [Obsolete("Use transformParentId instead.")]
        public int parentId;
        public int transformParentId;
        public int transformId;
        [Newtonsoft.Json.JsonIgnore]
        public SocketSettings socketSettings;
        public AltUnityObject(string name, int id = 0, int x = 0, int y = 0, int z = 0, int mobileY = 0, string type = "", bool enabled = true, float worldX = 0, float worldY = 0, float worldZ = 0, int idCamera = 0, int parentId = 0, int transformParentId = 0, int transformId = 0)
        {
            this.name = name;
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
            this.mobileY = mobileY;
            this.type = type;
            this.enabled = enabled;
            this.worldX = worldX;
            this.worldY = worldY;
            this.worldZ = worldZ;
            this.idCamera = idCamera;
#pragma warning disable CS0618
            this.parentId = parentId;
            this.transformParentId = (transformParentId != 0) ? transformParentId : parentId;
#pragma warning restore CS0618
            this.transformId = transformId;
        }

        public AltUnityObject getParent()
        {
            return new AltUnityFindObject(socketSettings, By.PATH, "//*[@id=" + this.id + "]/..", By.NAME, "", true).Execute();
        }
        public AltUnityVector2 getScreenPosition()
        {
            return new AltUnityVector2(x, y);
        }
        public AltUnityVector3 getWorldPosition()
        {
            return new AltUnityVector3(worldX, worldY, worldZ);
        }
        public string GetComponentProperty(string componentName, string propertyName, string assemblyName = null, int maxDepth = 2)
        {
            return new AltUnityGetComponentProperty(socketSettings, componentName, propertyName, assemblyName, maxDepth, this).Execute();
        }
        public string SetComponentProperty(string componentName, string propertyName, string value, string assemblyName = null)
        {
            return new AltUnitySetComponentProperty(socketSettings, componentName, propertyName, value, assemblyName, this).Execute();
        }
        public string CallComponentMethod(string componentName, string methodName, string parameters, string typeOfParameters = "", string assemblyName = null)
        {
            return new AltUnityCallComponentMethod(socketSettings, componentName, methodName, parameters, typeOfParameters, assemblyName, this).Execute();
        }
        public string GetText()
        {
            return new AltUnityGetText(socketSettings, this).Execute();
        }
        public AltUnityObject SetText(string text)
        {
            return new AltUnitySetText(socketSettings, this, text).Execute();
        }
        [Obsolete("Use Click")]
        public AltUnityObject ClickEvent()
        {
            return new AltUnityClickEvent(socketSettings, this).Execute();
        }

        /// <summary>
        /// Click current object
        /// </summary>
        /// <param name="count">Number of times to click</param>
        /// <param name="interval">Interval between clicks in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The clicked object</returns>
        public AltUnityObject Click(int count = 1, float interval = 0.1f, bool wait = true)
        {
            return new AltUnityClickElement(socketSettings, this, count, interval, wait).Execute();
        }

        public AltUnityObject PointerUpFromObject()
        {
            return new AltUnityPointerUpFromObject(socketSettings, this).Execute();
        }
        public AltUnityObject PointerDownFromObject()
        {
            return new AltUnityPointerDownFromObject(socketSettings, this).Execute();
        }
        public AltUnityObject PointerEnterObject()
        {
            return new AltUnityPointerEnterObject(socketSettings, this).Execute();
        }
        public AltUnityObject PointerExitObject()
        {
            return new AltUnityPointerExitObject(socketSettings, this).Execute();
        }

        /// <summary>
        /// Tap current object
        /// </summary>
        /// <returns>The tapped object</returns>
        public AltUnityObject Tap()
        {
            //TODO: replace in 1.7.0 with Tap(int count=1, float interval = 0.1f); 
            // keeping it for now for backwards compatibility
            return new AltUnityTap(socketSettings, this, 1).Execute();
        }

        /// <summary>
        /// Tap current object
        /// </summary>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The tapped object</returns>
        public AltUnityObject Tap(int count, float interval = 0.1f, bool wait = true)
        {
            return new AltUnityTapElement(socketSettings, this, count, interval, wait).Execute();
        }

        [Obsolete("Use Tap with parameter count=2")]
        public AltUnityObject DoubleTap()
        {
            return new AltUnityTap(socketSettings, this, 2).Execute();
        }
        public System.Collections.Generic.List<AltUnityComponent> GetAllComponents()
        {
            return new AltUnityGetAllComponents(socketSettings, this).Execute();
        }
        public System.Collections.Generic.List<AltUnityProperty> GetAllProperties(AltUnityComponent altUnityComponent, AltUnityPropertiesSelections altUnityPropertiesSelections = AltUnityPropertiesSelections.ALLPROPERTIES)
        {
            return new AltUnityGetAllProperties(socketSettings, altUnityComponent, this, altUnityPropertiesSelections).Execute();
        }
        public System.Collections.Generic.List<AltUnityProperty> GetAllFields(AltUnityComponent altUnityComponent, AltUnityFieldsSelections altUnityFieldsSelections = AltUnityFieldsSelections.ALLFIELDS)
        {
            return new AltUnityGetAllFields(socketSettings, altUnityComponent, this, altUnityFieldsSelections).Execute();
        }
        public System.Collections.Generic.List<string> GetAllMethods(AltUnityComponent altUnityComponent, AltUnityMethodSelection methodSelection = AltUnityMethodSelection.ALLMETHODS)
        {
            return new AltUnityGetAllMethods(socketSettings, altUnityComponent, this, methodSelection).Execute();
        }
    }
}