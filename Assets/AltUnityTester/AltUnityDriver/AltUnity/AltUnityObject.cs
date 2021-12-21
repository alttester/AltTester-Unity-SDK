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
        public int transformParentId;
        public int transformId;
        [Newtonsoft.Json.JsonIgnore]
        public IDriverCommunication CommHandler;
        public AltUnityObject(string name, int id = 0, int x = 0, int y = 0, int z = 0, int mobileY = 0, string type = "", bool enabled = true, float worldX = 0, float worldY = 0, float worldZ = 0, int idCamera = 0, int transformParentId = 0, int transformId = 0)
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
            this.transformParentId = transformParentId;
            this.transformId = transformId;
        }

        public AltUnityObject getParent()
        {
            return new AltUnityFindObject(CommHandler, By.PATH, "//*[@id=" + this.id + "]/..", By.NAME, "", true).Execute();
        }
        public AltUnityVector2 getScreenPosition()
        {
            return new AltUnityVector2(x, y);
        }
        public AltUnityVector3 getWorldPosition()
        {
            return new AltUnityVector3(worldX, worldY, worldZ);
        }
        public T GetComponentProperty<T>(string componentName, string propertyName, string assemblyName = null, int maxDepth = 2)
        {
            return new AltUnityGetComponentProperty<T>(CommHandler, componentName, propertyName, assemblyName, maxDepth, this).Execute();
        }
        public void SetComponentProperty(string componentName, string propertyName, object value, string assemblyName = null)
        {
            new AltUnitySetComponentProperty(CommHandler, componentName, propertyName, value, assemblyName, this).Execute();
        }

        public T CallComponentMethod<T>(string componentName, string methodName, object[] parameters, string[] typeOfParameters = null, string assemblyName = null)
        {
            return new AltUnityCallComponentMethod<T>(CommHandler, componentName, methodName, parameters, typeOfParameters, assemblyName, this).Execute();
        }
        public string GetText()
        {
            return new AltUnityGetText(CommHandler, this).Execute();
        }
        public AltUnityObject SetText(string text, bool submit = false)
        {
            return new AltUnitySetText(CommHandler, this, text, submit).Execute();
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
            return new AltUnityClickElement(CommHandler, this, count, interval, wait).Execute();
        }

        public AltUnityObject PointerUpFromObject()
        {
            return new AltUnityPointerUpFromObject(CommHandler, this).Execute();
        }
        public AltUnityObject PointerDownFromObject()
        {
            return new AltUnityPointerDownFromObject(CommHandler, this).Execute();
        }
        public AltUnityObject PointerEnterObject()
        {
            return new AltUnityPointerEnterObject(CommHandler, this).Execute();
        }
        public AltUnityObject PointerExitObject()
        {
            return new AltUnityPointerExitObject(CommHandler, this).Execute();
        }

        /// <summary>
        /// Tap current object
        /// </summary>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        /// <returns>The tapped object</returns>
        public AltUnityObject Tap(int count = 1, float interval = 0.1f, bool wait = true)
        {
            return new AltUnityTapElement(CommHandler, this, count, interval, wait).Execute();
        }

        public System.Collections.Generic.List<AltUnityComponent> GetAllComponents()
        {
            return new AltUnityGetAllComponents(CommHandler, this).Execute();
        }
        public System.Collections.Generic.List<AltUnityProperty> GetAllProperties(AltUnityComponent altUnityComponent, AltUnityPropertiesSelections altUnityPropertiesSelections = AltUnityPropertiesSelections.ALLPROPERTIES)
        {
            return new AltUnityGetAllProperties(CommHandler, altUnityComponent, this, altUnityPropertiesSelections).Execute();
        }
        public System.Collections.Generic.List<AltUnityProperty> GetAllFields(AltUnityComponent altUnityComponent, AltUnityFieldsSelections altUnityFieldsSelections = AltUnityFieldsSelections.ALLFIELDS)
        {
            return new AltUnityGetAllFields(CommHandler, altUnityComponent, this, altUnityFieldsSelections).Execute();
        }
        public System.Collections.Generic.List<string> GetAllMethods(AltUnityComponent altUnityComponent, AltUnityMethodSelection methodSelection = AltUnityMethodSelection.ALLMETHODS)
        {
            return new AltUnityGetAllMethods(CommHandler, altUnityComponent, methodSelection).Execute();
        }
    }
}